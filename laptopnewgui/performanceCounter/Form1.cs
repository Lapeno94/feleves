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

        public int X { get; set; }                                          //x koordináta pixelben

        public int Y { get; set; }


        List<Point> _cpuPt = new List<Point>();
        List<Point> _ramPt = new List<Point>();

        List<Point> _cpu1Pt = new List<Point>();
        List<Point> _cpu2Pt = new List<Point>();
        List<Point> _cpu3Pt = new List<Point>();
        List<Point> _cpu4Pt = new List<Point>();

        public int RamVal { get; set; }

        public int Test { get; set; }

        List<string> _testvalues = new List<string>() {"15","35","60","80","100" };

        Computer _thisComputer;

        public int Plusten { get; set; }





        public Form1()
        {
            InitializeComponent();

            _thisComputer = new Computer() { CPUEnabled = true };

            _thisComputer.Open();

            //pictureBox1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            //this.pictureBox1.Padding = new System.Windows.Forms.Padding(1);
            GetAvaiablePorts();
        }

        private void GetAvaiablePorts()
        {
            String[] ports = SerialPort.GetPortNames();
            comboBox1.Items.AddRange(ports);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;                                //eseményre
            if(X>pictureBox1.Width || X>pictureBox2.Width)         //ha eléri a végét újra rajzoljuk
            {
                X = 0;
                _cpuPt.Clear();
                _ramPt.Clear();
            }

            g.FillRectangle(new HatchBrush(HatchStyle.Cross, Color.Gray), pictureBox1.ClientRectangle);
            if (_cpuPt.Count>1)
            {
                g.DrawLines(new Pen(new SolidBrush(Color.FromArgb(255, 0, 255, 100))), _cpuPt.ToArray());
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
            if (_ramPt.Count>1)
            {
                g.DrawLines(new Pen(new SolidBrush(Color.FromArgb(255, 0, 255, 100))), _ramPt.ToArray());
            }
            
        }

        public void timer1_Tick(object sender, EventArgs e)
        {
            X += 2;
            Y += 1;
            int cpuVal = (pictureBox1.Height * (int)Math.Round(cpu.NextValue())) / 100;
            RamVal = (pictureBox2.Height * (int)Math.Round(ram.NextValue())) / 100;

            int cpu1Val = (pictureBox3.Height * (int)Math.Round(cpu1.NextValue())) / 100;
            int cpu2Val = (pictureBox4.Height * (int)Math.Round(cpu2.NextValue())) / 100;
            int cpu3Val = (pictureBox5.Height * (int)Math.Round(cpu3.NextValue())) / 100;
            int cpu4Val = (pictureBox6.Height * (int)Math.Round(cpu4.NextValue())) / 100;

            _cpuPt.Add(new Point(X, pictureBox1.Height - cpuVal));                         //megcsináltok a graf linejait
            _ramPt.Add(new Point(X, pictureBox2.Height - RamVal));

            _cpu1Pt.Add(new Point(Y, pictureBox3.Height - cpu1Val));
            _cpu2Pt.Add(new Point(Y, pictureBox4.Height - cpu2Val));
            _cpu3Pt.Add(new Point(Y, pictureBox5.Height - cpu3Val));
            _cpu4Pt.Add(new Point(Y, pictureBox6.Height - cpu4Val));

            //public int rampwm { get; set { rampwm = value; }; }





            label3.Text = cpuVal.ToString()+" % ";

            label9.Text =   cpu1Val.ToString() + "%";
            label10.Text =  cpu2Val.ToString() + "%";
            label11.Text =  cpu3Val.ToString() + "%";
            label12.Text =  cpu4Val.ToString() + "%";
            label4.Text =   RamVal.ToString() + "%";

            String temp = "";

            foreach (var hardwareItem in _thisComputer.Hardware)
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

                            temp += string.Format("{0} Temperature = {1}°C\r\n", sensor.Name, sensor.Value.HasValue ? sensor.Value.Value.ToString() : "no value");

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

            if (Y > pictureBox3.Width || Y > pictureBox4.Width || Y > pictureBox5.Width || Y > pictureBox6.Width)         //ha eléri a végét újra rajzoljuk
            {
                Y = 0;
                _cpu1Pt.Clear();
                _cpu2Pt.Clear();
                _cpu3Pt.Clear();
                _cpu4Pt.Clear();
                //
            }

            g.FillRectangle(new HatchBrush(HatchStyle.Cross, Color.Gray), pictureBox3.ClientRectangle);
            if (_cpu1Pt.Count > 1)
            {
                g.DrawLines(new Pen(new SolidBrush(Color.FromArgb(255, 0, 255, 100))), _cpu1Pt.ToArray());
            }
        }

        private void pictureBox4_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;                                //eseményre

            g.FillRectangle(new HatchBrush(HatchStyle.Cross, Color.Gray), pictureBox4.ClientRectangle);
            if (_cpu2Pt.Count > 1)
            {
                g.DrawLines(new Pen(new SolidBrush(Color.FromArgb(255, 0, 255, 100))), _cpu2Pt.ToArray());
            }

        }

        private void pictureBox5_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;                                //eseményre

            g.FillRectangle(new HatchBrush(HatchStyle.Cross, Color.Gray), pictureBox5.ClientRectangle);
            if (_cpu3Pt.Count > 1)
            {
                g.DrawLines(new Pen(new SolidBrush(Color.FromArgb(255, 0, 255, 100))), _cpu3Pt.ToArray());
            }

        }

        private void pictureBox6_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;                                //eseményre

            g.FillRectangle(new HatchBrush(HatchStyle.Cross, Color.Gray), pictureBox6.ClientRectangle);
            if (_cpu4Pt.Count > 1)
            {
                g.DrawLines(new Pen(new SolidBrush(Color.FromArgb(255, 0, 255, 100))), _cpu4Pt.ToArray());
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
            serialPort1.WriteLine("pwmr "+ RamVal.ToString()+".0");
            serialPort1.WriteLine("pwmg " + RamVal.ToString() + ".0");
            serialPort1.WriteLine("pwmb " + RamVal.ToString() + ".0");

            label13.Text = RamVal.ToString() + " %";
            Plusten = RamVal;
            //Minusten = RamVal;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Test = RamVal+30;

            //for (int i = 0; i < testvalues.Count; i++)
            //{
            //    serialPort1.WriteLine("pwmr " + testvalues + ".0");
            //}

            if (20 <= Test && Test <= 100)
            {
                serialPort1.WriteLine("pwmr " + Test.ToString() + ".0");
                serialPort1.WriteLine("pwmg " + Test.ToString() + ".0");
                serialPort1.WriteLine("pwmb " + Test.ToString() + ".0");
            }
            else
            {
                Test = 20;
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
                    label13.Text = number.ToString() + " %";

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
            Plusten = Plusten+10;
            if (15<=Plusten&&Plusten<=100)
            {
                serialPort1.WriteLine("pwmr " + Plusten.ToString() + ".0");
                serialPort1.WriteLine("pwmg " + Plusten.ToString() + ".0");
                serialPort1.WriteLine("pwmb " + Plusten.ToString() + ".0");
            }
            else
            {
                Plusten = 100;
                serialPort1.WriteLine("pwmr " + "100" + ".0");
                serialPort1.WriteLine("pwmg " + "100" + ".0");
                serialPort1.WriteLine("pwmb " + "100" + ".0");
            }

            label13.Text = Plusten.ToString()+"%";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Plusten = Plusten - 10;
            if (15 <= Plusten && Plusten <= 100)
            {
                serialPort1.WriteLine("pwmr " + Plusten.ToString() + ".0");
                serialPort1.WriteLine("pwmg " + Plusten.ToString() + ".0");
                serialPort1.WriteLine("pwmb " + Plusten.ToString() + ".0");
            }
            else
            {
                Plusten = 15;
                serialPort1.WriteLine("pwmr " + "15" + ".0");
                serialPort1.WriteLine("pwmg " + "15" + ".0");
                serialPort1.WriteLine("pwmb " + "15" + ".0");
            }

            label13.Text = Plusten.ToString() + " %";
        }
    }
}
