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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();            
        }
        bool b;
        class Page
        {
            public static List<Text1> visComponents = new List<Text1>();
            public static List<int> layers = new List<int>();
            public static Bitmap imageOfPage = new Bitmap(1920, 1080);
            public Page()
            {
            }
            public void VisualizeOnPage(Text1 element, int layer)
            {
                page.Clear(Color.Transparent);
                Page.visComponents.Add(element);
                Page.layers.Add(layer);
                int index = 0;
                for (bool end = true; end;)
                {
                    var ind = Page.layers.IndexOf(index);
                    if (ind !=-1)
                    {
                        page.DrawImage(Page.visComponents[ind].image, element.drawPoint);
                        //Page.visComponents.Remove(Page.visComponents[index]);
                        //Page.layers.Remove(index);
                    }
                    if (ind == -1)
                    {
                        end = false;
                    }
                    index++;
                }

            }
            public Graphics page = Graphics.FromImage(imageOfPage);
        }
        class Text1
        {
            public Bitmap image = new Bitmap(1920, 1080);
            public System.Drawing.Point drawPoint = new System.Drawing.Point();
            public void VisMe(TextBox text, System.Drawing.Point drawPoint, Label l, Color c)
            {
                var graph = Graphics.FromImage(image);
                this.drawPoint = drawPoint;
                graph.DrawString(text.Text, l.Font,new SolidBrush(c), new System.Drawing.Point(0, 0)/*new StringFormat(StringFormatFlags.NoClip)*/);
            }          
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(b)
            {
                var text2 = new Text1();
                var text = new Text1();
                var page = new Page();
                text.VisMe(textBox1, new System.Drawing.Point(20, 20), label1, Color.Blue);
                text2.VisMe(textBox2, new System.Drawing.Point(20, 30), label1, Color.Orange);
                page.VisualizeOnPage(text2, 0);
                page.VisualizeOnPage(text, 1);
                pictureBox1.Image = Page.imageOfPage;
                Page.layers.Clear();
                Page.visComponents.Clear();
                b = false;
            }
            else
            {
                var text2 = new Text1();
                var text = new Text1();
                var page = new Page();
                text.VisMe(textBox1, new System.Drawing.Point(20, 20), label1, Color.Blue);
                text2.VisMe(textBox2, new System.Drawing.Point(20, 30), label1, Color.Orange);
                page.VisualizeOnPage(text2, 1);
                page.VisualizeOnPage(text, 0);
                pictureBox1.Image = Page.imageOfPage;
                Page.layers.Clear();
                Page.visComponents.Clear();
                b = true;
            }
        }
    }
}
