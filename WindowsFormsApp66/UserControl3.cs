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
    public partial class UserControl3 : UserControl
    {
        Color brushC;
        public UserControl3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            brushC = Form1.brushColl;
            var F = new Form2(false);
            Form1.BrushColor = true;
            F.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var coll = Form1.brushColl;
            var value2 = hScrollBar1.Value / 2;
            if (Form4.SelectedImage != null)
            {
                for (int pixel = 0; pixel < Form4.SelectedImage.image.Width - 2; pixel++)
                {
                    for (int pixel2 = 0; pixel2 < Form4.SelectedImage.image.Height - 2; pixel2++)
                    {
                        var getPix = Form4.SelectedImage.image.GetPixel(pixel, pixel2);
                        if (getPix.R > coll.R - value2 && getPix.R < coll.R + value2)
                        {
                            if (getPix.G > coll.G - value2 && getPix.G < coll.G + value2)
                            {
                                if (getPix.B > coll.B - value2 && getPix.B < coll.B + value2)
                                {
                                    Form4.SelectedImage.image.SetPixel(pixel, pixel2, Color.Transparent);
                                }
                            }
                        }
                    }
                }
            }
            Form4.Action = true;
            Form1.brushColl = brushC;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
