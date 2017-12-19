using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MarkChristopher.AutoDetectPort;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace Sample
{
    public partial class Form1 : Form
    {
        int i = 0;
        public Form1()
        {
            InitializeComponent();

            //serialPortChanges.DeviceChange += Form1_DeviceChange;
        }
        SerialPortChanges serialPortChanges;

        private void Form1_Load(object sender, EventArgs e)
        {


        }
        Thread thread;
        private void Form1_DeviceChange(object sender, EventArgs e)
        {

            thread = new Thread(new ThreadStart(() => { System.Threading.Thread.Sleep(1000); Debug.WriteLine("Detected"); }));
            thread.Start();



        }

        private void button1_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            serialPortChanges = new SerialPortChanges();
            if (btn.Text != "running")
            {
                var eventDeviceChange = new SerialPortChanges.EventDeviceChange();
                eventDeviceChange.DeviceChange += EventDeviceChange_DeviceChange;
                serialPortChanges.start(eventDeviceChange);
                btn.Text = "running";
            }
            else
            {
                serialPortChanges.Stop();
                btn.Text = "Start";
            }


        }

        private void EventDeviceChange_DeviceChange(object sender, EventArgs e)
        {
            i++;
            this.Invoke(new Action(() =>
            {
                listBox1.Items.Add("Detected Flash Drives " + i);

            }));
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        int copy = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            var files = Directory.EnumerateFiles(textBox1.Text);
            var subdirectory = textBox1.Text + "/" + new Random().Next(0, 1000);
            Directory.CreateDirectory(subdirectory);
            foreach (var i in files)
            {

                var filename = subdirectory + "/" + "nr_" + Path.GetFileName(i).Split(' ')[0] + Path.GetExtension(i);

                copy = 0;
                if (File.Exists(filename))
                {

                    foreach (var c in Directory.EnumerateFiles(subdirectory))
                    {
                        if (c.Contains(Path.GetFileName(i).Split(' ')[0]))
                        {
                            copy++;
                        }
                    }

                    filename = subdirectory + "/" + "nr_" + Path.GetFileName(i).Split(' ')[0] + "_" + copy + Path.GetExtension(i);
                    File.Copy(i, filename);
                }
                else
                {
                    File.Copy(i, filename);
                }

            }
        }
    }
}
