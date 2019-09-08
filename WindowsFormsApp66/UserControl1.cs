using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp66
{
    public partial class UserControl1 : UserControl
    {
        Bitmap bmp2;
        public UserControl1()
        {
            InitializeComponent();
            //hScrollBar1.Value++;
            var bmp = new Bitmap(255, 255);
            for (int top = 0; top < 255; top++)
            {
                for (int left = 0; left < 255; left++)
                {
                    bmp.SetPixel(left, top, Color.FromArgb(top, left, (int)((double)hScrollBar1.Value * 2.5)));
                }
            }
            this.BackgroundImage = bmp;
            bmp2 = bmp;
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {

        }

        private void UserControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if(bmp2 !=null)
            {
                panel1.BackColor = bmp2.GetPixel(e.X, e.Y);
            }            
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            var bmp = new Bitmap(255, 255);
            for (int top = 0; top < 255; top++)
            {
                for (int left = 0; left < 255; left++)
                {
                    bmp.SetPixel(left, top, Color.FromArgb(top, left, (int)((double)hScrollBar1.Value * 2.5)));
                }
            }
            this.BackgroundImage = bmp;
            bmp2 = bmp;
        }

        private void UserControl1_Click(object sender, EventArgs e)
        {
        }

        private void UserControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (bmp2 != null)
            {
                Form1.allColor = bmp2.GetPixel(e.X, e.Y);
            }
        }
    }
}
