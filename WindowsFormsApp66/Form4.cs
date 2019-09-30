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
    public partial class Form4 : Form
    {
        double height = 100;
        Point downPoint = new Point();
        List<int> ims = new List<int>();
        bool move;
        public static bool act;
        public static bool Action;               
        public static Form1.Image2 SelectedImage ;
        public void VisualizeImage(Graphics grph)
        {
            grph.Clear(Color.White);
            int maxwidth = 0;
            if (ims.Count < Form1.imagess.Count)
            {
                for (int ind = 0; ind < Form1.imagess.Count - ims.Count; ind++)
                {
                    ims.Add(ims[ims.Count - 1] + (int)height + 20);
                }
            }
            foreach (var image in Form1.imagess)
            {
                double proportionNumber = (double)image.image.Height / height;
                var width = (double)image.image.Width / proportionNumber;
                if (width > maxwidth)
                {
                    maxwidth = (int)width;
                }
                grph.DrawImage(image.image, new Rectangle(0, ims[Form1.imagess.IndexOf(image)], (int)width, (int)height));
            }
            var plus = panel2.Left - panel1.Left;
            pictureBox1.Width = maxwidth;
            panel2.Left = pictureBox1.Width+plus;
            label1.Left = pictureBox1.Width;
            panel1.Left = pictureBox1.Width;
            pictureBox2.Left = panel1.Left + panel1.Width;
        }
        public Form4()
        {
            InitializeComponent();
            for (int ind = 0; ind < Form1.imagess.Count; ind++)
            {
                ims.Add(ind * (int)height + 20);
            }
            pictureBox1.MouseWheel += panel1_MouseWheel;
            panel1.Controls.Remove(panel2);
            this.Controls.Add(panel2);
            panel2.BringToFront();
            if(Form1.imagess.Count >= 1)
            {
                SelectedImage = Form1.imagess[0];
            }            
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            VisualizeImage(e.Graphics);
            button1.Left = pictureBox1.Width + pictureBox1.Left + 2;
            button2.Left = button1.Left + button1.Width + 6;            
        }
        private void panel1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            for (int ind = 0; ind < ims.Count; ind++)
            {
                ims[ind] += e.Delta - ((panel2.Left - panel1.Left)/4);
            }
            pictureBox1.Refresh();
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (height < 250)
            {
                height += 25;
                for (int ind = 0; ind < ims.Count; ind++)
                {
                    ims[ind] += 25 * ind;
                }
                pictureBox1.Refresh();
            }
            button1.Left = pictureBox1.Width + pictureBox1.Left + 2;
            button2.Left = button1.Left + button1.Width + 6;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (height > 10)
            {
                height -= 25;
                for (int ind = 0; ind < ims.Count; ind++)
                {
                    ims[ind] -= 25 * ind;
                }
                pictureBox1.Refresh();
            }
            button1.Left = pictureBox1.Width + pictureBox1.Left + 2;
            button2.Left = button1.Left + button1.Width + 6;
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (panel2.Left > panel1.Left+1|e.X>downPoint.X && panel2.Left + panel2.Width < panel1.Left + panel1.Width-1 | e.X < downPoint.X&&e.Button == MouseButtons.Left)
            {
                panel2.Left += e.X - downPoint.X;
            }
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            downPoint = e.Location;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(Action!=act)
            {
                Action = false;
                pictureBox1.Refresh();
                pictureBox2.Refresh();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var us =new UserControl3();
            us.Parent = this;
            us.BringToFront();
            us.Left = 400;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            if (SelectedImage != null)
            {
                e.Graphics.DrawImage(SelectedImage.image, pictureBox2.Width / 2 - SelectedImage.image.Width / 2, pictureBox2.Height / 2 - SelectedImage.image.Height / 2);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            foreach(var image in ims)
            {
                if (e.Y > image && e.Y < image + Form1.imagess[ims.IndexOf(image)].image.Height)
                {
                    SelectedImage = Form1.imagess[ims.IndexOf(image)];
                    pictureBox2.Refresh();
                }
            }
        }
    }
}
