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
using System.Diagnostics;
using System.Threading;

namespace GPSdisplay
{
    public partial class Form1 : Form
    {
        static NMEAData ActualData = new NMEAData();
        SerialPort Port = new SerialPort("COM5", 9600, Parity.None, 8, StopBits.One);

        public Form1()
        {
            InitializeComponent();
            Port.Open();
            Thread trd = new Thread(new ThreadStart(this.ThreadTask));
            trd.IsBackground = true;
            trd.Start();
            Thread.Sleep(300);
            update();

        }

        private void ThreadTask()
        {
            while (true)
            {
                string tmp = Port.ReadLine();
                ActualData.parse(tmp);
                Thread.Sleep(100);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            update();
        }

        private void update()
        {
            latitude.Text = ActualData.latitude;
            longitude.Text = ActualData.longitude;
            linkLabel2.Text = ActualData.mapslink;
            label6.Text = ActualData.fix;
            label15.Text = ActualData.numberofsatelites;
            label7.Text = ActualData.trackedsatelites;
            label16.Text = ActualData.pdop;
            label17.Text = ActualData.vdop;
            label8.Text = ActualData.hdop;
            label9.Text = ActualData.altitude;

            webBrowser1.Navigate(ActualData.mapslink);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(ActualData.mapslink);
        }


        
    }
}
