using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialPortMosas
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (var serialPort in SerialPort.GetPortNames())
            {
                comboBoxPorts.Items.Add(serialPort);
            }
            comboBoxPorts.SelectedIndex = 0;
            buttonDisconnect.Enabled = false;
            buttonSend.Enabled = false;
        }
        private void buttonConnect_Click(object sender, EventArgs e)
        {
            serialPort1.PortName = comboBoxPorts.Text;
            serialPort1.BaudRate = 9600;
            serialPort1.Parity = Parity.Even;
            serialPort1.StopBits = StopBits.One;
            serialPort1.DataBits = 8;

            buttonConnect.Enabled = false;

            try
            {
                serialPort1.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Seri Port bağlantısı yapılamadı \n Hata:{ ex.Message}","Hata",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            if(serialPort1.IsOpen)
            {
                buttonConnect.Enabled = false;
                buttonDisconnect.Enabled = true;
                buttonSend.Enabled = true;
            }         
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(serialPort1.IsOpen)
            {
                serialPort1.Close();
                buttonConnect.Enabled = true;
                buttonDisconnect.Enabled = false;
            }
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if(serialPort1.IsOpen)
            {
                serialPort1.Write(textBoxSendData.Text);
                textBoxSendData.Clear();
            }
        }
        public delegate void ShowData(string data);
        public void writeToTextbox(string data)
        {
            textBoxShowData.Text += data;
        }
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            String receivedData = serialPort1.ReadExisting();
            textBoxShowData.Invoke(new ShowData(writeToTextbox), receivedData);
        }
    }
}
