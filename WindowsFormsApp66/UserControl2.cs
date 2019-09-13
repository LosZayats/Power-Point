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
    public partial class UserControl2 : UserControl
    {
        public UserControl2()
        {
            InitializeComponent();
            button1.BackgroundImage = Gradient.MakeGradient(Color.Green, Direction.right, button1);
            button2.BackgroundImage = Gradient.MakeGradient(Color.Red, Direction.left, button2);
            button3.BackgroundImage = Gradient.MakeGradient(Color.Indigo, Direction.right, button3);
        }
        public enum Direction
        {
            up, down, left, right
        }
        public static class Gradient
        {
            public static Bitmap MakeGradient(Color color, Direction direction, Control control)
            {
                var bitm = new Bitmap(control.Width, control.Height);
                int transparency = 255;
                if (direction == Direction.up)
                {
                    for (int top = control.Height-1; top > 0; top--)
                    {
                        if (transparency > 0)
                        {
                            transparency -= 255 / control.Height;
                        }
                        for (int left = 0; left < control.Width; left++)
                        {
                            bitm.SetPixel(left, top, Color.FromArgb(transparency, color.R, color.G, color.B));
                        }
                    }

                }
                if (direction == Direction.down)
                {
                    for (int top = 0; top < control.Height; top++)
                    {
                        if (transparency > 0)
                        {
                            transparency -= 255 / control.Height;
                        }
                        for (int left = 0; left < control.Width; left++)
                        {
                            bitm.SetPixel(left, top, Color.FromArgb(transparency, color.R, color.G, color.B));
                        }
                    }

                }
                if (direction == Direction.up)
                {
                    transparency = 0;
                    for (int top = control.Height - 1; top > 0; top--)
                    {                        
                        if (transparency <255+ 255 / control.Height)
                        {
                            transparency += 255 / control.Height;
                        }
                        for (int left = 0; left < control.Width; left++)
                        {
                            bitm.SetPixel(left, top, Color.FromArgb(transparency, color.R, color.G, color.B));
                        }
                    }

                }
                if (direction == Direction.left)
                {                    
                    for (int left = 0; left < control.Width; left++)
                    {
                        if (transparency > 255 / control.Width + 1)
                        {
                            transparency -= 255 / control.Width;
                        }
                        for (int top = 0; top < control.Height; top++)
                        {                            
                            bitm.SetPixel(left, top, Color.FromArgb(transparency, color.R, color.G, color.B));
                        }
                    }


                }
                if (direction == Direction.right)
                {
                    transparency = 0;
                    for (int left = 0; left < control.Width; left++)
                    {
                        if (transparency < 255)
                        {
                            transparency += 255 / control.Width;
                        }
                        for (int top = 0; top < control.Height; top++)
                        {
                            bitm.SetPixel(left, top, Color.FromArgb(transparency, color.R, color.G, color.B));
                        }
                    }


                }

                return bitm;
            }
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
        public void TextVis()
        {
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var ind = Form1.richTextBoxes.IndexOf(Form1.workTextBox);         
            if(Form1.texts.Count>=ind+1)
            {
                Form1.texts.Remove(Form1.texts[ind]);                
            }            
            Form1.layers.Remove(Form1.layers[ind]);
            Form1.workTextBox.Dispose();
            Form1.richTextBoxes.Remove(Form1.workTextBox);            
            this.Visible = false;
            Form1.counter--;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.Show();
            this.Visible = false;
        }
    }
}
