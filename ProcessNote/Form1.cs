using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ProcessNote
{
    public partial class Form1 : Form
    {
        private Dictionary<string, string> comments = new Dictionary<string, string>();
        
        
        

        public Form1()
        {
            InitializeComponent();
            listProcesses();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.TopMost = false;
        }

        private void listProcesses()
        {
            Process[] processlist = Process.GetProcesses();
            foreach (var process in processlist)
            {
                listBox1.Items.Add(process.ProcessName + " " + process.Id.ToString());
            }
            textBox1.Text = listBox1.Items.Count.ToString() + " - Processes Runnning";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listProcesses();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var tokens = listBox1.SelectedItem.ToString().Split(' ');
                string selectedProcess = tokens[0];
                using (PerformanceCounter pcProcess =
                    new PerformanceCounter("Process", "% Processor Time", selectedProcess))
                {
                    pcProcess.NextValue();
                    System.Threading.Thread.Sleep(500);
                    int memsize = 0; // memsize in Megabyte
                    PerformanceCounter PC = new PerformanceCounter();
                    PC.CategoryName = "Process";
                    PC.CounterName = "Working Set - Private";
                    PC.InstanceName = selectedProcess;
                    memsize = Convert.ToInt32(PC.NextValue()) / (int) (1024);
                    PC.Close();
                    PC.Dispose();
                    textBox3.Text = ("CPU--->" + (Math.Round(pcProcess.NextValue(), 1) + "%") + "\r\n");
                    textBox3.Text += "Memory--->" + memsize + "KB";
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            string curItem = listBox1.SelectedItem.ToString();
            textBox4.Text = curItem + "|\r\n";
            foreach(var item in comments)
            {
                if (item.Key.Equals(listBox1.SelectedItem.ToString()))
                {
                    textBox4.Text += item.Value;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var tokens = textBox4.Text.Split('|');
            System.Console.WriteLine(tokens[0]);
            System.Console.WriteLine(tokens[1]);
           comments.Add(tokens[0],tokens[1]);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = !this.TopMost;
        }
    }
}
