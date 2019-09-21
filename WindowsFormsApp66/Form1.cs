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
using System.Windows.Input;
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
        class Shape:Attribute
        {
            public enum Shapes
            {
                Circle,Rectangle,
            }
        }
        double Frame;
        public static void SaveSettings(ProgrammSettings settings)
        {
            using (Stream stream = File.Open(SettingsFileName, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, settings);
            }
        }
        bool anim;
        
        public Form1()
        {
            InitializeComponent();
            var load = LoadSettings();
            if (load.pages == null)
            {
                load.pages = new List<Page2>();
            }
            load.pages.Add(new Page2());
            SaveSettings(load);
            Func2(true);
            Func();
            var loadSettings = LoadSettings();
            var page2 = new Page2();
            if (loadSettings.pages.Count > pageNum)
            {
                PageNow = new Page();
                var page = loadSettings.pages[pageNum];
                layers = page.layers2;
                texts = page.texts2;
                counter = page.counter;
                imagess = page.images;
                PageNow.page.Clear(Color.Transparent);
                pictureBox1.Image = PageNow.imageOfPage;
                PageNow.layers.Clear();
                PageNow.visComponents.Clear();
                if (loadSettings.pages[pageNum].BackGround != null)
                {
                    pictureBox1.BackgroundImage = LoadSettings().pages[pageNum].BackGround;
                }
                foreach (var text in texts)
                {
                    var textBox = new RichTextBox()
                    {
                        Text = text.text2
                    };
                    textBox.Location = text.drawPoint;
                    richTextBoxes.Add(textBox);
                    textBox.Parent = this;
                    textBox.BringToFront();
                    workTextBox = textBox;
                    textBox.MouseDown += textBox2_MouseDown;
                    textBox.TextChanged += textBox2_TextChanged;
                    textBox.MouseMove += textBox2_MouseMove;
                    textBox.MouseUp += textBox2_MouseUp;
                    textBox.Height = 20;
                    textBox.BorderStyle = BorderStyle.None;
                    var ind = texts.IndexOf(text);
                    richTextBoxes[ind].Font = text.myFont;
                    richTextBoxes[ind].Size = new Size(new Point((int)PageNow.page.MeasureString(workTextBox.Text, text.myFont).Width + 20, (int)PageNow.page.MeasureString(workTextBox.Text, text.myFont).Height));
                }
                foreach (var image in imagess)
                {
                    PageNow.page.DrawImage(image.image, image.rect);
                }
            }
            label2.Text = pageNum.ToString();
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
            panel8.Controls.Add(button3);
            panel3.Controls.Add(button7);
            panel6.Controls.Add(button4);
            panel7.Controls.Add(label2);
            label2.BringToFront();
            header = UserControl2.Gradient.MakeGradient(Color.Orange, UserControl2.Direction.down, this);
            panels.Add(panel1);
            panels.Add(panel2);
            panels.Add(panel3);
            panels.Add(panel4);
            panels2.Add(panel5);
            panels2.Add(panel6);
            panels2.Add(panel7);
            panels2.Add(panel8);
            panel2.Controls.Add(label3);
            font = label1.Font;
            allColor = Color.Black;            
        }
        bool b;
        Bitmap header;
        bool loading = false;
        bool redact;
        bool draw;
        public static RichTextBox workTextBox;
        Point DownPoint;
        public static Color brushColl;
        public static bool BrushColor;
        bool ableToMove;
        public static int counter;
        static Color allCol;
        public void Loading(Graphics e)
        {
            if (loading)
            {
                e.Clear(Color.White);
                pictureBox1.Image = PageNow.imageOfPage;
                e.DrawImage((Image)Properties.Resources.ResourceManager.GetObject("_" + ((int)Frame).ToString()), this.Width / 2 - 150, 100);
                Frame += 0.2;
                if (Frame >= 47)
                {
                    Frame = 0;
                }
            }
        }
        public async void Load2(Graphics e)
        {
            await Task.Run(() => Loading(e));
        }
        public static Color allColor
        {
            get
            {
                return allCol;
            }
            set
            {
                allCol = value;
                if (workTextBox != null)
                {
                    var ind = richTextBoxes.IndexOf(workTextBox);
                    texts[ind].myColor = value;
                }
            }
        }
        public static Font font;
        int pagenum;
        public int pageNum
        {
            get
            {
                return pagenum;
            }
            set
            {
                Func();
                pagenum = value;
                PageNow.page.Clear(Color.Transparent);
                RichTextBoxVisualize();
                ImageVisualize();
                if (pageNum < LoadSettings().pages.Count)
                {
                    if (LoadSettings().pages[pageNum].BackGround != null)
                    {
                        pictureBox1.BackgroundImage = LoadSettings().pages[pageNum].BackGround;
                    }
                    else
                    {
                        pictureBox1.BackgroundImage = null;
                    }
                }
                pictureBox1.Image = PageNow.imageOfPage;
            }
        }
        public void DrawLine(Point start, Point end, double ThickNess)
        {
            double count = 0;
            var y1 = (double)start.Y;
            var y2 = (double)end.Y;
            var hei = (y2 - y1) / ThickNess;
            var wid = ((double)end.X - (double)start.X) / ThickNess;
            if (Math.Abs(y2 - y1) > Math.Abs(end.X-start.X))
            {
                count = hei;
            }
            else
            {
                count = wid;
            }
            count = Math.Abs(count);
            double YPlus=0;
            double XPlus = 0;
            YPlus = ((double)end.Y - (double)start.Y) / count;
            XPlus = ((double)end.X - (double)start.X) / count;            
            for (int top = 0; top/* * YPlus*/ < count/* (y2 - y1)*/; top++)
            {
                PageNow.page.FillRectangle(new SolidBrush(brushColl), new Rectangle((int)((int)top * (int)XPlus + (int)start.X), (int)((int)top * (int)YPlus)+ (int)start.Y, (int)ThickNess, (int)ThickNess));
            }
        }
        List<Panel> panels = new List<Panel>();
        List<Panel> panels2 = new List<Panel>();
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
            return
        default(ProgrammSettings);
        }
        public void RichTextBoxVisualize()
        {
            foreach (var text in texts)
            {
                var textBox = new RichTextBox()
                {
                    Text = text.text2
                };
                textBox.Location = text.drawPoint;
                richTextBoxes.Add(textBox);
                textBox.Parent = this;
                textBox.BringToFront();
                workTextBox = textBox;
                textBox.MouseDown += textBox2_MouseDown;
                textBox.TextChanged += textBox2_TextChanged;
                textBox.MouseMove += textBox2_MouseMove;
                textBox.MouseUp += textBox2_MouseUp;
                textBox.Cursor = Cursors.Hand;
                textBox.BorderStyle = BorderStyle.None;
                var ind = texts.IndexOf(text);
                richTextBoxes[ind].Font = text.myFont;
                textBox.ScrollBars = RichTextBoxScrollBars.None;
                richTextBoxes[ind].Size = new Size(new Point((int)PageNow.page.MeasureString(workTextBox.Text, text.myFont).Width + 20, (int)PageNow.page.MeasureString(workTextBox.Text, text.myFont).Height));
            }
        }
        public void DrawStroke(int strokeThickness, Rectangle rect2)
        {
            for (int left = 0; left < strokeThickness; left++)
            {
                Rectangle rect = new Rectangle(rect2.X - left, rect2.Y - left, rect2.Width + left * 2, rect2.Height + left * 2);
                PageNow.page.DrawRectangle(new Pen(Color.LightGreen), rect);
            }
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
                visComponents.Add(element);
                layers.Add(layer);
                int index = 0;
                for (bool end = true; end;)
                {
                    var ind = layers.IndexOf(index);
                    if (ind != -1)
                    {
                        page.DrawImage(visComponents[ind].image, 0, 0);
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
        public void Func()
        {
            foreach (var textbox in Controls)
            {
                var text = textbox as RichTextBox;
                if (text != null)
                {
                    text.Dispose();
                }
            }
            foreach (var textbox in Controls)
            {
                var text = textbox as RichTextBox;
                if (text != null)
                {
                    Func();
                }
            }
            richTextBoxes.Clear();
            counter = 0;
        }
        Point LastDrawPoint = new Point();
        public void Func2(bool Visible2)
        {
            foreach (var textbox in Controls)
            {
                var text = textbox as RichTextBox;
                if (text != null)
                {
                    text.Visible = Visible2;
                }
            }
        }
        public void ImageVisualize()
        {            
            foreach (var image in imagess)
            {
                PageNow.page.DrawImage(image.image, image.rect);
                DrawStroke(4, image.rect);
                pictureBox1.Image = PageNow.imageOfPage;
            }
        }
        [Serializable]
        public class Page2
        {
            public Bitmap BackGround;
            public List<Text1> texts2 = new List<Text1>();
            public List<int> layers2 = new List<int>();
            public List<Image2> images = new List<Image2>();
            public int counter;
        }
        public List<Image2> imagess = new List<Image2>();
        [Serializable]
        public class Text1
        {
            public bool Link;
            public Font myFont = Form1.font;
            public Color myColor = Color.Black;
            public Bitmap image;
            public System.Drawing.Point drawPoint = new System.Drawing.Point();
            public string text2;
            public void VisMe(RichTextBox text, System.Drawing.Point drawPoint, Color c)
            {
                image = new Bitmap(1920, 1080);
                var graph = Graphics.FromImage(image);
                this.drawPoint = drawPoint;
                if (myFont == null)
                {
                    myFont = Form1.font;
                }
                if (myColor == null)
                {
                    myColor = Color.Black;
                }
                graph.DrawString(text.Text, myFont, new SolidBrush(myColor), drawPoint);
                text2 = text.Text;
            }
        }
        [Serializable]
        public class Image2
        {
            public bool Back;
            public Rectangle rect;
            public Bitmap image;
            public int layer;
            public System.Drawing.Point drawPoint = new System.Drawing.Point();
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

        private void button1_Click(object sender, EventArgs e) { }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Func2(true);
            redact = false;
            button2.Text = "preview";
            PageNow.page.Clear(Color.Transparent);
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
            ImageVisualize();            
            richTextBoxes.Add(textBox);
            pictureBox1.Image = PageNow.imageOfPage;
            counter++;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            int ind = 0;
            foreach (var textbox in texts)
            {
                if (textbox.text2 == workTextBox.Text)
                {
                    ind = texts.IndexOf(textbox);
                }
            }
            Text1 text1 = null;
            text1 = texts[ind];
            var send = (RichTextBox)sender;
            text1.VisMe(send, new Point(send.Left, send.Top), allColor);
            var textBox = (RichTextBox)sender;
            if ((int)PageNow.page.MeasureString(textBox.Text, textBox.Font).Width == 0)
            {
                textBox.Width = 20;
                textBox.Height = 20;
            }
            else
            {
                textBox.Size = new Size(new Point((int)PageNow.page.MeasureString(textBox.Text, textBox.Font).Width + 40, (int)Form1.PageNow.page.MeasureString(textBox.Text, textBox.Font).Height));
            }
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
                else if (send.Top + send.Height >= this.Height - 60)
                {
                    send.Top = this.Height - 60;
                }
                else
                {
                    send.Top += e.Y - DownPoint.Y;
                    send.BringToFront();
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
                    send.BringToFront();
                }
                var ind = richTextBoxes.IndexOf((RichTextBox)sender);
                texts[ind].drawPoint = richTextBoxes[ind].Location;
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
            if (anim)
            {
                pictureBox1.Refresh();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            anim = true;
            loading = true;
            if (redact)
            {
                Func2(true);
                redact = false;
                button2.Text = "preview";
                pictureBox1.Image = null;
                PageNow.page.Clear(Color.Transparent);
                foreach (var image in imagess)
                {
                    PageNow.page.DrawImage(image.image, image.rect);
                    DrawStroke(4, image.rect);
                    pictureBox1.Image = PageNow.imageOfPage;
                }
            }
            else
            {
                Func2(false);
                PageNow.page.Clear(Color.Transparent);
                bool ex = false;
                foreach (var text in Controls)
                {
                    if (text as RichTextBox != null | imagess.Count != 0)
                    {
                        ex = true;
                    }
                }
                if (ex)
                {
                    var loadSettings = LoadSettings();
                    var page2 = new Page2();
                    page2.layers2 = layers;
                    page2.counter = counter;
                    page2.texts2 = texts;
                    page2.images = imagess;
                    page2.BackGround = LoadSettings().pages[pageNum].BackGround;
                    loadSettings.pages[pageNum] = page2;
                    SaveSettings(loadSettings);
                    var g = LoadSettings();
                    PageNow.page.Clear(Color.Transparent);
                    if (loadSettings.pages[pageNum].BackGround != null)
                    {
                        pictureBox1.BackgroundImage = LoadSettings().pages[pageNum].BackGround;
                        //PageNow.page.DrawImage(LoadSettings().pages[pageNum].BackGround, 0, 0);
                    }
                    foreach (var text in texts)
                    {
                        if (text.drawPoint.Y == 0) { }
                        else
                        {
                            text.drawPoint.X = richTextBoxes[texts.IndexOf(text)].Left;
                            text.drawPoint.Y = richTextBoxes[texts.IndexOf(text)].Top;
                            text.VisMe(richTextBoxes[texts.IndexOf(text)], text.drawPoint, text.myColor);
                            PageNow.VisualizeOnPage(text, layers[texts.IndexOf(text)]);
                        }
                    }
                    foreach (var image in imagess)
                    {
                        PageNow.page.DrawImage(image.image, image.rect);
                    }
                    pictureBox1.Image = PageNow.imageOfPage;
                    SaveSettings(loadSettings);
                    PageNow.layers.Clear();
                    PageNow.visComponents.Clear();
                }
                var loadSettings2 = LoadSettings();
                var page22 = new Page2();
                page22.layers2 = layers;
                page22.counter = counter;
                page22.texts2 = texts;
                page22.images = imagess;
                page22.BackGround = LoadSettings().pages[pageNum].BackGround;
                loadSettings2.pages[pageNum] = page22;
                SaveSettings(loadSettings2);
                pictureBox1.Image = PageNow.imageOfPage;
                redact = true;
                button2.Text = "redact";
            }
            loading = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            redact = true;
            var loadSettings = LoadSettings();
            pictureBox1.Image = null;
            var page = loadSettings.pages[pageNum - 1];
            layers = page.layers2;
            texts = page.texts2;
            imagess = page.images;
            pageNum--;
            counter = page.counter;
            PageNow = new Page();
            label2.Text = pageNum.ToString();
            SaveSettings(loadSettings);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            redact = true;
            var loadSettings = LoadSettings();
            var page2 = new Page2();
            pictureBox1.Image = null;
            if (loadSettings.pages.Count <= pageNum + 1)
            {
                page2.layers2 = layers;
                page2.counter = counter;
                page2.images = imagess;
                page2.texts2 = texts;
                loadSettings.pages[pageNum] = page2;
                var page3 = new Page2();
                loadSettings.pages.Add(page3);
                PageNow = new Page();
                pageNum++;
                SaveSettings(loadSettings);
                foreach (var image in imagess)
                {
                    PageNow.page.DrawImage(image.image, image.rect);
                    DrawStroke(4, image.rect);
                    pictureBox1.Image = PageNow.imageOfPage;
                }
            }
            else if (loadSettings.pages.Count > pageNum)
            {
                button2.Text = "preview";
                PageNow = new Page();
                var page = loadSettings.pages[pageNum + 1];
                layers = page.layers2;
                texts = page.texts2;
                counter = page.counter;
                imagess = page.images;
                PageNow.page.Clear(Color.Transparent);
                pictureBox1.Image = PageNow.imageOfPage;
                PageNow.layers.Clear();
                PageNow.visComponents.Clear();
                pageNum++;
                foreach (var image in imagess)
                {
                    PageNow.page.DrawImage(image.image, image.rect);
                    DrawStroke(4, image.rect);
                    pictureBox1.Image = PageNow.imageOfPage;
                }
            }
            label2.Text = pageNum.ToString();
        }

        private void pictureBox1_Click(object sender, EventArgs e) { }


        private void Form1_Paint(object sender, PaintEventArgs e) { }

        private void Form1_Resize(object sender, EventArgs e)
        {
            int width = 0;
            foreach (var panel in panels)
            {
                width += panel.Width;
            }
            int width2 = 0;
            foreach (var panel in panels2)
            {
                width2 += panel.Width;
                panel.Top = this.Height - 100;
            }
            if (width >= this.Width)
            {

            }
            else
            {
                for (; width <= this.Width;)
                {
                    var panel = new Panel();
                    panel.Left = width;
                    panel.Parent = this;
                    panel.Width = 255;
                    panel.Height = 100;
                    width += panel.Width;
                    panel.BringToFront();
                    panel.BackgroundImage = panel1.BackgroundImage;
                    panels.Add(panel);
                }
                for (; width2 <= this.Width;)
                {
                    var panel = new Panel();
                    panel.Left = width2;
                    panel.Parent = this;
                    panel.Width = 255;
                    panel.Height = 60;
                    panel.Top = this.Height - 100;
                    width2 += panel.Width;
                    panel.BringToFront();
                    panel.BackgroundImage = panel5.BackgroundImage;
                    panels2.Add(panel);
                }
            }
            pictureBox1.Image = PageNow.imageOfPage;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (anim)
            {
                if (button2.Text == "preview")
                {
                    var btn = new Button();
                    btn.Width = 200;
                    btn.Height = 50;
                    var bmp = UserControl2.Gradient.MakeGradient(Color.DarkOrange, UserControl2.Direction.right, btn);
                    if (Frame < 200 * this.Width / 812 & Frame > 150 * this.Width / 812)
                    {
                        Frame += 0.5;
                    }
                    else if (Frame < 150 * this.Width / 812)
                    {
                        Frame += 6;
                    }
                    else
                    {
                        Frame += 20 * this.Width / 812;
                    }
                    e.Graphics.DrawImage(bmp, new Point((int)Frame, 200));
                    e.Graphics.DrawString("Preview mode : off", label2.Font, new SolidBrush(Color.Black), new Point((int)Frame, 10 + 200));
                    if (Frame >= this.Width)
                    {
                        Frame = 0;
                        anim = false;
                    }
                }
                else
                {
                    var btn = new Button();
                    btn.Width = 200;
                    btn.Height = 50;
                    var bmp = UserControl2.Gradient.MakeGradient(Color.DarkOrange, UserControl2.Direction.right, btn);
                    if (Frame < 200 * this.Width / 812 & Frame > 150 * this.Width / 812)
                    {
                        Frame += 0.5;
                    }
                    else if (Frame < 150 * this.Width / 812)
                    {
                        Frame += 6;
                    }
                    else
                    {
                        Frame += 20 * this.Width / 812;
                    }
                    e.Graphics.DrawImage(bmp, new Point((int)Frame, 200));
                    e.Graphics.DrawString("Preview mode : on", label2.Font, new SolidBrush(Color.Black), new Point((int)Frame, 10 + 200));
                    if (Frame >= this.Width)
                    {
                        Frame = 0;
                        anim = false;
                    }
                }
            }
        }
        private void OnFrameChange2d(object o, EventArgs e) { }

        private void Form1_Load(object sender, EventArgs e) { }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (redact)
            {
                foreach (var text in texts)
                {
                    if (text.image != null)
                    {
                        if (e.X > text.drawPoint.X & e.X < text.drawPoint.X + Graphics.FromImage(pictureBox1.Image).MeasureString(text.text2, text.myFont).Width & e.Y > text.drawPoint.Y & e.Y < Graphics.FromImage(pictureBox1.Image).MeasureString(text.text2, text.myFont).Height + text.drawPoint.Y)
                        {
                            if (text.Link & redact)
                            {
                                text.myColor = Color.Purple;                                
                                text.VisMe(new RichTextBox() { Text = text.text2 }, text.drawPoint, text.myColor);
                                PageNow.page.DrawImage(text.image, 0, 0);
                                pictureBox1.Image = PageNow.imageOfPage;
                            }
                        }
                        else
                        {
                            if(e.Button == MouseButtons.None)
                            {
                                if (text.Link &!redact)
                                {
                                   // PageNow.page.Clear(Color.Transparent);                                                                    
                                    text.myColor = Color.Blue;
                                    text.VisMe(new RichTextBox() { Text = text.text2 }, text.drawPoint, text.myColor);
                                    PageNow.page.DrawImage(text.image, 0, 0);
                                    if(!redact)
                                    {
                                        foreach (var t in texts)
                                        {
                                            PageNow.page.DrawImage(t.image, 0, 0);
                                        }
                                        ImageVisualize();
                                    }                                    
                                    pictureBox1.Image = PageNow.imageOfPage;
                                }
                            }                            
                        }
                    }
                }

            }
            foreach (var im in imagess)
            {
                if (e.X > im.rect.X + im.rect.Width - 4 & e.Y > im.rect.Y)
                {
                    if (e.X <= im.rect.X + im.rect.Width + 4 & e.Y <= im.rect.Y + im.rect.Height)
                    {
                        pictureBox1.Cursor = Cursors.SizeWE;
                    }
                    else
                    {
                        pictureBox1.Cursor = Cursors.Default;
                    }
                }
                else
                {
                    if (e.X < im.rect.X + im.rect.Width & e.X > im.rect.X)
                    {
                        if (e.Y > im.rect.Y & e.Y < im.rect.Y + im.rect.Height)
                        {
                            pictureBox1.Cursor = Cursors.Hand;
                        }
                        else
                        {
                            pictureBox1.Cursor = Cursors.Default;
                        }
                    }
                    else
                    {
                        pictureBox1.Cursor = Cursors.Default;
                    }
                }               

            }
            bool resize = false;
            bool move = false;
            if (e.Button == MouseButtons.Left)
            {
                redact = true;
                foreach (var im in imagess)
                {                    
                    foreach (var image in imagess)
                    {
                        PageNow.page.DrawImage(image.image, image.rect);                        
                        pictureBox1.Image = PageNow.imageOfPage;
                    }
                    if (e.X > im.rect.X + im.rect.Width - 8 & e.Y > im.rect.Y)
                    {
                        if (e.X <= im.rect.X + im.rect.Width + 8 & e.Y <= im.rect.Y + im.rect.Height)
                        {
                            im.rect.Width += e.X - (im.rect.X + im.rect.Width);
                            Func2(true);
                            redact = false;
                            resize = true;
                            button2.Text = "preview";
                            pictureBox1.Image = null;
                            PageNow.page.Clear(Color.Transparent);
                            foreach (var image in imagess)
                            {
                                PageNow.page.DrawImage(image.image, image.rect);
                                pictureBox1.Image = PageNow.imageOfPage;
                            }
                            DrawStroke(4, im.rect);
                        }
                    }
                    else
                    {
                        if (e.X < im.rect.X + im.rect.Width & e.X > im.rect.X)
                        {
                            if (e.Y > im.rect.Y & e.Y < im.rect.Y + im.rect.Height) { }
                        }                        
                    }
                    if (e.X > im.rect.X & e.X < im.rect.X + im.rect.Width - 8)
                    {
                        if (e.Y > im.rect.Y && e.Y < im.rect.Y + im.rect.Height - 8)
                        {
                            im.rect.X += 1;
                            PageNow.page.Clear(Color.Transparent);
                            foreach (var image in imagess)
                            {
                                PageNow.page.DrawImage(image.image, image.rect);
                                pictureBox1.Image = PageNow.imageOfPage;
                            }
                            if (!redact)
                            {
                                RichTextBoxVisualize();
                                Func2(true);
                                redact = false;
                                button2.Text = "preview";
                            }
                            move = true;
                        }
                    }
                }
            }
            bool imageClick = false;
            bool TextClick = false;
            foreach (var im in imagess)
            {
                if (e.X > im.rect.X + im.rect.Width - 4 & e.Y > im.rect.Y)
                {
                    if (e.X <= im.rect.X + im.rect.Width + 4 & e.Y <= im.rect.Y + im.rect.Height)
                    {
                    }
                    else
                    {
                        imageClick = true;
                    }
                }
                else
                {
                    imageClick = true;
                }
            }
            foreach (var im in texts)
            {
                if (e.X > im.drawPoint.X & e.Y > im.drawPoint.Y)
                {
                    if (e.X <= im.drawPoint.X + (int)PageNow.page.MeasureString(im.text2, im.myFont).Width & e.Y <= im.drawPoint.Y + (int)PageNow.page.MeasureString(im.text2, im.myFont).Height)
                    {

                    }
                    else
                    {
                        TextClick = true;
                    }
                }
                else
                {
                    TextClick = true;
                }
            }
            if (e.Button == MouseButtons.Left &!resize &!move)
            {
                if (LastDrawPoint.X == 0&&LastDrawPoint.Y ==0)
                {
                    LastDrawPoint = e.Location;
                }
                DialogResult res = DialogResult.None;
                if(brushColl.A == 0)
                {
                    res = MessageBox.Show(this,"Chose brush collor!","Error",MessageBoxButtons.YesNoCancel,MessageBoxIcon.Error);
                }
                if (res == DialogResult.Yes)
                {
                    Form2 f = new Form2(false);
                    f.Show();
                    BrushColor = true;
                }               

                DrawLine(LastDrawPoint, e.Location, 10);                               
                pictureBox1.Image = PageNow.imageOfPage;
                LastDrawPoint = e.Location;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var image = new Image2();
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image files (*.png) | *.png";
            if (open.ShowDialog() == DialogResult.OK)
            {
                image.image = new Bitmap(open.FileName);
                image.rect = new Rectangle(100, 200, image.image.Width, image.image.Height);
                image.layer = counter;
                imagess.Add(image);
                var loadSett = LoadSettings();
                loadSett.pages[pageNum].images.Add(image);
                SaveSettings(loadSett);
            }
            PageNow.page.Clear(Color.Transparent);
            foreach (var image2 in imagess)
            {
                PageNow.page.DrawImage(image2.image, image2.rect);
            }
            pictureBox1.Image = PageNow.imageOfPage;
        }

        private void button7_Click(object sender, EventArgs e) { }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image files (*.png) | *.png";
            if (open.ShowDialog() == DialogResult.OK)
            {
                var loadsett = LoadSettings();
                loadsett.pages[pageNum].BackGround = new Bitmap(open.FileName);
                pictureBox1.BackgroundImage = loadsett.pages[pageNum].BackGround;
                SaveSettings(loadsett);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Func2(true);
            redact = true;
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
            texts[texts.Count - 1].Link = true;
            texts[texts.Count - 1].myColor = Color.Blue;
            layers.Add(counter);
            richTextBoxes.Add(textBox);
            counter++;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (var text in texts)
            {
                if (text.image != null)
                {
                    if (e.X > text.drawPoint.X & e.X < text.drawPoint.X + Graphics.FromImage(PageNow.imageOfPage).MeasureString(text.text2, text.myFont).Width & e.Y > text.drawPoint.Y & e.Y < Graphics.FromImage(PageNow.imageOfPage).MeasureString(text.text2, text.myFont).Height + text.drawPoint.Y)
                    {
                        if (text.Link)
                        {
                            try
                            {
                                System.Diagnostics.Process.Start(text.text2);
                            }
                            catch
                            {

                            }
                        }
                    }
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            LastDrawPoint.Y = 0;
            LastDrawPoint.X = 0;
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            Form2 f = new Form2(false);
            f.Show();
            BrushColor = true;
        }

        private void userControl21_Load(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            PageNow.page.ScaleTransform(1.0F, 2.0F);
            pictureBox1.Image = PageNow.imageOfPage;
        }
    }
}
