using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace RigCAT.NET.Elecraft
{
    internal sealed class K3 : IRadio
    {
        private RadioConnectionSettings ConnectionSettings { get; set; }
        private SerialPort m_SerialPort;
        private char[] m_ReceiveBuffer = new char[0x1000];

        public K3(RadioConnectionSettings connectionSettings)
        {
            ConnectionSettings = connectionSettings;
            m_SerialPort = new SerialPort { PortName = connectionSettings.Port, BaudRate = connectionSettings.BaudRate, DtrEnable = connectionSettings.UseDTR, RtsEnable = connectionSettings.UseRTS };
            m_SerialPort.Open();
        }

        public event EventHandler<EventArgs> FrequencyChanged;

        public long PrimaryFrequency
        {
            get
            {
                string s = SendQuery("FA;");
                long freq;
                long.TryParse(s.Substring(2, s.Length - 3), out freq);
                return freq;
            }
            set
            {
                m_SerialPort.WriteLine(string.Format("FA{0:00000000000};", value));
            }
        }

        public OperatingMode PrimaryMode
        {
            get
            {
                string response = SendQuery("MD;");
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

        private string SendQuery(string cmd)
        {
            lock (m_SerialPort)
            {
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

                return response;
            }
        }

        public override string ToString()
        {
            return string.Format("Elecraft K3, {0}, {1}", m_SerialPort.PortName, m_SerialPort.BaudRate);
        }
    }
}
