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
        bool pickColor;
        bool select;
        Point cursorPoint = new Point();
        public static bool act;
        public static bool Action;
        public static Form1.Image2 SelectedImage;
        Graphics gr;
        Rectangle selectedRect = new Rectangle();
        Bitmap im;
        Point e = new Point();
        enum tools
        {
            selection, pipetka, brush
        }
        tools tool = tools.selection;
        enum direction
        {
            up, left
        }
        int thickness = 10;
        void DrawPunctir(int width, direction direction, Point startPoint, Graphics drawGr)
        {
            var count = width / (thickness);
            for (int i = 0; i < count; i += 2)
            {
                if (direction == direction.up)
                {
                    drawGr.DrawLine(new Pen(new SolidBrush(Color.Blue), 2), new Point(startPoint.X, startPoint.Y + thickness * i), new Point(startPoint.X, startPoint.Y + i * thickness + 5));
                }


                if (direction == direction.left)
                {
                    drawGr.DrawLine(new Pen(new SolidBrush(Color.Blue), 2), new Point(startPoint.X + thickness * i, startPoint.Y), new Point(startPoint.X + i * thickness + 5, startPoint.Y));
                }
            }
        }
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
            panel2.Left = pictureBox1.Width + plus;
            label1.Left = pictureBox1.Width;
            panel1.Left = pictureBox1.Width;            
            button3.Left = pictureBox1.Width;
            pictureBox2.Left = button3.Left + button3.Width;
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
            if (Form1.imagess.Count >= 1)
            {
                SelectedImage = Form1.imagess[0];
            }
            im = new Bitmap(1920, 1080);
            gr = Graphics.FromImage(im);
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
                ims[ind] += e.Delta - ((panel2.Left - panel1.Left) / 4);
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
            if (panel2.Left > panel1.Left + 1 | e.X > downPoint.X && panel2.Left + panel2.Width < panel1.Left + panel1.Width - 1 | e.X < downPoint.X && e.Button == MouseButtons.Left)
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
            if (Action != act)
            {
                Action = false;
                pictureBox1.Refresh();
                pictureBox2.Refresh();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var us = new UserControl3();
            us.Parent = this;
            us.BringToFront();
            us.Left = 0;
            pictureBox2.Controls.Add(us);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (tool == tools.pipetka)
            {
                Form1.brushColl = im.GetPixel(cursorPoint.X, cursorPoint.Y);
            }
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs ee)
        {
            if (SelectedImage != null)
            {
                gr.DrawImage(SelectedImage.image, pictureBox2.Width / 2 - SelectedImage.image.Width / 2, pictureBox2.Height / 2 - SelectedImage.image.Height / 2);
                ee.Graphics.DrawImage(SelectedImage.image, pictureBox2.Width / 2 - SelectedImage.image.Width / 2, pictureBox2.Height / 2 - SelectedImage.image.Height / 2);
            }
            if (pickColor)
            {
                var pixel = im.GetPixel(cursorPoint.X, cursorPoint.Y);
                ee.Graphics.FillRectangle(new SolidBrush(im.GetPixel(cursorPoint.X, cursorPoint.Y)), new Rectangle(cursorPoint.X, cursorPoint.Y - 11, 10, 10));
                ee.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.FromArgb(255, 255 - pixel.R, 255 - pixel.G, 255 - pixel.B))), new Rectangle(cursorPoint.X, cursorPoint.Y - 12, 11, 11));
            }
            if (tool == tools.selection && select)
            {
                var start = new Point();
                var end = new Point();                
                if (e.X < downPoint.X)
                {
                    start.X = e.X;
                }
                else
                {
                    start.X = downPoint.X;
                }
                if (e.X < downPoint.X)
                {
                    end.X = downPoint.X;
                }
                else
                {
                    end.X = e.X;
                }
                if (e.Y < downPoint.Y)
                {
                    start.Y = e.Y;
                }
                else
                {
                    start.Y = downPoint.Y;
                }
                if (e.Y < downPoint.Y)
                {
                    end.Y = downPoint.Y;
                }
                else
                {
                    end.Y = e.Y;
                }
                DrawPunctir(end.X - start.X, direction.left, start, ee.Graphics);
                DrawPunctir(end.X - start.X, direction.left, new Point(start.X, end.Y), ee.Graphics);
                DrawPunctir(end.Y - start.Y, direction.up, start, ee.Graphics);
                DrawPunctir(end.Y - start.Y, direction.up, new Point(end.X, start.Y), ee.Graphics);
                selectedRect.Width = end.X - start.X;
                selectedRect.Height = end.Y - start.Y;
                button6.Left = downPoint.X + pictureBox2.Left;
                button6.Top = downPoint.Y - button6.Height-4;
                button6.Visible = true;
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (var image in ims)
            {
                if (e.Y > image && e.Y < image + Form1.imagess[ims.IndexOf(image)].image.Height)
                {
                    SelectedImage = Form1.imagess[ims.IndexOf(image)];
                    pictureBox2.Refresh();
                }
            }
        }

        private void pictureBox2_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (tool == tools.pipetka)
            {
                pickColor = true;
                cursorPoint = e.Location;
                pictureBox2.Refresh();
            }
            if (tool == tools.selection && e.Button == MouseButtons.Left)
            {
                select = true;
                pictureBox2.Refresh();
                this.e = e.Location;
            }
        }

        private void pictureBox2_ClientSizeChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (tool == tools.selection)
            {
                downPoint = e.Location;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tool = tools.pipetka;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tool = tools.selection;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var bmp = new Bitmap(selectedRect.Width, selectedRect.Height);
            var grph = Graphics.FromImage(bmp);
            var rect = new Rectangle(downPoint.X, downPoint.Y, selectedRect.Width, selectedRect.Height);
            grph.DrawImage(im, new Rectangle(0,0, selectedRect.Width, selectedRect.Height), rect, GraphicsUnit.Pixel);
            pictureBox3.Image = bmp;
        }
    }
}
