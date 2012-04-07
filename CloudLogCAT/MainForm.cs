using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RigCAT.NET;
using CloudlogCAT.API;
using System.Threading;

namespace CloudlogCAT
{
    public partial class MainForm : Form
    {
        private IRadio m_Radio;
        private CloudlogAPI m_API;
        private Thread m_UpdateThread;

        public MainForm()
        {
            InitializeComponent();
        }
        
        private void MainForm_Shown(object sender, EventArgs e)
        {
            RadioFactory radioFactory = new RadioFactory();
            RadioConnectionSettings rcs = new RadioConnectionSettings();
            RadioModel model;
            using (ConnectionForm connForm = new ConnectionForm())
            {
                DialogResult dr = connForm.ShowDialog(this);
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    model = connForm.Model;
                    rcs.BaudRate = connForm.Speed;
                    rcs.Port = connForm.SerialPort;
                    rcs.UseDTR = connForm.UseDTR;
                    rcs.UseRTS = connForm.UseRTS;
                }
                else
                {
                    Close();
                    return;
                }
            }

            m_Radio = radioFactory.GetRadio(model, rcs);

            m_RadioLabel.Text = m_Radio.ToString();

            m_API = new CloudlogAPI();
            m_UpdateThread = new Thread(UpdateFrequency);
            m_UpdateThread.IsBackground = true;
            m_UpdateThread.Start();
        }

        private void m_Update_Click(object sender, EventArgs e)
        {
            m_API.PushCAT(new CATModel { Frequency = m_Radio.PrimaryFrequency, Timestamp = DateTime.UtcNow, Mode = m_Radio.PrimaryMode.ToString(), Radio = "K3" });
        }

        private static string FormatFrequency(long freq)
        {
            double freqInMhz = freq / 1e6;
            return string.Format("{0} MHz", freqInMhz);
        }

        private static long ParseFrequency(string freq)
        {
            double freqInMhz;
            if (double.TryParse(freq, out freqInMhz))
                return (long)(freqInMhz * 1e6);
            else
                return 0;
        }

        private void UpdateFrequency()
        {
            long previousFrequency = 0;
            OperatingMode previousMode = OperatingMode.Unknown;
            while (!Disposing && !IsDisposed)
            {
                try
                {
                    long frequency = m_Radio.PrimaryFrequency;
                    OperatingMode mode = m_Radio.PrimaryMode;

                    if (frequency != previousFrequency || mode != previousMode)
                    {
                        // Sanity check: re-read and make sure we get the same values!
                        long checkFrequency = m_Radio.PrimaryFrequency;
                        OperatingMode checkMode = m_Radio.PrimaryMode;

                        if (checkFrequency == frequency && checkMode == mode)
                        {
                            // Updated frequency or mode
                            CATModel model = new CATModel
                            {
                                Frequency = frequency,
                                Mode = mode.ToString(),
                                Timestamp = DateTime.UtcNow,
                                Radio = m_Radio.ToString()
                            };
                            m_API.PushCAT(model);

                            previousFrequency = frequency;
                            previousMode = mode;
                        }
                    }

                    // Update the UI
                    Invoke(new MethodInvoker(() =>
                    {
                        m_Status.Text = "Successfully updated at " + DateTime.Now.ToLongTimeString();
                        m_Frequency.Text = FormatFrequency(frequency);
                        m_Mode.Text = mode.ToString();
                    }));
                }
                catch (Exception ex)
                {
                    Invoke(new MethodInvoker(() =>
                    {
                        m_Status.Text = ex.Message;
                    }));
                }
                Thread.Sleep(1000);
            }
        }
    }
}
