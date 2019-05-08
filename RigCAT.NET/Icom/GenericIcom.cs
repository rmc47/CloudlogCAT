using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace RigCAT.NET.Icom
{
    public class GenericIcom : IRadio, IVoiceKeyer, IWinKey
    {
        private SerialPort m_Port;

        private volatile bool m_Stopping;
        public event EventHandler<EventArgs> FrequencyChanged;
        private Thread m_MonitorThread;
        private long m_Frequency;
        private OperatingMode m_Mode;
        private readonly ManualResetEvent m_CommandReadResetEvent = new ManualResetEvent(false);

        public long PrimaryFrequency
        {
            get {
                if (m_Frequency == 0)
                    QueryFrequency();
                return m_Frequency; 
            }
            set {
                SelectVfoA();
                m_Frequency = value;
                SetFrequency();
            }
        }

        public long SecondaryFrequency
        {
            get
            {
                if (m_Frequency == 0)
                    QueryFrequency();
                return m_Frequency;
            }
            set
            {
                SelectVfoB();
                m_Frequency = value;
                SetFrequency();
            }
        }

        public OperatingMode PrimaryMode
        {
            get
            {
                if (m_Mode == OperatingMode.Unknown)
                    QueryMode();
                return m_Mode;
            }
            set
            {
                ;
            }
        }

        public GenericIcom(RadioConnectionSettings connectionSettings)
        {
            string[] ports = SerialPort.GetPortNames();

            m_Port = new SerialPort(connectionSettings.Port);
            m_Port.BaudRate = connectionSettings.BaudRate;
            m_Port.DtrEnable = connectionSettings.UseDTR;
            m_Port.RtsEnable = connectionSettings.UseRTS;
            m_Port.Open();

            m_MonitorThread = new Thread(ReadStream);
            m_MonitorThread.IsBackground = true;
            m_MonitorThread.Start();
        }

        public void QueryFrequency()
        {
            m_CommandReadResetEvent.Reset();
            byte[] buff = new byte []{ 0xFE, 0xFE, 0x00, 0xE0, 0x03, 0xFD};
            m_Port.Write (buff, 0, buff.Length);
            m_CommandReadResetEvent.WaitOne(500);
            m_CommandReadResetEvent.Reset();
            buff[4] = 0x04;
            // Wait for the buffer to empty or we get a collision on the wire
            //while (m_Port.BytesToRead > 0)
            //    Thread.Sleep(100);
            m_Port.Write(buff, 0, buff.Length);
            m_CommandReadResetEvent.WaitOne(500);
        }

        private void SetFrequency()
        {
            m_CommandReadResetEvent.Reset();

            byte[] bcdFrequency = FrequencyToBcd(this.PrimaryFrequency);
            byte[] header = new byte[] { 0xFE, 0xFE, 0x00, 0xE0, 0x05 };
            byte[] buff = new byte[header.Length + bcdFrequency.Length + 1];
            Buffer.BlockCopy(header, 0, buff, 0, header.Length);
            Buffer.BlockCopy(bcdFrequency, 0, buff, header.Length, bcdFrequency.Length);
            Buffer.SetByte(buff, buff.Length - 1, 0xFD);
            
            m_Port.Write(buff, 0, buff.Length);
            m_CommandReadResetEvent.WaitOne(500);
        }

        private void SelectVfoB()
        {
            m_CommandReadResetEvent.Reset();

            byte[] buff = new byte[] { 0xFE, 0xFE, 0x00, 0xE0, 0x07, 0x01, 0xFD };

            m_Port.Write(buff, 0, buff.Length);
            m_CommandReadResetEvent.WaitOne(500);
        }

        private void SelectVfoA()
        {
            m_CommandReadResetEvent.Reset();

            byte[] buff = new byte[] { 0xFE, 0xFE, 0x00, 0xE0, 0x07, 0x00, 0xFD };

            m_Port.Write(buff, 0, buff.Length);
            m_CommandReadResetEvent.WaitOne(500);
        }

        public void EqualiseVFOs()
        {
            m_CommandReadResetEvent.Reset();

            byte[] buff = new byte[] { 0xFE, 0xFE, 0x00, 0xE0, 0x07, 0xA0, 0xFD };

            m_Port.Write(buff, 0, buff.Length);
            m_CommandReadResetEvent.WaitOne(500);
        }

        public void QueryMode()
        {
            m_CommandReadResetEvent.Reset();
            byte[] buff = new byte[] { 0xFE, 0xFE, 0x00, 0xE0, 0x04, 0xFD };
            // Wait for the buffer to empty or we get a collision on the wire
            //while (m_Port.BytesToRead > 0)
            //    Thread.Sleep(100);
            m_Port.Write(buff, 0, buff.Length);
            m_CommandReadResetEvent.WaitOne(500);
        }

        private void ReadStream()
        {
            try
            {
                byte[] buff = new byte[1024];
                while (!m_Stopping)
                {
                    int i;
                    for (i = 0; i < buff.Length; i++)
                    {
                        while (m_Port.BytesToRead == 0)
                        {
                            Thread.Sleep(100);
                            if (m_Stopping)
                                return;
                        }

                        m_Port.Read(buff, i, 1);
                        if (buff[i] == 0xFD)
                            break; // End of a command
                    }
                    ProcessCommand(buff, i);
                    m_CommandReadResetEvent.Set();
                }
            }
            catch (Exception)
            {
            }
        }

        private void ProcessCommand(byte[] buff, int cb)
        {
            try
            {
                byte to = buff[2];
                byte from = buff[3];
                byte cmd = buff[4];
                switch (cmd)
                {
                    case 0x00: // Set frequency
                    case 0x03:
                        if (cb > 5)
                        {
                            long freq = ParseBcd(buff, 5, cb - 5);
                            if (freq > 0 && freq < 10000000000)
                            {
                                m_Frequency = freq;
                                m_CommandReadResetEvent.Set();
                                if (FrequencyChanged != null)
                                    FrequencyChanged(this, new EventArgs());
                            }
                        }
                        break;
                    case 0x01:
                    case 0x04:
                        if (cb > 5)
                        {
                            OperatingMode mode;
                            switch (buff[5])
                            {
                                case 0x00:
                                    mode = OperatingMode.LSB;
                                    break;
                                case 0x01:
                                    mode = OperatingMode.USB;
                                    break;
                                case 0x03:
                                case 0x07:
                                    mode = OperatingMode.CW;
                                    break;
                                case 0x05:
                                case 0x06:
                                    mode = OperatingMode.FM;
                                    break;
                                case 0x04:
                                    mode = OperatingMode.Data;
                                    break;
                                default:
                                    mode = OperatingMode.Unknown;
                                    break;
                            }
                            if (mode != OperatingMode.Unknown)
                            {
                                m_Mode = mode;
                            }
                            m_CommandReadResetEvent.Set();
                            if (FrequencyChanged != null)
                                FrequencyChanged(this, new EventArgs());
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (System.IO.IOException)
            {
                throw;
            }
            catch
            {
            }
        }

        public static long ParseBcd(byte[] buff, int offset, int count)
        {
            long result = 0;
            for (int i = offset+count-1; i >= offset; i--)
            {
                result *= 100;
                result += buff[i] & 0x0F;
                result += ((buff[i] & 0xF0) >> 4) * 10;
            }
            return result;
        }

        public static byte[] FrequencyToBcd(long freq)
        {
            int chunks = (int)Math.Floor(Math.Log10(freq) / 2) + 1;
            byte[] bytes = new byte[chunks];
            for (int i = 0; i < chunks; i++)
            {
                int rem = (int)(freq % 100);
                freq /= 100;


                bytes[i] = (byte)((rem % 10) + ((rem / 10) << 4));
            }
            return bytes;
        }

        protected string RadioModelName
        {
            get { return "Generic Icom"; }
        }
        public override string ToString()
        {
            return string.Format("{2}, {0}, {1}", m_Port.PortName, m_Port.BaudRate, RadioModelName);
        }

        public void Dispose()
        {
            m_Stopping = true;
        }

        public void SendDvk(int messageNumber)
        {
            m_CommandReadResetEvent.Reset();

            byte[] buff = new byte[] { 0xFE, 0xFE, 0x00, 0xE0, 0x28, 0x00, (byte)messageNumber, 0xFD };

            m_Port.Write(buff, 0, buff.Length);
            m_CommandReadResetEvent.WaitOne(500);
        }

        public void CancelDvk()
        {
            m_CommandReadResetEvent.Reset();

            byte[] buff = new byte[] { 0xFE, 0xFE, 0x00, 0xE0, 0x28, 0x00, 0x00, 0xFD };

            m_Port.Write(buff, 0, buff.Length);
            m_CommandReadResetEvent.WaitOne(500);
        }

        public void SendString(string str)
        {
            m_CommandReadResetEvent.Reset();

            byte[] header = new byte[] { 0xFE, 0xFE, 0x00, 0xE0, 0x17 };
            byte[] strBytes = Encoding.ASCII.GetBytes(str);

            byte[] msg = new byte[header.Length + strBytes.Length + 1];
            Buffer.BlockCopy(header, 0, msg, 0, header.Length);
            Buffer.BlockCopy(strBytes, 0, msg, header.Length, strBytes.Length);
            msg[msg.Length - 1] = 0xFD;

            m_Port.Write(msg, 0, msg.Length);
            m_CommandReadResetEvent.WaitOne(500);
        }

        public void StopSending()
        {
            m_CommandReadResetEvent.Reset();

            byte[] buff = new byte[] { 0xFE, 0xFE, 0x00, 0xE0, 0x17, 0xFF, 0xFD };

            m_Port.Write(buff, 0, buff.Length);
            m_CommandReadResetEvent.WaitOne(500);
        }
    }
}
