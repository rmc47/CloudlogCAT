using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RigCAT.NET;

namespace CloudlogCAT
{
    public partial class ConnectionForm : Form
    {
        public ConnectionForm()
        {
            InitializeComponent();
        }

        public string LogbookURL
        {
            get { return m_LogbookURL.Text; }
        }

        public RadioModel Model
        {
            get { return (RadioModel)m_RadioType.SelectedItem; }
        }

        public string SerialPort
        {
            get { return m_SerialPort.SelectedItem.ToString(); }
        }

        public int Speed
        {
            get
            {
                int speed;
                int.TryParse(m_Speed.Text, out speed);
                return speed;
            }
        }

        public bool UseDTR
        {
            get { return m_EnableDTR.Checked; }
        }

        public bool UseRTS
        {
            get { return m_EnableRTS.Checked; }
        }

        private void m_Connect_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void ConnectionForm_Load(object sender, EventArgs e)
        {
            // Populate the list of radio models
            m_RadioType.BeginUpdate();
            m_RadioType.Items.Clear();
            foreach (RadioModel model in Enum.GetValues(typeof(RadioModel)))
            {
                m_RadioType.Items.Add(model);
            }
            m_RadioType.EndUpdate();
            m_RadioType.SelectedIndex = 0;

            // Populate the list of serial ports
            m_SerialPort.BeginUpdate();
            m_SerialPort.Items.Clear();
            foreach (string portName in System.IO.Ports.SerialPort.GetPortNames())
            {
                m_SerialPort.Items.Add(portName);
            }
            m_SerialPort.EndUpdate();
            if (m_SerialPort.Items.Count > 0)
                m_SerialPort.SelectedIndex = 0;

            // Populate flow control
            m_FlowControl.BeginUpdate();
            m_FlowControl.Items.Clear();
            foreach (FlowControl fc in Enum.GetValues(typeof(FlowControl)))
            {
                m_FlowControl.Items.Add(fc);
            }
            m_FlowControl.EndUpdate();
            m_FlowControl.SelectedIndex = 0;

            // Load from registry if applicable
            m_LogbookURL.Text = Settings.Get("LogbookURL", m_LogbookURL.Text);
            m_Speed.Text = Settings.Get("Speed", m_Speed.Text);
            RadioModel radioModel;
            if (Enum.TryParse<RadioModel>(Settings.Get("RadioType", null), out radioModel))
                m_RadioType.SelectedItem = radioModel;
            string serialPort = Settings.Get("SerialPort", null);
            if (serialPort != null && m_SerialPort.Items.Contains(serialPort))
                m_SerialPort.SelectedItem = serialPort;
            FlowControl flowControl;
            if (Enum.TryParse<FlowControl>(Settings.Get("FlowControl", null), out flowControl))
                m_FlowControl.SelectedItem = flowControl;
            m_EnableDTR.Checked = bool.Parse(Settings.Get("EnableDTR", "False"));
            m_EnableRTS.Checked = bool.Parse(Settings.Get("EnableRTS", "False"));
        }

        private void ConnectionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != System.Windows.Forms.DialogResult.OK)
                return;

            // Persist back to registry
            Settings.Set("LogbookURL", m_LogbookURL.Text);
            Settings.Set("Speed", m_Speed.Text);
            Settings.Set("RadioType", m_RadioType.SelectedItem.ToString());
            Settings.Set("SerialPort", m_SerialPort.SelectedItem.ToString());
            Settings.Set("FlowControl", m_FlowControl.SelectedItem.ToString());
            Settings.Set("EnableDTR", m_EnableDTR.Checked.ToString());
            Settings.Set("EnableRTS", m_EnableRTS.Checked.ToString());
        }
    }
}
