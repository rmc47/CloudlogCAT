using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.IO;
using System.Threading;

namespace RigCAT.NET.CAT
{
    public abstract class GenericCATRadio : IRadio
    {
        private RadioConnectionSettings ConnectionSettings { get; set; }
        private SerialPort m_SerialPort;
        private char[] m_ReceiveBuffer = new char[0x1000];
        private const bool c_EnableLogging = false;
        private StreamWriter m_LogWriter;
        private Thread m_PollThread;

        protected abstract string RadioModelName { get; }
        protected abstract string GetModeCommand { get; }
        
        public GenericCATRadio(RadioConnectionSettings connectionSettings)
        {
            ConnectionSettings = connectionSettings;
            m_SerialPort = new SerialPort { PortName = connectionSettings.Port, BaudRate = connectionSettings.BaudRate, DtrEnable = connectionSettings.UseDTR, RtsEnable = connectionSettings.UseRTS };
            switch (connectionSettings.FlowControl)
            {
                case FlowControl.None:
                    m_SerialPort.Handshake = Handshake.None;
                    break;
                case FlowControl.DsrDtr:
                case FlowControl.RtsCts:
                    m_SerialPort.Handshake = Handshake.RequestToSend;
                    break;
                case FlowControl.XonXoff:
                    m_SerialPort.Handshake = Handshake.XOnXOff;
                    break;
            }
            m_SerialPort.Open();

            if (c_EnableLogging)
            {
                string logsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Cloudlog\\Logs");
                if (!Directory.Exists(logsFolder))
                    Directory.CreateDirectory(logsFolder);
                string logPath = Path.Combine(logsFolder, "CloudlogCAT-" + DateTime.Now.ToString("yyyy-MM-dd-hhmmss") + ".txt");
                m_LogWriter = new StreamWriter(logPath);
                m_LogWriter.AutoFlush = true;
            }

            m_PollThread = new Thread(PollMethod);
            m_PollThread.IsBackground = true;
            m_PollThread.Start();
        }

        public event EventHandler<EventArgs> FrequencyChanged;

        private void PollMethod(object state)
        {
            long lastFrequency = 0;
            OperatingMode lastMode = OperatingMode.Unknown;
            while (true)
            {
                try
                {
                    long newFrequency = PrimaryFrequency;
                    OperatingMode newMode = PrimaryMode;
                    if (newFrequency != lastFrequency || newMode != lastMode)
                    {
                        if (FrequencyChanged != null)
                            FrequencyChanged(this, new EventArgs());
                    }
                    Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    ;
                }
            }
        }

        public long PrimaryFrequency
        {
            get
            {
                string s = SendQuery("FA;");
                if (s.Length < 4)
                    return 0;
                long freq;
                long.TryParse(s.Substring(2, s.Length - 3), out freq);
                return freq;
            }
            set
            {
                m_SerialPort.WriteLine(string.Format("FA{0:00000000000};", value));
            }
        }

        public long SecondaryFrequency
        {
            get
            {
                string s = SendQuery("FB;");
                if (s.Length < 4)
                    return 0;
                long freq;
                long.TryParse(s.Substring(2, s.Length - 3), out freq);
                return freq;
            }
            set
            {
                m_SerialPort.WriteLine(string.Format("FB{0:00000000000};", value));
            }
        }

        public OperatingMode PrimaryMode
        {
            get
            {
                string response = SendQuery(GetModeCommand);
                if (response.Length < 4)
                    return OperatingMode.Unknown;

                int modeInt;
                if (int.TryParse(response.Substring(2, response.Length - 3), out modeInt))
                    return (OperatingMode)modeInt;
                else
                    return OperatingMode.Unknown;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void EqualiseVFOs()
        {
            ;
        }

        protected string SendQuery(string cmd)
        {
            return SendQuery(cmd, true);
        }

        protected string SendQuery(string cmd, bool expectReply)
        {
            lock (m_SerialPort)
            {
                if (c_EnableLogging)
                    m_LogWriter.WriteLine("SEND: " + cmd);

                // Check the serial port is open
                if (!m_SerialPort.IsOpen)
                {
                    m_SerialPort.Open();
                }

                // Check there's nothing outstanding in the buffer first
                if (m_SerialPort.BytesToRead > 0)
                {
                    m_SerialPort.DiscardInBuffer();
                }

                // Issue the command
                m_SerialPort.WriteLine(cmd);

                // Wait for the reply
                if (expectReply)
                {
                    int justRead = 1;
                    int pos = 0;
                    m_SerialPort.ReadTimeout = 2000;
                    while (justRead > 0)
                    {
                        justRead = m_SerialPort.Read(m_ReceiveBuffer, pos, m_ReceiveBuffer.Length - pos);
                        pos += justRead;

                        // If we ended with the end of the command, stop trying to read more...
                        if (m_ReceiveBuffer[pos - 1] == ';')
                        {
                            break;
                        }
                    }

                    // Turn it into a string
                    string response = new string(m_ReceiveBuffer, 0, pos);

                    if (c_EnableLogging)
                        m_LogWriter.WriteLine("RAW: " + response);

                    // Trim any excess garbage (Looking at you, FT-950)
                    int responseStart = response.IndexOf(cmd.Substring(0, 2));
                    int responseEnd;
                    if (responseStart >= 0)
                        responseEnd = response.IndexOf(';', responseStart);
                    else
                        responseEnd = -1;

                    if (responseStart < 0 || responseEnd < 0)
                    {
                        if (c_EnableLogging)
                            m_LogWriter.WriteLine("RCV: [Command not found]");
                        return string.Empty;
                    }
                    else
                    {
                        string trimmedResponse = response.Substring(responseStart, responseEnd - responseStart + 1);
                        if (c_EnableLogging)
                            m_LogWriter.WriteLine("RCV: " + trimmedResponse);
                        return trimmedResponse;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public override string ToString()
        {
            return string.Format("{2}, {0}, {1}", m_SerialPort.PortName, m_SerialPort.BaudRate, RadioModelName);
        }
    }
}
