using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace WindowsFormsApp66
{
    public partial class Form1 : Form
    {
        const string SettingsFileName = "презентация.pttx";
        [Serializable] //Это атрибут, который позволяет автоматически сериализовать и десериализовать 
        public struct ProgrammSettings
        {
            public List<Page2> pages;
        }
        public static void SaveSettings(ProgrammSettings settings)
        {
            using (Stream stream = File.Open(SettingsFileName, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, settings);
            }
        }
        public Form1()
        {
            InitializeComponent();
            font = label1.Font;
            panel1.BackgroundImage = UserControl2.Gradient.MakeGradient(Color.OrangeRed, UserControl2.Direction.down, panel1);
            panel2.BackgroundImage = UserControl2.Gradient.MakeGradient(Color.OrangeRed, UserControl2.Direction.down, panel1);
            panel3.BackgroundImage = UserControl2.Gradient.MakeGradient(Color.OrangeRed, UserControl2.Direction.down, panel1);
            panel4.BackgroundImage = UserControl2.Gradient.MakeGradient(Color.OrangeRed, UserControl2.Direction.down, panel1);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(button2);
            panel5.BackgroundImage = UserControl2.Gradient.MakeGradient(Color.OrangeRed, UserControl2.Direction.up, panel1);
            panel6.BackgroundImage = UserControl2.Gradient.MakeGradient(Color.OrangeRed, UserControl2.Direction.up, panel1);
            panel7.BackgroundImage = UserControl2.Gradient.MakeGradient(Color.OrangeRed, UserControl2.Direction.up, panel1);
            panel8.BackgroundImage = UserControl2.Gradient.MakeGradient(Color.OrangeRed, UserControl2.Direction.up, panel1);
            panel7.Controls.Add(button3);
            panel7.Controls.Add(button4);
            panel7.Controls.Add(label2);
            label2.BringToFront();
            var load = new ProgrammSettings();
            if (load.pages == null)
            {
                load.pages = new List<Page2>();
            }
            load.pages.Add(new Page2());
            SaveSettings(load);

        }
        bool b;
        public static RichTextBox workTextBox;
        Point DownPoint;
        bool ableToMove;
        public static int counter;
        public static Color allColor = Color.Black;
        public static Font font;
        int pageNum = 0;
        public static List<Text1> texts = new List<Text1>();
        public static List<RichTextBox> richTextBoxes = new List<RichTextBox>();
        public static List<int> layers = new List<int>();
        public static Page PageNow = new Page();
        public static ProgrammSettings LoadSettings()
        {
            if (File.Exists(SettingsFileName))
            {
                using (Stream stream = File.Open(SettingsFileName, FileMode.Open))
                {
                    var formatter = new BinaryFormatter();
                    return (ProgrammSettings)formatter.Deserialize(stream);
                }
            }
            return default(ProgrammSettings);
        }
        [Serializable] //Это атрибут, который позволяет автоматически сериализовать и десериализовать 
        public class Page
        {
            public List<Text1> visComponents = new List<Text1>();
            public List<int> layers = new List<int>();
            public Bitmap imageOfPage = new Bitmap(1920, 1080);
            public Page()
            {
                page = Graphics.FromImage(imageOfPage);
            }
            public void Clear(Text1 element)
            {
                var ind = visComponents.IndexOf(element);
                visComponents.Remove(element);
                layers.Add(layers[ind]);
            }
            public void VisualizeOnPage(Text1 element, int layer)
            {
                page.Clear(Color.Transparent);
                visComponents.Add(element);
                layers.Add(layer);
                int index = 0;
                for (bool end = true; end;)
                {
                    var ind = layers.IndexOf(index);
                    if (ind != -1)
                    {
                        page.DrawImage(visComponents[ind].image, element.drawPoint);
                    }
                    if (ind == -1)
                    {
                        end = false;
                    }
                    index++;
                }

            }
            public Graphics page;
        }
        [Serializable]
        public class Page2
        {
            public List<Text1> texts2 = new List<Text1>();
            public List<int> layers2 = new List<int>();
            public int counter;
        }
        [Serializable]
        public class Text1
        {
            public Bitmap image;
            public System.Drawing.Point drawPoint = new System.Drawing.Point();
            public void VisMe(RichTextBox text, System.Drawing.Point drawPoint, Label l, Color c)
            {
                image = new Bitmap(1920, 1080);
                var graph = Graphics.FromImage(image);
                this.drawPoint = drawPoint;
                graph.DrawString(text.Text, font, new SolidBrush(c), new System.Drawing.Point(0, 0)/*new StringFormat(StringFormatFlags.NoClip)*/);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var text2 = new Text1();
            var text = new Text1();
            var page = new Page();
            page.VisualizeOnPage(text2, 0);
            page.VisualizeOnPage(text, 1);
            pictureBox1.Image = PageNow.imageOfPage;

        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var textBox = new RichTextBox();
            textBox.Left = 100;
            textBox.Top = 200;
            textBox.Height = 20;
            textBox.Parent = this;
            texts.Add(new Text1());
            this.Controls.Add(textBox);
            textBox.BorderStyle = BorderStyle.None;
            textBox.Cursor = Cursors.Hand;
            textBox.BringToFront();
            workTextBox = textBox;
            textBox.MouseDown += textBox2_MouseDown;
            textBox.TextChanged += textBox2_TextChanged;
            textBox.MouseMove += textBox2_MouseMove;
            textBox.MouseUp += textBox2_MouseUp;
            layers.Add(counter);
            richTextBoxes.Add(textBox);
            counter++;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Text1 text1 = null;
            text1 = texts[texts.Count - 1];
            var send = (RichTextBox)sender;
            text1.VisMe(send, new Point(send.Left, send.Top), label1, allColor);
        }

        private void textBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DownPoint.Y = e.Y;
                DownPoint.X = e.X;
                ableToMove = true;
                workTextBox = (RichTextBox)sender;
                workTextBox.BorderStyle = BorderStyle.None;
            }
            else if (e.Button == MouseButtons.Right)
            {
                userControl21.Visible = true;
                workTextBox = (RichTextBox)sender;
                userControl21.Left = workTextBox.Left;
                userControl21.Top = workTextBox.Top - userControl21.Height;
            }

        }

        private void textBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (ableToMove)
            {
                var send = (RichTextBox)sender;
                if (send.Top <= 100)
                {
                    send.Top = 100;
                }
                else if (send.Top + send.Height >= this.Height - 40)
                {
                    send.Top = this.Height - 60;
                }
                else
                {
                    send.Top += e.Y - DownPoint.Y;
                }
                if (send.Left + send.Width >= this.Width - 20)
                {
                    send.Left = this.Width - 14 - send.Width;
                }
                else if (send.Left <= 0)
                {
                    send.Left = 0;
                }
                else
                {
                    send.Left += e.X - DownPoint.X;
                }
            }
        }

        private void textBox2_MouseUp(object sender, MouseEventArgs e)
        {
            ableToMove = false;
            workTextBox.BackColor = Color.WhiteSmoke;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (workTextBox != null)
            {
                workTextBox.BackColor = Color.LightCyan;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (var text in texts)
            {
                text.drawPoint.X = richTextBoxes[texts.IndexOf(text)].Left;
                text.drawPoint.Y = richTextBoxes[texts.IndexOf(text)].Top;
                PageNow.VisualizeOnPage(text, layers[texts.IndexOf(text)]);
            }
            pictureBox1.Image = PageNow.imageOfPage;
            PageNow.layers.Clear();
            PageNow.visComponents.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var loadSettings = LoadSettings();
            pictureBox1.Image = null;
            var page = loadSettings.pages[pageNum];
            layers = page.layers2;
            texts = page.texts2;
            counter = page.counter;
            pageNum--;
            foreach (var text in texts)
            {
                PageNow.VisualizeOnPage(text, 0);
                pictureBox1.Image = PageNow.imageOfPage;
            }
            label2.Text = pageNum.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var loadSettings = LoadSettings();
            var page2 = new Page2();
            pictureBox1.Image = null;
            if (loadSettings.pages.Count == pageNum + 1)
            {
                page2.layers2 = layers;
                page2.counter = counter;
                page2.texts2 = texts;
                loadSettings.pages[pageNum] = page2;
                var page3 = new Page2();
                loadSettings.pages.Add(page3);
                PageNow = new Page();
                pageNum++;
            }
            else if (loadSettings.pages.Count > pageNum + 1)
            {
                var page = loadSettings.pages[pageNum];
                layers = page.layers2;
                texts = page.texts2;
                counter = page.counter;
                foreach (var text in texts)
                {
                    PageNow.VisualizeOnPage(text, 0);
                    pictureBox1.Image = PageNow.imageOfPage;
                }
                pageNum++;
            }
            label2.Text = pageNum.ToString();
            SaveSettings(loadSettings);
        }
    }
}
