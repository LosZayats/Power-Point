using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Input;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing.Printing;

namespace WindowsFormsApp66
{
    public partial class Form1 : Form
    {
        const string SettingsFileName = "презентация.ptx";
        const string SettingsFileName2 = "table.ptx";
        [Serializable] //Это атрибут, который позволяет автоматически сериализовать и десериализовать 
        public struct ProgrammSettings
        {
            public List<Page2> pages;
        }
        [Serializable]
        public class Item
        {
            public Rectangle myRect;
            public Text1 Text;
            public Item()
            {
                if (myRect == null)
                {
                    myRect = new Rectangle();
                }
            }
            public void Resize()
            {
                Graphics myGraphics = Graphics.FromImage(new Bitmap(1, 1));
                myRect.Width = (int)myGraphics.MeasureString(Text.text2, Text.myFont).Width + 10;
                myRect.Height = (int)myGraphics.MeasureString(Text.text2, Text.myFont).Height + 10;
            }
            [NonSerialized]
            public RichTextBox richTextBox;
        }
        [Serializable]
        public struct ProgrammSettings2
        {
            public List<Table> tables;
        }
        public static ProgrammSettings2 LoadSettings2()
        {
            if (File.Exists(SettingsFileName2))
            {
                using (Stream stream = File.Open(SettingsFileName2, FileMode.Open))
                {
                    var formatter = new BinaryFormatter();
                    return (ProgrammSettings2)formatter.Deserialize(stream);
                }
            }
            return
        default(ProgrammSettings2);
        }
        public static void SaveSettings2(ProgrammSettings2 settings)
        {
            using (Stream stream = File.Open(SettingsFileName2, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, settings);
            }
        }
        [Serializable]
        public class Table
        {
            public int myPage
            {
                get;
                private set;
            }
            public List<Item> items = new List<Item>();
            public int Column;
            public int Row;
            public Table(int columns, int rows, int PageNumber)
            {
                Column = columns;
                Row = rows;
                myInd = LoadSettings2().tables.Count;
                for (int ind = 0; ind < Row * columns; ind++)
                {
                    var text = new Text1() { myColor = Color.Black };
                    text.myFont = new Font(new FontFamily("Arial"), 20.0F, FontStyle.Bold);
                    text.text2 = "item";
                    myPage = PageNumber;
                    items.Add(new Item() { Text = text });
                }
                myPage = PageNumber;
                var loadSettings = Form1.LoadSettings2();
                if (loadSettings.tables == null)
                {
                    loadSettings.tables = new List<Table>();
                }
                loadSettings.tables.Add(this);
                SaveSettings2(loadSettings);
            }
            public int myInd
            {
                get;
                private set;
            }
            public Size Vis()
            {
                int col = 0;
                int row = 0;
                int y = 150;
                int x = 100;
                int max = 0;
                foreach (var text in items)
                {
                    text.Resize();
                }
                int max2 = 0;
                foreach (var text in items)
                {
                    if (text.myRect.Width > max)
                    {
                        max = text.myRect.Width;
                    }
                    if (text.myRect.Height > max2)
                    {
                        max2 = text.myRect.Height;
                    }
                }
                foreach (var t in items)
                {
                    t.myRect.Width = max;
                    t.myRect.Height = max2;
                    t.myRect.X = 0;
                    t.myRect.Y = 0;
                }
                foreach (var t in items)
                {
                    col++;
                    RichTextBox textbox = null;
                    var str = this.myInd.ToString() + items.IndexOf(t).ToString();
                    foreach (var text in Form1.richTextBoxes2)
                    {
                        if (text.Name == str)
                        {
                            textbox = text;
                            break;
                        }
                    }
                    t.Text.text2 = textbox.Text;
                    var loadSettings = Form1.LoadSettings2();
                    if (loadSettings.tables == null)
                    {
                        loadSettings.tables = new List<Table>();
                    }
                    t.myRect.Y += y;
                    t.myRect.X += x;
                    x = t.myRect.X + t.myRect.Width;
                    y = t.myRect.Y;
                    void oper() => Form1.PageNow.page.DrawString(t.Text.text2, t.Text.myFont, new SolidBrush(t.Text.myColor), new Point(t.myRect.X + 5, t.myRect.Y + 5));
                    if (Form1.redact == true)
                    {
                        oper();
                    }
                    Form1.PageNow.page.DrawRectangle(new Pen(new SolidBrush(Color.Black), 3), t.myRect);
                    loadSettings.tables[myInd] = this;
                    var ind = -1;
                    foreach (var textBox in Form1.richTextBoxes2)
                    {
                        if (textBox.Name == str)
                        {
                            textBox.Top = t.myRect.Y;
                            textBox.Left = t.myRect.X;
                        }
                    }
                    if (col == Column)
                    {
                        col = 0;
                        row++;
                        y += t.myRect.Height;
                        x = 100;
                    }
                    if (row == Row)
                    {
                        SaveSettings2(loadSettings);
                        return items[0].myRect.Size;
                    }
                    GC.Collect();
                }
                return items[0].myRect.Size;
            }
            public Size RetSize()
            {
                return items[0].myRect.Size;
            }
        }
        public static int PageNumForOut;
        class Shape : Attribute
        {
            public enum Shapes
            {
                Circle, Rectangle, None
            }
            public Shapes myShape = Shapes.None;
        }
        [Shape]
        public class VectorGraphics
        {
            Rectangle drawRect = new Rectangle();
            Graphics graphics = Graphics.FromImage(new Bitmap(1920, 1080));
            public void VisMe()
            {
                graphics.Clear(Color.Transparent);
                var type = typeof(VectorGraphics);
                var attrs = type.GetCustomAttributes(false);
                var attrCust = (Shape)attrs[0];
                attrCust.myShape = Shape.Shapes.Circle;                
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
        public static int Thikness = 1;
        public void Parentness()
        {
            foreach (var t in richTextBoxes2)
            {
                var ff = LoadSettings2().tables[int.Parse(t.Name[0].ToString())];
                var gg = LoadSettings2().tables[int.Parse(t.Name[0].ToString())].myPage;
                if (LoadSettings2().tables[int.Parse(t.Name[0].ToString())].myPage == pageNum)
                {
                    t.Parent = this;
                    t.BringToFront();
                }
            }
        }
        public void Reffresh()
        {
            richTextBoxes2.Clear();
            var load2 = LoadSettings2();
            var ind1 = -1;
            var ind2 = -1;
            if (load2.tables == null)
            {
                load2.tables = new List<Table>();
            }
            foreach (var table2 in load2.tables)
            {
                ind1++;
                ind2 = -1;
                var row = 0;
                var column = 0;
                var y = 150;
                var x = 100;
                foreach (var item in table2.items)
                {
                    column++;
                    ind2++;
                    item.Resize();
                    var rich = new RichTextBox();
                    rich.BackColor = Color.LightBlue;
                    rich.Text = item.Text.text2;
                    item.Resize();
                    rich.Size = new Size(item.myRect.Width, item.myRect.Height);
                    item.richTextBox = rich;
                    richTextBoxes2.Add(rich);
                    rich.BorderStyle = BorderStyle.None;
                    rich.BringToFront();
                    rich.Name = ind1.ToString() + ind2.ToString();
                    rich.Top = y;
                    rich.Left = x;
                    rich.Font = item.Text.myFont;
                    rich.TextChanged += textBox1_TextChanged_1;
                    if (row == table2.Row && column == table2.Column)
                    {
                        break;
                    }
                    if (column == table2.Column)
                    {
                        column = 0;
                        row++;
                        y += item.myRect.Height;
                        x = 100;
                    }
                    else
                    {
                        x += item.myRect.Width;
                    }
                }
            }
        }
        bool print2;
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
                RichTextBoxVisualize();
                ImageVisualize();
            }
            foreach (var con in this.Controls)
            {
                var con2 = (Control)con;
                con2.MouseDown += panel3_MouseDown;
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
            pictureBox1.Cursor = new Cursor(Properties.Resources.pen1.Handle);
            panels2.Add(panel7);
            panels2.Add(panel8);
            panel2.Controls.Add(label3);
            font = label1.Font;
            Form4 f3 = new Form4();
            // var table = new Table(2, 2, pagenum); 
            var form4 = new Form4();
            form4.Show();
            Reffresh();
            Parentness();
            TableVis();
        }
        public void TableVis()
        {
            int ind = 0;
            if (LoadSettings2().tables == null)
            {
                var load = LoadSettings2();
                load.tables = new List<Table>();
                SaveSettings2(load);
            }
            foreach (var table2 in LoadSettings2().tables)
            {
                if (table2.myPage == pageNum)
                {
                    table2.Vis();
                    ind++;
                }
            }
            pictureBox1.Image = PageNow.imageOfPage;
        }
        bool draw;
        Bitmap header;
        bool loading = false;
        public static bool redact = false;
        Point ClickOnImagePoint;
        bool ImageMove;
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
                PageNumForOut = value;
            }
        }
        public void DrawLine(Graphics gr, Point Point1, Point Point2, Color col, int size)
        {
            var fun = (double)(Point2.Y - Point1.Y) / (double)(Point2.X - Point1.X);
            for (double x = 0; x < (Point2.X - Point1.X); x += 0.005)
            {
                gr.FillRectangle(new SolidBrush(col), new Rectangle((int)(x + Point1.X), (int)(x * fun) + Point1.Y, size, size));
            }
            if ((Point2.X - Point1.X) < 0)
            {
                for (double x = 0; x > (Point2.X - Point1.X); x -= 0.005)
                {
                    gr.FillRectangle(new SolidBrush(col), new Rectangle((int)(x + Point1.X), (int)(x * fun) + Point1.Y, size, size));
                }
            }
            if ((Point2.X - Point1.X) == 0)
            {
                if (Point2.Y - Point1.Y > 0)
                {
                    gr.FillRectangle(new SolidBrush(col), new Rectangle(Point1.X, Point1.Y, size, Point2.Y - Point1.Y));
                }
                else
                {
                    gr.FillRectangle(new SolidBrush(col), new Rectangle(Point1.X, Point2.Y, size, Math.Abs(Point2.Y - Point1.Y)));
                }
            }
        }
        static Rectangle scale = new Rectangle(0, 0, 1920, 1080);
        List<Panel> panels = new List<Panel>();
        List<Panel> panels2 = new List<Panel>();
        public static List<Text1> texts = new List<Text1>();
        public static List<RichTextBox> richTextBoxes = new List<RichTextBox>();
        public static List<RichTextBox> richTextBoxes2 = new List<RichTextBox>();
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
        Point drawLoc = new Point();
        public static bool Circle;
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
        static Bitmap bmp = new Bitmap(scale.Width, scale.Height);
        public static Graphics grph = Graphics.FromImage(bmp);
        [Serializable]
        public class Page2
        {
            public Bitmap BackGround;
            public List<Text1> texts2 = new List<Text1>();
            public List<int> layers2 = new List<int>();
            public List<Image2> images = new List<Image2>();
            public int counter;
        }
        public static List<Image2> imagess = new List<Image2>();
        [Serializable]
        public class Text1
        {
            public bool Link;
            public Font myFont = Form1.font;
            public Color myColor
            {
                get;
                set;
            }

            public Bitmap image = new Bitmap(1920, 1080);
            public System.Drawing.Point drawPoint = new System.Drawing.Point();
            public string text2;
            public void VisMe(RichTextBox text, System.Drawing.Point drawPoint, Color c)
            {
                var graph = Graphics.FromImage(image);
                graph.Clear(Color.Transparent);
                this.drawPoint = drawPoint;
                if (myFont == null)
                {
                    myFont = Form1.font;
                }
                graph.DrawString(text.Text, myFont, new SolidBrush(myColor), drawPoint);
                text2 = text.Text;
                graph.Dispose();
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
                if (send.Top <= 100 && e.Y < DownPoint.Y)
                {
                    send.Top = 100;
                }
                else if (send.Top + send.Height >= this.Height - 125 + send.Height && e.Y > DownPoint.Y)
                {
                    send.Top = this.Height - (125);
                }
                else
                {
                    send.Top += e.Y - DownPoint.Y;
                    send.BringToFront();
                }
                if (send.Left + send.Width >= this.Width - 20 && e.X > DownPoint.X)
                {
                    send.Left = this.Width - 14 - send.Width;
                }
                else if (send.Left <= 0 && e.X < DownPoint.X)
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
        public void vis(Rectangle margin)
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
                    var prop2 = (double)LoadSettings().pages[pageNum].BackGround.Height / (double)LoadSettings().pages[pageNum].BackGround.Width;
                    pictureBox1.BackgroundImage = null;
                    PageNow.page.DrawImage(LoadSettings().pages[pageNum].BackGround, 0, 0, margin.Width, (float)(margin.Width * prop2));
                }                
                foreach (var image in imagess)
                {
                    PageNow.page.DrawImage(image.image, image.rect);
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
            TableVis();
            pictureBox1.Image = PageNow.imageOfPage;
            loading = false;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if(!print2)
            {
                anim = true;
                loading = true;
            }            
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
                    foreach (var image in imagess)
                    {
                        PageNow.page.DrawImage(image.image, image.rect);
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
            TableVis();
            pictureBox1.Image = PageNow.imageOfPage;
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
            foreach (var table in LoadSettings2().tables)
            {
                if (table.myPage == pageNum)
                {
                    Reffresh();
                    Parentness();
                }
            }
            TableVis();
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
            foreach (var table in LoadSettings2().tables)
            {
                if (table.myPage == pageNum)
                {
                    Reffresh();
                    Parentness();
                }
            }
            TableVis();
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
            if (ImageMove)
            {


            }
        }
        private void OnFrameChange2d(object o, EventArgs e) { }

        private void Form1_Load(object sender, EventArgs e) { }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (button2.Text == "redact")
            {
                pictureBox1.Cursor = new Cursor(Properties.Resources.pen1.Handle);
            }
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
                            if (e.Button == MouseButtons.None)
                            {
                                if (text.Link & button2.Text == "redact")
                                {
                                    // PageNow.page.Clear(Color.Transparent);                                                                    
                                    text.myColor = Color.Blue;
                                    text.VisMe(new RichTextBox() { Text = text.text2 }, text.drawPoint, text.myColor);
                                    PageNow.page.DrawImage(text.image, 0, 0);
                                    if (!redact)
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
                    if (e.X <= im.rect.X + im.rect.Width + 4 & e.Y <= im.rect.Y + im.rect.Height && button2.Text == "preview")
                    {
                        pictureBox1.Cursor = Cursors.SizeWE;
                    }
                    else
                    {

                    }
                }
                else
                {
                    if (e.X < im.rect.X + im.rect.Width - 16 & e.X > im.rect.X + 16)
                    {
                        if (e.Y > im.rect.Y + 16 & e.Y < im.rect.Y + im.rect.Height - 16)
                        {
                            pictureBox1.Cursor = Cursors.Hand;
                        }
                        else
                        {
                            //pictureBox1.Cursor = new Cursor(Properties.Resources.pen1.Handle);
                        }
                    }
                    else
                    {
                        //pictureBox1.Cursor = new Cursor(Properties.Resources.pen1.Handle);
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
                        if (e.X <= im.rect.X + im.rect.Width + 8 & e.Y <= im.rect.Y + im.rect.Height && button2.Text == "preview")
                        {
                            im.rect.Width += e.X - (im.rect.X + im.rect.Width);
                            pictureBox1.Image = null;
                            PageNow.page.Clear(Color.Transparent);
                            resize = true;
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
                        if (e.Y > im.rect.Y && e.Y < im.rect.Y + im.rect.Height - 8 && button2.Text == "preview" && !resize)
                        {
                            im.rect.Y += e.Y - ClickOnImagePoint.Y;
                            im.rect.X += e.X - ClickOnImagePoint.X;
                            ClickOnImagePoint = e.Location;
                            PageNow.page.Clear(Color.Transparent);
                            foreach (var image in imagess)
                            {
                                PageNow.page.DrawImage(image.image, image.rect);
                                DrawStroke(4, image.rect);
                            }
                            pictureBox1.Image = PageNow.imageOfPage;
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
            if (e.Button == MouseButtons.Left & !resize & !move && button2.Text == "redact")
            {
                draw = true;
                drawLoc = e.Location;
                pictureBox1.Refresh();
                if (LastDrawPoint.X == 0 && LastDrawPoint.Y == 0)
                {
                    LastDrawPoint = e.Location;
                }
                DialogResult res = DialogResult.None;
                if (brushColl.A == 0)
                {
                    LastDrawPoint.X = 0;
                    LastDrawPoint.Y = 0;
                    res = MessageBox.Show(this, "Chose brush collor!", "Error", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error);
                }
                if (res == DialogResult.Yes)
                {
                    Form2 f = new Form2(false);
                    f.Show();
                    BrushColor = true;
                }
                scale.Width = 1920;
                scale.Height = 1080;
                DrawLine(PageNow.page,LastDrawPoint, e.Location, brushColl, Thikness);
                LastDrawPoint = e.Location;
                pictureBox1.Image = PageNow.imageOfPage;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var image = new Image2();
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image files (*.png) | *.png";
            if (redact)
            {
                if (!redact)
                {
                    RichTextBoxVisualize();
                }
                redact = false;
            }
            if (open.ShowDialog() == DialogResult.OK)
            {
                image.image = new Bitmap(open.FileName);
                image.rect = new Rectangle(100, 200, image.image.Width, image.image.Height);
                image.layer = counter;
                imagess.Add(image);
                var loadSett = LoadSettings();
                loadSett.pages[pageNum].images.Add(image);
                SaveSettings(loadSett);
                DrawStroke(4, image.rect);
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
            foreach (var t in imagess)
            {
                if (e.X > t.rect.X && e.Y > t.rect.Y)
                {
                    if (e.X < t.rect.X + t.rect.Width && e.Y < t.rect.Y + t.rect.Height)
                    {
                        ClickOnImagePoint = e.Location;
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
            var f = new Form3();
            f.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (scale.Width == 0)
            {
                scale.Width = PageNow.imageOfPage.Width;
                scale.Height = PageNow.imageOfPage.Height;
            }
            bmp = new Bitmap((int)((double)scale.Width * 1.25), (int)((double)scale.Height * 1.25));
            grph = Graphics.FromImage(bmp);
            grph.DrawImage(PageNow.imageOfPage, new Rectangle(0, 0, (int)((double)scale.Width * 1.25), (int)((double)scale.Height * 1.25)));
            scale.Size = new Size((int)((double)scale.Width * 1.25), (int)((double)scale.Height * 1.25));
            pictureBox1.Image = bmp;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (scale.Width == 0)
            {
                scale.Width = PageNow.imageOfPage.Width;
                scale.Height = PageNow.imageOfPage.Height;
            }
            bmp = new Bitmap((int)((double)scale.Width * 0.75), (int)((double)scale.Height * 0.75));
            grph = Graphics.FromImage(bmp);
            grph.DrawImage(PageNow.imageOfPage, new Rectangle(0, 0, (int)((double)scale.Width * 0.75), (int)((double)scale.Height * 0.75)));
            scale.Size = new Size((int)((double)scale.Width * 0.75), (int)((double)scale.Height * 0.75));
            pictureBox1.Image = bmp;
        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            //var sender2 = (Control)sender;
            //sender2.BackColor = Color.Beige;
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            var send = (RichTextBox)sender;
            var size = PageNow.page.MeasureString(send.Text, send.Font);
            var LastSize = send.Size;
            send.Size = new Size((int)size.Width + 30, (int)size.Height + 30);
            var max = 0;
            var max2 = 0;
            foreach (var t in richTextBoxes2)
            {
                if (t.Width > max)
                {
                    max = t.Width;
                }
                if (t.Height > max2)
                {
                    max2 = t.Height;
                }
            }
            if (LastSize.Height != max2 | LastSize.Width != max)
            {
                PageNow.page.Clear(Color.Transparent);
                ImageVisualize();
                TableVis();
            }
            pictureBox1.Image = PageNow.imageOfPage;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Form5 f = new Form5();
            f.Show();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            print2 = true;
            var print = new PrintDocument();            
            print.PrintPage += PrintPageEventHandler;
            print.DefaultPageSettings.Landscape = true;
            print.Print();
            print2 = true;

        }
        public void PrintPageEventHandler(object sender, PrintPageEventArgs e)
        {
            vis(e.MarginBounds);
            var prop = (double)PageNow.imageOfPage.Height / (double)PageNow.imageOfPage.Width;
            var prop2 = (double)pictureBox1.BackgroundImage.Height / (double)pictureBox1.BackgroundImage.Width;
            e.Graphics.DrawImage(pictureBox1.BackgroundImage,0,0,e.MarginBounds.Width, (float)(e.MarginBounds.Width* prop2));
            //e.Graphics.DrawImage(PageNow.imageOfPage, 0, 0, e.MarginBounds.Width, (float)(e.MarginBounds.Width* prop));
        }
    }
}
