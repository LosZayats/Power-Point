using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp66
{
    public partial class Form3 : Form
    {
        Point clickPoint;
        bool ableToMove;
        Color BK;
        public Form3()
        {
            InitializeComponent();
            var bmp = UserControl2.Gradient.MakeGradient(Color.BlueViolet, UserControl2.Direction.down, this);
            this.BackgroundImage = bmp;
        }

        private void Form3_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = new Cursor(Properties.Resources.cursor.Handle);
        }

        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            if(ableToMove)
            {
                if(panel3.Left + e.X - clickPoint.X>panel1.Left&& panel3.Left + e.X - clickPoint.X <panel1.Width)
                {
                    double plus = panel1.Width / 100;
                    Form1.Thikness = (int)((double)(panel3.Left - panel1.Left)/plus);
                    if(((double)(panel3.Left - panel1.Left) / plus) == Form1.Thikness)
                    {
                        label1.Text = ((double)(panel3.Left - panel1.Left) / plus).ToString()+",0";
                    }
                    else
                    {
                        label1.Text = ((double)(panel3.Left - panel1.Left) / plus).ToString();
                    }
                    panel3.Left += e.X - clickPoint.X;
                    label1.BackColor = Color.Transparent;
                }                
            }            
        }

        private void panel3_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void panel3_MouseUp(object sender, MouseEventArgs e)
        {
            ableToMove = false;
        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            ableToMove = true;
            clickPoint = e.Location;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1.brushColl = Color.White;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(button2.Text == "Сгладить")
            {
                Form1.Circle = true;
                button2.Text = "Не сглаживать";
            }
            else
            {
                Form1.Circle = false;
                button2.Text = "Сгладить";
            }            
        }

        private void panel3_MouseEnter(object sender, EventArgs e)
        {
            BK = panel3.BackColor;
            panel3.BackColor = Color.FromArgb(panel3.BackColor.A, panel3.BackColor.R, panel3.BackColor.G, 255);
        }

        private void panel3_MouseLeave(object sender, EventArgs e)
        {
            panel3.BackColor = BK;
        }

        private void Form3_MouseDown(object sender, MouseEventArgs e)
        {

        }
    }
}
