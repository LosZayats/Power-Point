using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MATH_2;
using System.Drawing.Drawing2D;

namespace WindowsFormsApp66
{
    public partial class Form4 : Form
    {
        public class HScrollBar : IScrollAble
        {
            Bitmap Caret
            {
                get; set;
            }
            public double Value
            {
                get; set;
            }
            public int X
            {
                get; set;
            }
            public double GetValue()
            {
                return Value;
            }
            public int Y
            {
                get; set;
            }
            public void SetX(int x)
            {
                X = x;
            }
            public void SetY(int y)
            {
                Y = y;
            }
            public int Width
            {
                get; set;
            }
            public int Height
            {
                get; set;
            }
            public System.Drawing.Brush CaretBrush
            {
                get; set;
            }
            public int GetY()
            {
                return (int)(Value * (double)Height);
            }
            public int GetX()
            {
                return -1;
            }
            public System.Drawing.Brush Line
            {
                get; set;
            }
            Bitmap Arrow;
            int arrowheight = 20;
            public void CreateArrow()
            {
                var penwidth = 3;
                var height = Height;
                var pen = new Pen(new SolidBrush(Color.DarkGray).Color, 2);
                Arrow = new Bitmap(this.Width, arrowheight);
                var gr = Graphics.FromImage(Arrow);
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                var point1 = new PointF(4, arrowheight - 4);
                var point2 = new PointF(this.Width / 2, 0 + 4);
                var point3 = new PointF(this.Width - 4, arrowheight - 4);
                var array = new List<PointF>();
                array.Add(point2);
                array.Add(point3);
                array.Add(point1);
                gr.DrawLine(pen, point1, point2);
                gr.DrawLine(pen, point3, point2);
            }
            public HScrollBar(Rectangle rect, System.Drawing.Brush line, System.Drawing.Brush caretBrush, double StartValue = 0)
            {
                X = rect.X;
                Y = rect.Y;
                Width = rect.Width;
                Height = rect.Height;
                CaretBrush = caretBrush;
                Line = line;
                CreateCaret();
                Value = (double)arrowheight / (double)Height;
                CreateArrow();
                if (StartValue != 0)
                {
                    Value = StartValue;
                }
            }
            const int height = 20;
            public void CreateCaret()
            {
                var penwidth = 3;
                var width = Width;
                Caret = new Bitmap(width, height + penwidth);
                var gr = Graphics.FromImage(Caret);
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                var rect = new Rectangle(0, 0, width, height);
                gr.FillRectangle(CaretBrush, rect);
            }
            const int gradesWidth = 20;
            private Matrix RotateAroundPoint(float angle, Point center)
            {
                // Translate the point to the origin.
                Matrix result = new Matrix();
                result.RotateAt(angle, center);
                return result;
            }
            public void Visualize(Graphics gr)
            {
                var penWidth = 2;
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                gr.FillRectangle(Line, new Rectangle(X + Width / 2 - Width / 2, Y, Width, Height));
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                gr.DrawImage(Caret, X, Y + (int)((double)Height * Value));
                gr.DrawImage(Arrow, new Point(this.X, this.Y));
                var arrowCopy = new Bitmap(Arrow.Width, Arrow.Height);
                var gr2 = Graphics.FromImage(arrowCopy);
                gr2.Transform = RotateAroundPoint(180, new Point(this.Width / 2, arrowheight / 2));
                gr2.DrawImage(Arrow, 0, 0);
                gr.DrawImage(arrowCopy, new Point(this.X, this.Y + this.Height - arrowheight));
            }
            public bool Scroll(Point click, Point realPoint)
            {
                var clicked = false;
                if (realPoint.X > X)
                {
                    if (realPoint.Y - height > Y)
                    {
                        if (realPoint.X < X + Width)
                        {
                            if (realPoint.Y + height * 2 < Y + Height)
                            {
                                clicked = true;
                            }
                        }
                    }
                }
                if (clicked)
                {
                    if ((double)click.Y / (double)(Height + Y) >= 0 && (double)click.Y / (double)(Height + Y) <= 1)
                    {
                        Value = (double)(click.Y - Y) / (double)(Height);
                    }
                }
                return clicked;
            }
        }
        class Shape
        {
            //Properties
            SolidBrush Color { get; set; }
            Graphics Graphics { get; set; }
            double Thickness { get; set; }
            public List<Point> Points { get; set; }
            //Methods            
            public Shape(SolidBrush color, double thikness = 0)
            {
                Color = color;
                Thickness = thikness;
                Points = new List<Point>();
            }
            public Rectangle GetBounds()
            {
                var listX = new List<double>();
                var listY = new List<double>();
                foreach (var point in Points)
                {
                    listX.Add(point.X);
                    listY.Add(point.X);
                }
                var maxX = Math2.GetBiggestNumber(listX.ToArray());
                var maxY = Math2.GetBiggestNumber(listY.ToArray());
                var minX = Math2.GetLowestNumber(listX.ToArray());
                var minY = Math2.GetLowestNumber(listY.ToArray());
                var rect = new Rectangle((int)minX, (int)minY, (int)maxX - (int)minX, (int)maxY - (int)minY);
                return rect;
            }
            public void Scale(double percent)
            {
                var bounds = GetBounds();
                for(int i =0;i<Points.Count;i++)
                {
                    var point = Points[i];
                    
                    Points[i] = new Point((int)(point.X* percent), (int)(point.Y * percent));
                }
            }
            public void Visualize(Graphics gr,Rectangle bounds)
            {
                Graphics = gr;
                for (int index = 0; index < Points.Count - 1; index++)
                {
                    var point1 = Points[index];
                    var point2 = Points[index + 1];
                    if (point1.X+bounds.X>0&&point1.X + bounds.X < bounds.Width&&point1.Y + bounds.Y> 0&&point1.Y + bounds.Y < bounds.Height)
                    {
                        if (point2.X + bounds.X > 0 && point2.X + bounds.X < bounds.Width && point2.Y + bounds.Y > 0 && point2.Y + bounds.Y < bounds.Height)
                        {
                            //point1 = new Point(point1.X + bounds.X, point1.Y + bounds.Y);
                            //point2 = new Point(point2.X + bounds.X, point2.Y + bounds.Y);
                            Graphics.DrawLine(new Pen(Color, (int)Thickness), point1, point2);
                            Graphics.FillEllipse(Color, new Rectangle((int)(point1.X - Thickness / 2), (int)(point1.Y - Thickness / 2), (int)Thickness, (int)Thickness));
                            Graphics.FillEllipse(Color, new Rectangle((int)(point2.X - Thickness / 2), (int)(point2.Y - Thickness / 2), (int)Thickness, (int)Thickness));
                        }
                    }                   
                  
                }
            }
        }
        public class ScrollBar : IScrollAble
        {
            Bitmap Caret
            {
                get; set;
            }
            public double Value
            {
                get; set;
            }
            public double GetValue()
            {
                return Value;
            }
            public void SetX(int x)
            {
                X = x;
            }
            public void SetY(int y)
            {
                Y = y;
            }
            public int X
            {
                get; set;
            }
            public int Y
            {
                get; set;
            }
            public int Width
            {
                get; set;
            }
            public int Height
            {
                get; set;
            }
            public System.Drawing.Brush CaretBrush
            {
                get; set;
            }
            public System.Drawing.Brush Line
            {
                get; set;
            }
            public int GetY()
            {
                return -1;
            }
            public ScrollBar(Rectangle rect, System.Drawing.Brush line, System.Drawing.Brush caretBrush, double StartValue = 0)
            {
                X = rect.X;
                Y = rect.Y;
                Width = rect.Width;
                Height = rect.Height;
                CaretBrush = caretBrush;
                Line = line;
                CreateCaret();
                CreateArrow();
                Value = (double)arrowwidth / (double)Width;
                if (StartValue != 0)
                {
                    Value = StartValue;
                }

            }
            public void CreateCaret()
            {
                var penwidth = 3;
                var height = Height;
                Caret = new Bitmap(width + penwidth, height);
                var gr = Graphics.FromImage(Caret);
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                var rect = new Rectangle(0, 0, width, height);
                gr.FillRectangle(CaretBrush, rect);
            }
            Bitmap Arrow;
            int arrowwidth = 20;
            public void CreateArrow()
            {
                var penwidth = 3;
                var height = Height;
                var pen = new Pen(new SolidBrush(Color.DarkGray).Color, 2);
                Arrow = new Bitmap(arrowwidth, this.Height);
                var gr = Graphics.FromImage(Arrow);
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                var point1 = new PointF(4, this.Height - 4);
                var point2 = new PointF(arrowwidth / 2, 0 + 4);
                var point3 = new PointF(arrowwidth - 4, this.Height - 4);
                var array = new List<PointF>();
                array.Add(point2);
                array.Add(point3);
                array.Add(point1);
                gr.DrawLine(pen, point1, point2);
                gr.DrawLine(pen, point3, point2);
            }
            private Matrix RotateAroundPoint(float angle, Point center)
            {
                // Translate the point to the origin.
                Matrix result = new Matrix();
                result.RotateAt(angle, center);
                return result;
            }
            const int gradesHeight = 20;
            public void Visualize(Graphics gr)
            {
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                gr.FillRectangle(Line, new Rectangle(X, Y - Height / 2 + Height / 2, Width, Height));
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                gr.DrawImage(Caret, X + (int)(Value * (double)Width), Y);
                var arrowCopy = new Bitmap(Arrow.Width, Arrow.Height);
                var arrowCopy2 = new Bitmap(Arrow.Width, Arrow.Height);
                var gr2 = Graphics.FromImage(arrowCopy);
                gr2.Transform = RotateAroundPoint(-90, new Point(arrowwidth / 2, Height / 2));
                gr2.DrawImage(Arrow, new Point(0, 0));
                gr.DrawImage(arrowCopy, new Point(this.X, this.Y));
                gr2 = Graphics.FromImage(arrowCopy2);
                gr2.Transform = RotateAroundPoint(90, new Point(arrowwidth / 2, Height / 2));
                gr2.DrawImage(Arrow, new Point(0, 0));
                gr.DrawImage(arrowCopy2, new Point(this.Width - arrowwidth + this.X, this.Y));
            }
            public int GetX()
            {
                return (int)(Value * (double)Width);
            }
            const int width = 20;
            public bool Scroll(Point click, Point realPoint)
            {
                var clicked = false;
                if (realPoint.X - width > X)
                {
                    if (realPoint.Y > Y)
                    {
                        if (realPoint.X + width * 2 < X + Width)
                        {
                            if (realPoint.Y < Y + Height)
                            {
                                clicked = true;
                            }
                        }
                    }
                }
                if (clicked)
                {
                    if ((double)click.X / (double)(Width + X) >= 0 && (double)click.X / (double)(Width + X) <= 1)
                    {
                        Value = (double)(click.X - X) / (double)(Width);
                    }
                }
                return clicked;
            }
        }
        List<Color> baseColors = new List<Color>();
        public interface IScrollAble
        {
            bool Scroll(Point click, Point RealPoint);
            void Visualize(Graphics gr);
            int GetX();
            int GetY();
            void SetX(int x);
            void SetY(int y);
            double GetValue();
        }
        IScrollAble xCord;
        IScrollAble yCord;
        Rectangle drawingSpace;
        double scale = 1;
        double second = 0;
        int coolDown = 2;
        bool cool;
        Bitmap rainball;
        double lastXValue;
        Bitmap picker;
        enum tools
        {
            brush, transparent
        }
        Bitmap picture = new Bitmap(SystemInformation.PrimaryMonitorSize.Width, SystemInformation.PrimaryMonitorSize.Height);
        Graphics painting;
        List<Shape> shapes = new List<Shape>();
        tools tool = tools.brush;
        double lastYValue;
        public void PlaceByX()
        {
            var deltaValue = xCord.GetValue();
            drawingSpace.X = (int)(drawingSpace.Width * (deltaValue - 0.5)) + pictureBox1.Width / 2 - drawingSpace.Width / 2;
            Console.WriteLine((drawingSpace.Width * deltaValue));
            lastXValue = xCord.GetValue();
        }
        public void PlaceByY()
        {
            var deltaValue = yCord.GetValue();
            drawingSpace.Y = (int)(drawingSpace.Height * (deltaValue - 0.5)) + pictureBox1.Height / 2 - drawingSpace.Height / 2;
            Console.WriteLine((drawingSpace.Height * deltaValue));
            lastYValue = yCord.GetValue();
        }
        public void ResizeWorkingSpace()
        {
            drawingSpace.Height = (int)(selectedImage.Height * scale);
            drawingSpace.Width = (int)(selectedImage.Width * scale);
        }
        public void Start()
        {
            xCord = new ScrollBar(new Rectangle(0, pictureBox1.Height - 20, pictureBox1.Width - 20, 20), new SolidBrush(Color.LightGray), new SolidBrush(Color.DarkGray), 0.5);
            yCord = new HScrollBar(new Rectangle(pictureBox1.Width - 20, 0, 20, pictureBox1.Height), new SolidBrush(Color.LightGray), new SolidBrush(Color.DarkGray), 0.5);
            drawingSpace = new Rectangle(-500, 100, 1000, 200);
            lastXValue = xCord.GetValue();
            lastYValue = yCord.GetValue();
        }
        Bitmap selectedImage = Properties.Resources.f9d4a24d_724d_4e09_9382_691399ce9fcc_200x200;
        public Bitmap CreateColorPicker(Color Base)
        {
            var bmp = new Bitmap(255, 255);
            var deltaR = 255 - Base.R;
            var deltaB = 255 - Base.B;
            var deltaG = 255 - Base.G;
            var x = 0;
            for (int white = 255; white > 0; white--)
            {
                var r = 255 - (deltaR / 255.0) * white;
                var g = 255 - (deltaG / 255.0) * white;
                var b = 255 - (deltaB / 255.0) * white;
                var baseColor = Color.FromArgb((int)r, (int)g, (int)b);
                var y = 0;
                for (int delta = 0; delta < 255; delta++)
                {
                    var r2 = r - delta;
                    var g2 = g - delta;
                    var b2 = b - delta;
                    if (r2 < 0)
                    {
                        r2 = 0;
                    }
                    if (g2 < 0)
                    {
                        g2 = 0;
                    }
                    if (b2 < 0)
                    {
                        b2 = 0;
                    }
                    bmp.SetPixel(254 - x, y, Color.FromArgb((int)r2, (int)g2, (int)b2));
                    y++;
                }
                x++;
            }
            return bmp;
        }
        public Bitmap CreateRainBall()
        {
            var bmp = new Bitmap(rainballWidth, rainballHeight);
            var gr = Graphics.FromImage(bmp);
            var widthPerColor = (double)rainballWidth / baseColors.Count;
            var x = 0;
            for (int index = 0; index < baseColors.Count - 1; index++)
            {
                var colorPrevious = baseColors[index];
                var colorNow = baseColors[index + 1];
                var deltaR = (colorNow.R - colorPrevious.R) / widthPerColor;
                var deltaG = (colorNow.G - colorPrevious.G) / widthPerColor;
                var deltaB = (colorNow.B - colorPrevious.B) / widthPerColor;
                for (int factor = 0; factor < widthPerColor; factor++)
                {
                    var r = colorPrevious.R + deltaR * factor;
                    var g = colorPrevious.G + deltaG * factor;
                    var b = colorPrevious.B + deltaB * factor;
                    gr.FillRectangle(new SolidBrush(Color.FromArgb((int)r, (int)g, (int)b)), new Rectangle(x, 0, 1, rainballHeight));
                    x++;
                }
            }
            return bmp;
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
        public void AddColors()
        {
            baseColors.Add(Color.FromArgb(255, 0, 0));
            baseColors.Add(Color.Yellow);
            baseColors.Add(Color.Lime);
            baseColors.Add(Color.Aqua);
            baseColors.Add(Color.Blue);
            baseColors.Add(Color.FromArgb(255, 0, 255));
            baseColors.Add(Color.FromArgb(125, 0, 255));
            baseColors.Add(Color.FromArgb(255, 0, 0));
        }
        const int rainballWidth = 255;
        const int rainballHeight = 20;
        Color Base;
        Color Selected;
        Point point;
        Point point2;
        public Bitmap FillByPath(Bitmap path, Bitmap original)
        {
            var bmp = new Bitmap(path.Width, path.Height);
            for (var x = 0; x < path.Width; x++)
            {
                for (int y = 0; y < path.Height; y++)
                {
                    var color = path.GetPixel(x, y);
                    if (color.R < 5 && color.A > 200)
                    {
                        bmp.SetPixel(x, y, original.GetPixel(x, y));
                    }
                }
            }
            return bmp;
        }
        public Bitmap GetShape()
        {
            var bmp = new Bitmap(255, 20);
            var gr = Graphics.FromImage(bmp);
            gr.SmoothingMode = SmoothingMode.AntiAlias;
            gr.FillEllipse(new SolidBrush(Color.Black), new Rectangle(0, 0, rainballHeight, rainballHeight));
            gr.FillEllipse(new SolidBrush(Color.Black), new Rectangle(rainballWidth - rainballHeight * 4, 0, rainballHeight, rainballHeight));
            gr.FillRectangle(new SolidBrush(Color.Black), new Rectangle(rainballHeight / 2, 0, rainballWidth - rainballHeight * 4, rainballHeight));
            return bmp;
        }
        public void ResizeMe()
        {
            pictureBox3.Width = 255;
            pictureBox3.Height = this.Height;
            pictureBox2.Width = (int)(0.2 * (this.Width - 255));
            pictureBox2.Height = this.Height;
            pictureBox2.Left = 0;
            pictureBox2.Top = 0;
            pictureBox1.Width = (int)(0.8 * (this.Width - 255));
            pictureBox1.Left = pictureBox2.Width;
            pictureBox1.Top = 0;
            pictureBox1.Height = this.Height;
            pictureBox3.Left = pictureBox1.Left + pictureBox1.Width;
            pictureBox3.Top = 0;
            xCord = new ScrollBar(new Rectangle(0, pictureBox1.Height - 59, pictureBox1.Width - 20, 20), new SolidBrush(Color.LightGray), new SolidBrush(Color.DarkGray), 0.5);
            yCord = new HScrollBar(new Rectangle(pictureBox1.Width - 20, 0, 20, pictureBox1.Height), new SolidBrush(Color.LightGray), new SolidBrush(Color.DarkGray), 0.5);
            pictureBox1.Refresh();
            pictureBox2.Refresh();
            pictureBox3.Refresh();
        }
        public delegate void refresh();
        public static refresh RefreshMe;
        int ScrollDelta = 0;
        Shape SelectedShape;
        public void DrawShapes()
        {
            painting.Clear(Color.Transparent);
            foreach(var shape in shapes)
            {
                shape.Visualize(painting, new Rectangle(drawingSpace.X, drawingSpace.Y, pictureBox1.Width, pictureBox1.Height));
            }
        }
        public void ScaleShapes(double percent)
        {           
            foreach (var shape in shapes)
            {
                shape.Scale(percent);
            }
        }
        public void VisualCoolDown(Graphics gr)
        {
            if (cool)
            {
                var transparency = second / coolDown * 255;
                var mes = gr.MeasureString(scale.ToString(), new Font(new FontFamily("Comic Sans MS"), 20F));
                var rect = new Rectangle(pictureBox1.Width / 2 - (int)mes.Width / 2, pictureBox1.Height / (int)mes.Height / 2, (int)mes.Width, (int)mes.Height);
                gr.FillRectangle(new SolidBrush(Color.FromArgb((int)transparency, Color.Blue)), rect);
                gr.DrawString(scale.ToString(), new Font(new FontFamily("Comic Sans MS"), 20F), new SolidBrush(Color.FromArgb((int)transparency, Color.Black)), new Point(pictureBox1.Width / 2 - (int)mes.Width / 2, pictureBox1.Height / (int)mes.Height / 2));
            }
        }
        public void RefreshImages()
        {
            pictureBox2.Refresh();
        }
        Point lastclick;        
        public Form4()
        {
            InitializeComponent();
            Start();
            PlaceByX();
            PlaceByY();
            AddColors();
            ResizeWorkingSpace();
            pictureBox1.Refresh();
            ResizeMe();
            rainball = CreateRainBall();
            //rainball = FillByPath(GetShape(), rainball);
            picker = CreateColorPicker(Color.Red);
            RefreshMe += RefreshImages;
            pictureBox2.MouseWheel += new MouseEventHandler(PictureBox_MouseWheel);
            pictureBox1.MouseWheel += new MouseEventHandler(PictureBox2_MouseWheel);
            point2 = new Point(picker.Width / 2 - rainball.Width / 2 + rainballHeight / 2, 2152);
            painting = Graphics.FromImage(picture);
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            painting.SmoothingMode = SmoothingMode.AntiAlias;
            PlaceByX();
            PlaceByY();
            e.Graphics.DrawImage(selectedImage, drawingSpace);
            xCord.Visualize(e.Graphics);
            yCord.Visualize(e.Graphics);
            VisualCoolDown(e.Graphics);
            DrawShapes();
            e.Graphics.DrawImage(picture, 0, 0);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var click1 = xCord.Scroll(e.Location, e.Location);
                var click2 = yCord.Scroll(e.Location, e.Location);
                pictureBox1.Refresh();
                if (tool == tools.brush)
                {
                    var gr = Graphics.FromImage(selectedImage);
                    SelectedShape.Points.Add(e.Location);
                    pictureBox1.Refresh();
                }
            }
        }

        private void Form4_Resize(object sender, EventArgs e)
        {
            ResizeMe();
        }

        private void pictureBox3_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.DrawImage(picker, 0, 0);
            e.Graphics.DrawImage(rainball, picker.Width / 2 - rainball.Width / 2 + rainballHeight / 2, picker.Height + 3);
            e.Graphics.FillEllipse(new SolidBrush(Selected), new Rectangle(point, new Size(20, 20)));
            e.Graphics.DrawEllipse(new Pen(new SolidBrush(Color.White)), new Rectangle(point, new Size(20, 20)));
            e.Graphics.FillEllipse(new SolidBrush(Base), new Rectangle(new Point(point2.X, picker.Height + 3), new Size(20, 20)));
            e.Graphics.DrawEllipse(new Pen(new SolidBrush(Color.White), 3), new Rectangle(new Point(point2.X, picker.Height + 3), new Size(20, 20)));
        }

        private void pictureBox3_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (e.Y < picker.Height)
                {
                    if (e.Y > 0 && e.Y < picker.Height)
                    {
                        if (e.X > 0 && e.X < picker.Width)
                        {
                            Selected = picker.GetPixel(e.X, e.Y);
                            point = e.Location;
                        }
                    }
                }
                if (e.Y > picker.Height + 3)
                {
                    if (e.Y < picker.Height + 3 + 20)
                    {
                        if (e.X > point2.X && e.X < point2.X + 20)
                        {
                            var x = picker.Width / 2 - rainball.Width / 2 + rainballHeight / 2;
                            if (e.X - x >= 0 && e.Y - picker.Height < 20)
                            {
                                var color = rainball.GetPixel(e.X - x, e.Y - picker.Height);
                                if (color.A > 100)
                                {
                                    Base = color;
                                    picker = CreateColorPicker(Base);
                                    point2 = new Point(e.Location.X + point2.X - lastclick.X, e.Y);
                                }
                            }
                        }
                    }
                }
                lastclick = e.Location;
                pictureBox3.Refresh();
            }
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            var y = 0.0;
            foreach (var image in Form1.imagess)
            {
                var width = pictureBox2.Width;
                var height = (double)image.image.Height / (double)image.image.Width * width;
                y += height;
                e.Graphics.DrawImage(image.image, new Rectangle(0, (int)y + ScrollDelta, width, (int)height));
            }
        }
        private void PictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            ScrollDelta += e.Delta / 4;
            pictureBox2.Refresh();
        }
        private void PictureBox2_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                scale *= 0.9;
                ScaleShapes(0.9);
            }
            else
            {
                scale *= 1.1;
                ScaleShapes(1.1);
            }
            ResizeWorkingSpace();
            second = coolDown;
            cool = true;
            pictureBox1.Refresh();
        }
        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            var y = 0.0;
            foreach (var image in Form1.imagess)
            {
                var width = pictureBox2.Width;
                var height = (double)image.image.Height / (double)image.image.Width * width;
                y += height;
                if (e.X > 0 && e.X < width)
                {
                    if (e.Y > y && e.Y < height + y)
                    {
                        selectedImage = image.image;
                        ResizeMe();
                        ResizeWorkingSpace();
                        pictureBox1.Refresh();
                    }
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (cool)
            {
                second -= 0.01;
                pictureBox1.Refresh();
                if (second < 0)
                {
                    cool = false;
                    pictureBox1.Refresh();
                }
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            var shape = new Shape(new SolidBrush(Selected), 3);
            SelectedShape = shape;
            shapes.Add(SelectedShape);
        }

        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            lastclick = e.Location;
            if (e.Y < picker.Height)
            {
                if (e.Y > 0 && e.Y < picker.Height)
                {
                    if (e.X > 0 && e.X < picker.Width)
                    {
                        Selected = picker.GetPixel(e.X, e.Y);
                        point = e.Location;
                    }
                }
            }
            pictureBox3.Refresh();
        }
    }
}
