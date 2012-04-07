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
            m_RadioType.Items.Add(RadioModel.ElecraftK3);
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
        }
    }
}
