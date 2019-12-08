using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
namespace Chat_Serial
{
    public partial class Form1 : Form
    {
        Boolean dataReceived = false;
        int latestBytes = -1;
        string recv;
        public Form1()
        {
            InitializeComponent();
        }
        private void refreshSerial()
        {
            try
            {
                string[] ports = SerialPort.GetPortNames();
                foreach (string port in ports)
                {
                    comboBox1.Items.Add(port);
                }
                textBox1.Text = "9600";
                comboBox1.SelectedIndex = 0;
            }
            catch(Exception e)
            {
                MessageBox.Show("ERROR : " + e.Message, "Chatting Apps", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            refreshSerial();
            serialPort1.DataReceived += SerialPort1_DataReceived;
        }
        private delegate void AddItemCallback(object o);
        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if(serialPort1.BytesToRead == latestBytes)
            {
                dataReceived = true;
            }
            else
            {
                latestBytes = serialPort1.BytesToRead;
            }
            //if(serialPort1.BytesToRead != latestBytes)
            //{
            //    latestBytes = serialPort1.BytesToRead;
            //}
            //else
            //{
            //    dataReceived = true;
            //}
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    serialPort1.PortName = comboBox1.Text;
                    serialPort1.BaudRate = Convert.ToInt32(textBox1.Text);
                    serialPort1.NewLine = "\n";
                    serialPort1.Open();
                    button1.Text = "CLOSE";
                    timer1.Enabled = true;

                }
                else
                {
                    button1.Text = "OPEN";
                    serialPort1.DiscardInBuffer();
                    serialPort1.DiscardOutBuffer();
                    serialPort1.Close();
                    timer1.Enabled = false;

                }
            }
            catch(Exception er)
            {
                MessageBox.Show("ERROR : " + er.Message, "Chatting Apps", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (dataReceived)
            {

                listBox1.Items.Add("Irwin : " + serialPort1.ReadExisting());
                dataReceived = false;
                if(listBox1.Items.Count > 10)
                {
                    listBox1.Items.Remove(0);
                }
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }
            
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            serialPort1.Write(richTextBox1.Text+"\n");
            listBox1.Items.Add("Rachmat : " + richTextBox1.Text);
        }
    }
}
