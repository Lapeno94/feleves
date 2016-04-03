using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO.Ports;
using OpenHardwareMonitor.Hardware;
using OpenHardwareMonitor;

namespace performanceCounter
{
    public partial class Form1 : Form
    {

        public int x { get; set; }                                          //x koordináta pixelben

        public int y { get; set; }


        List<Point> cpu_pt = new List<Point>();
        List<Point> ram_pt = new List<Point>();

        List<Point> cpu1_pt = new List<Point>();
        List<Point> cpu2_pt = new List<Point>();
        List<Point> cpu3_pt = new List<Point>();
        List<Point> cpu4_pt = new List<Point>();

        public int ram_val { get; set; }

        public int test { get; set; }

        List<string> testvalues = new List<string>() {"15","35","60","80","100" };

        Computer thisComputer;

        public int plusten { get; set; }

        public int minusten { get; set; }



        public Form1()
        {
            InitializeComponent();

            thisComputer = new Computer() { CPUEnabled = true };

            thisComputer.Open();

            //pictureBox1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            //this.pictureBox1.Padding = new System.Windows.Forms.Padding(1);
            getAvaiablePorts();
        }

        private void getAvaiablePorts()
        {
            String[] ports = SerialPort.GetPortNames();
            comboBox1.Items.AddRange(ports);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;                                //eseményre
            if(x>pictureBox1.Width || x>pictureBox2.Width)         //ha eléri a végét újra rajzoljuk
            {
                x = 0;
                cpu_pt.Clear();
                ram_pt.Clear();
            }

            g.FillRectangle(new HatchBrush(HatchStyle.Cross, Color.Gray), pictureBox1.ClientRectangle);
            if (cpu_pt.Count>1)
            {
                g.DrawLines(new Pen(new SolidBrush(Color.FromArgb(255, 0, 255, 100))), cpu_pt.ToArray());
            }
            
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;                                //eseményre
            //if (x > pictureBox1.Width)         //ha eléri a végét újra rajzoljuk
            //{
            //    x = 0;
            //    cpu_pt.Clear();
            //    ram_pt.Clear();
            //}

            g.FillRectangle(new HatchBrush(HatchStyle.Cross, Color.Gray), pictureBox2.ClientRectangle);
            if (ram_pt.Count>1)
            {
                g.DrawLines(new Pen(new SolidBrush(Color.FromArgb(255, 0, 255, 100))), ram_pt.ToArray());
            }
            
        }

        public void timer1_Tick(object sender, EventArgs e)
        {
            x += 2;
            y += 1;
            int cpu_val = (pictureBox1.Height * (int)Math.Round(cpu.NextValue())) / 100;
            ram_val = (pictureBox2.Height * (int)Math.Round(ram.NextValue())) / 100;

            int cpu1_val = (pictureBox3.Height * (int)Math.Round(cpu1.NextValue())) / 100;
            int cpu2_val = (pictureBox4.Height * (int)Math.Round(cpu2.NextValue())) / 100;
            int cpu3_val = (pictureBox5.Height * (int)Math.Round(cpu3.NextValue())) / 100;
            int cpu4_val = (pictureBox6.Height * (int)Math.Round(cpu4.NextValue())) / 100;

            cpu_pt.Add(new Point(x, pictureBox1.Height - cpu_val));                         //megcsináltok a graf linejait
            ram_pt.Add(new Point(x, pictureBox2.Height - ram_val));

            cpu1_pt.Add(new Point(y, pictureBox3.Height - cpu1_val));
            cpu2_pt.Add(new Point(y, pictureBox4.Height - cpu2_val));
            cpu3_pt.Add(new Point(y, pictureBox5.Height - cpu3_val));
            cpu4_pt.Add(new Point(y, pictureBox6.Height - cpu4_val));

            //public int rampwm { get; set { rampwm = value; }; }





        label3.Text = cpu_val.ToString()+"%";

            label9.Text = cpu1_val.ToString() + "%";
            label10.Text = cpu2_val.ToString() + "%";
            label11.Text = cpu3_val.ToString() + "%";
            label12.Text = cpu4_val.ToString() + "%";
            label4.Text = ram_val.ToString() + "%";

            String temp = "";

            foreach (var hardwareItem in thisComputer.Hardware)
            {
                if (hardwareItem.HardwareType == HardwareType.CPU)
                {
                    hardwareItem.Update();
                    foreach (IHardware subHardware in hardwareItem.SubHardware)
                        subHardware.Update();

                    foreach (var sensor in hardwareItem.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature)
                        {

                            temp += String.Format("{0} Temperature = {1}°C\r\n", sensor.Name, sensor.Value.HasValue ? sensor.Value.Value.ToString() : "no value");

                        }
                    }
                }
            }

            textBox2.Text = temp;



            //pictureBox1.Invalidate();                       //újrarajzoljuk   CPU TOTAL
            //pictureBox2.Invalidate();
            //pictureBox3.Invalidate();
            //pictureBox4.Invalidate();
            //pictureBox5.Invalidate();
            //pictureBox6.Invalidate();

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    ((PictureBox)x).Invalidate(); 
                }
            }



        }

        private void pictureBox3_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;                                //eseményre

            if (y > pictureBox3.Width || y > pictureBox4.Width || y > pictureBox5.Width || y > pictureBox6.Width)         //ha eléri a végét újra rajzoljuk
            {
                y = 0;
                cpu1_pt.Clear();
                cpu2_pt.Clear();
                cpu3_pt.Clear();
                cpu4_pt.Clear();
                //
            }

            g.FillRectangle(new HatchBrush(HatchStyle.Cross, Color.Gray), pictureBox3.ClientRectangle);
            if (cpu1_pt.Count > 1)
            {
                g.DrawLines(new Pen(new SolidBrush(Color.FromArgb(255, 0, 255, 100))), cpu1_pt.ToArray());
            }
        }

        private void pictureBox4_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;                                //eseményre

            g.FillRectangle(new HatchBrush(HatchStyle.Cross, Color.Gray), pictureBox4.ClientRectangle);
            if (cpu2_pt.Count > 1)
            {
                g.DrawLines(new Pen(new SolidBrush(Color.FromArgb(255, 0, 255, 100))), cpu2_pt.ToArray());
            }

        }

        private void pictureBox5_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;                                //eseményre

            g.FillRectangle(new HatchBrush(HatchStyle.Cross, Color.Gray), pictureBox5.ClientRectangle);
            if (cpu3_pt.Count > 1)
            {
                g.DrawLines(new Pen(new SolidBrush(Color.FromArgb(255, 0, 255, 100))), cpu3_pt.ToArray());
            }

        }

        private void pictureBox6_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;                                //eseményre

            g.FillRectangle(new HatchBrush(HatchStyle.Cross, Color.Gray), pictureBox6.ClientRectangle);
            if (cpu4_pt.Count > 1)
            {
                g.DrawLines(new Pen(new SolidBrush(Color.FromArgb(255, 0, 255, 100))), cpu4_pt.ToArray());
            }

        }



        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Text == "")
                {
                    MessageBox.Show("Töltse ki");
                }
                else
                {                                                   // alapbeállítások
                    serialPort1.PortName = comboBox1.Text;
                    serialPort1.BaudRate = 115200;
                    serialPort1.Open();

                }
            }
            catch (UnauthorizedAccessException) { MessageBox.Show("nem sikerült elérni"); }

            button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            serialPort1.WriteLine("pwmr "+ ram_val.ToString()+".0");
            serialPort1.WriteLine("pwmg " + ram_val.ToString() + ".0");
            serialPort1.WriteLine("pwmb " + ram_val.ToString() + ".0");

            label13.Text = ram_val.ToString();
            plusten = ram_val;
            minusten = ram_val;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            test = ram_val+30;

            //for (int i = 0; i < testvalues.Count; i++)
            //{
            //    serialPort1.WriteLine("pwmr " + testvalues + ".0");
            //}

            if (20 <= test && test <= 100)
            {
                serialPort1.WriteLine("pwmr " + test.ToString() + ".0");
                serialPort1.WriteLine("pwmg " + test.ToString() + ".0");
                serialPort1.WriteLine("pwmb " + test.ToString() + ".0");
            }
            else
            {
                test = 20;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int number;
            if (int.TryParse(textBox1.Text,out number))
            {
                if (15<=number && number<=100)
                {
                    MessageBox.Show("Good");

                   
                    serialPort1.WriteLine("pwmr " + number.ToString() + ".0");
                    serialPort1.WriteLine("pwmg " + number.ToString() + ".0");
                    serialPort1.WriteLine("pwmb " + number.ToString() + ".0");

                }
                else
                {
                    MessageBox.Show("Please use number between 15-100");
                }
            }
            else
            {
                MessageBox.Show("Please use number between 15-100");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            plusten = plusten+10;
            if (15<=plusten&&plusten<=100)
            {
                serialPort1.WriteLine("pwmr " + plusten.ToString() + ".0");
                serialPort1.WriteLine("pwmg " + plusten.ToString() + ".0");
                serialPort1.WriteLine("pwmb " + plusten.ToString() + ".0");
            }
            else
            {
                plusten = 100;
                serialPort1.WriteLine("pwmr " + "100" + ".0");
                serialPort1.WriteLine("pwmg " + "100" + ".0");
                serialPort1.WriteLine("pwmb " + "100" + ".0");
            }

            label13.Text = plusten.ToString()+"%";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            minusten = minusten - 10;
            if (15 <= minusten && minusten <= 100)
            {
                serialPort1.WriteLine("pwmr " + minusten.ToString() + ".0");
                serialPort1.WriteLine("pwmg " + minusten.ToString() + ".0");
                serialPort1.WriteLine("pwmb " + minusten.ToString() + ".0");
            }
            else
            {
                minusten = 15;
                serialPort1.WriteLine("pwmr " + "15" + ".0");
                serialPort1.WriteLine("pwmg " + "15" + ".0");
                serialPort1.WriteLine("pwmb " + "15" + ".0");
            }

            label13.Text = minusten.ToString() + "%";
        }
    }
}
