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
        interface IClickAble
        {
            void VisalImage(Graphics gr);
            void VisalText(Graphics gr);
            bool ClickOnMe(Point location);
        }
        class MenuButton : IClickAble
        {
            Bitmap myImage
            {
                get; set;
            }
            public bool Selection;
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
            public string Name
            {
                get; set;
            }
            public int Height
            {
                get; set;
            }
            public MenuButton(Bitmap image, string name, int height)
            {
                myImage = image;
                Name = name;
                this.Height = height;
                this.Width = (int)((double)image.Width / (double)image.Height * height);
            }
            public void VisalImage(Graphics gr)
            {
                gr.SmoothingMode = SmoothingMode.Default;
                gr.DrawImage(myImage, new Rectangle(X, Y, Width, Height));
                if (Selection)
                {
                    gr.FillRectangle(new SolidBrush(Color.FromArgb(125, 200, 255, 200)), new Rectangle(X, Y, Width, Height));
                }
                gr.SmoothingMode = SmoothingMode.AntiAlias;
            }
            public void VisalText(Graphics gr)
            {

            }
            public bool ClickOnMe(Point location)
            {
                var clicked = false;
                if (location.X > this.X && location.X < this.X + this.Width)
                {
                    if (location.Y > this.Y && location.Y < this.Y + this.Height)
                    {
                        clicked = true;
                    }
                }
                return clicked;
            }
        }
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
                for (int i = 0; i < Points.Count; i++)
                {
                    var point = Points[i];
                    Points[i] = new Point((int)(point.X * percent), (int)(point.Y * percent));
                }
            }
            public void Visualize(Graphics gr, Rectangle bounds)
            {
                Graphics = gr;
                for (int index = 0; index < Points.Count - 1; index++)
                {
                    var point1 = Points[index];
                    var point2 = Points[index + 1];
                    if (point1.X + bounds.X > 0 && point1.X + bounds.X < bounds.Width && point1.Y + bounds.Y > 0 && point1.Y + bounds.Y < bounds.Height)
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
        class Effector
        {
        }
        class RectangleD
        {
            public double X;
            public double Y;
            public double Width;
            public double Height;
            public RectangleD(int x,int y, int width,int height)
            {
                Y = y;
                Width = width;
                Height = height;
                X = x;
            }
            public RectangleD(double x, double y, double width, double height)
            {
                Y = y;
                Width = width;
                Height = height;
                X = x;
            }
            public static implicit operator Rectangle(RectangleD d)
            {
                return new Rectangle((int)d.X, (int)d.Y, (int)d.Width, (int)d.Height);
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
        public Color GetColor(Point p1)
        {
            var x = (p1.X - drawingSpace.X) / scale;
            var color = Color.Transparent;
            var y = (p1.Y - drawingSpace.Y) / scale;
            if (x > 0 && x < selectedImage.Width)
            {
                if (y > 0 && y < selectedImage.Height)
                {
                    color = selectedImage.GetPixel((int)x, (int)y);
                }
            }
            return color;
        }
        enum tools
        {
            brush, selection, colorpick, transparent, unselection, none
        }
        Bitmap picture = new Bitmap(SystemInformation.PrimaryMonitorSize.Width, SystemInformation.PrimaryMonitorSize.Height);
        Graphics painting;
        List<Shape> shapes = new List<Shape>();
        tools t = tools.selection;
        tools tool
        {
            get
            {
                return t;
            }
            set
            {
                t = value;
                if (t == tools.unselection)
                {
                    selectedRectangle = null;
                    startSelection = null;
                }
            }
        }
        double lastYValue;
        public void PlaceByX()
        {
            var xLast = drawingSpace.X;
            var deltaValue = xCord.GetValue();
            drawingSpace.X = (int)(drawingSpace.Width * (deltaValue - 0.5)) + pictureBox1.Width / 2 - drawingSpace.Width / 2;

            lastXValue = xCord.GetValue();
            Replace(drawingSpace.X - xLast, 0);
        }
        public void PlaceByY()
        {
            var yLast = drawingSpace.Y;
            var deltaValue = yCord.GetValue();
            drawingSpace.Y = (int)(drawingSpace.Height * (deltaValue - 0.5)) + pictureBox1.Height / 2 - drawingSpace.Height / 2;

            lastYValue = yCord.GetValue();
            Replace(0, drawingSpace.Y - yLast);
        }
        public void ResizeWorkingSpace()
        {
            drawingSpace.Height = (int)(selectedImage.Height * scale);
            drawingSpace.Width = (int)(selectedImage.Width * scale);
        }
        const int buttonWidth = 50;
        public void Start()
        {
            xCord = new ScrollBar(new Rectangle(0, pictureBox1.Height - 20, pictureBox1.Width - 20, 20), new SolidBrush(Color.LightGray), new SolidBrush(Color.DarkGray), 0.5);
            yCord = new HScrollBar(new Rectangle(pictureBox1.Width - 20, 0, 20, pictureBox1.Height), new SolidBrush(Color.LightGray), new SolidBrush(Color.DarkGray), 0.5);
            drawingSpace = new Rectangle(-500, 100, 1000, 200);
            lastXValue = xCord.GetValue();
            lastYValue = yCord.GetValue();
        }
        public void CreateMenu()
        {
            colorpick = new MenuButton(Properties.Resources.color_dropper, "lol", buttonWidth);
            colorpick.X = 0;
            colorpick.Y = picker.Height + rainball.Height + 3;
            brush = new MenuButton(Properties.Resources.paint_brush, "lol", buttonWidth);
            brush.Y = picker.Height + rainball.Height + 3;
            brush.X = brush.Width;
            selection = new MenuButton(Properties.Resources.selection, "lol", buttonWidth);
            selection.X = brush.X + selection.Width;
            selection.Y = picker.Height + rainball.Height + 3;
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
                var DeltaG = g / 255.0;
                var DeltaB = b / 255.0;
                var DeltaR = r / 255.0;
                for (double delta = 0; delta < 255; delta++)
                {
                    var r2 = r - (DeltaR * delta);
                    var g2 = g - (DeltaG * delta);
                    var b2 = b - (DeltaB * delta);
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
            var widthPerColor = (double)rainballWidth / (baseColors.Count - 1);
            var x = 0;
            for (int index = 0; index < baseColors.Count - 1; index++)
            {
                var colorPrevious = baseColors[index];
                var colorNow = baseColors[index + 1];
                var deltaR = (colorNow.R - colorPrevious.R) / widthPerColor;
                var deltaG = (colorNow.G - colorPrevious.G) / widthPerColor;
                var deltaB = (colorNow.B - colorPrevious.B) / widthPerColor;
                for (double factor = 0; factor < widthPerColor; factor += widthPerColor / Math.Floor(widthPerColor))
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
            baseColors.Add(Color.Red);
            baseColors.Add(Color.Yellow);
            baseColors.Add(Color.Lime);
            baseColors.Add(Color.Cyan);
            baseColors.Add(Color.Blue);
            //baseColors.Add(Color.FromArgb(125, 0, 255));
            baseColors.Add(Color.Magenta);
            baseColors.Add(Color.Red);
        }
        const int rainballWidth = 255;
        const int rainballHeight = 20;
        Color Base;
        Color Selected;
        Point point;
        Point point2;
        int Thickness;
        public void DrawSelection(Graphics gr)
        {
            var pen = new Pen(new SolidBrush(Color.MediumAquamarine), 3);
            pen.DashStyle = DashStyle.Dash;
            var rect = new Rectangle(0, 0, 0, 0);
            if (selectedRectangle!=null)
            {
                rect = selectedRectangle;
            }
            gr.DrawRectangle(pen, rect);
        }
        public void Brush(MouseEventArgs e)
        {
            if (tool == tools.brush)
            {
                var pensize = 4;
                var gr = Graphics.FromImage(selectedImage);
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                var point1 = new Point((int)((e.X - drawingSpace.X) / scale), (int)((e.Y - drawingSpace.Y) / scale));
                var point2 = new Point((int)((lastclick2.X - drawingSpace.X) / scale), (int)((lastclick2.Y - drawingSpace.Y) / scale));
                var pen = new Pen(new SolidBrush(Selected), pensize);
                gr.FillEllipse(new SolidBrush(Selected), new Rectangle(point1.X - pensize / 2, point1.Y - pensize / 2, pensize, pensize));
                gr.FillEllipse(new SolidBrush(Selected), new Rectangle(point2.X - pensize / 2, point2.Y - pensize / 2, pensize, pensize));
                gr.DrawLine(pen, point1, point2);
                pictureBox1.Refresh();
            }
        }
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
            ResizeSelection();
            pictureBox1.Refresh();
            pictureBox2.Refresh();
            pictureBox3.Refresh();
        }
        public delegate void refresh();
        public static refresh RefreshMe;
        int ScrollDelta = 0;
        public void GetPosition()
        {
            var R = Convert.ToDouble(Selected.R);
            var G = Convert.ToDouble(Selected.G);
            var B = Convert.ToDouble(Selected.B);
            var h = GetH(R, G, B);
            var image = Properties.Resources.hue_small;
            var x = h / 360.0 * (image.Width);
            Base = image.GetPixel((int)x, 0);
            point2.X = (int)(rainball.Width * h / 360.0);
            picker = CreateColorPicker(Base);
            for (int x1 = 0; x1 < picker.Width; x1++)
            {
                for (int y = 0; y < picker.Height; y++)
                {
                    var color = picker.GetPixel(x1, y);
                    if (color.R > Selected.R - 3 && color.R < Selected.R + 3)
                    {
                        if (color.G > Selected.G - 3 && color.G < Selected.G + 3)
                        {
                            if (color.B > Selected.B - 3 && color.B < Selected.B + 3)
                            {
                                point.X = x1;
                                point.Y = y;
                                pictureBox3.Refresh();
                                return;
                            }
                        }
                    }
                }
            }
        }
        public Point Rotate(Point center, Point rotatedPoint, double degree)
        {
            var Angle = (Math.PI * degree) / 180;
            var xa = rotatedPoint.X - center.X;
            if (xa == 0)
            {
                xa = 1;
            }
            var sign = Math.Sign(xa);
            var ya = (rotatedPoint.Y - center.Y) * sign;
            if (ya == 0)
            {
                ya = 1;
            }
            var cathet1 = ya;
            var cathet2 = xa;
            var R = Math.Sqrt(cathet1 * cathet1 + cathet2 * cathet2);
            var arc_sin = Math.Asin((ya) / R);
            var Angle1 = Math.Abs(Math.Asin((ya) / R));
            var Angle2 = (-Math.Sign(arc_sin)) * Angle1 - Angle;
            var X = Math.Cos(Angle2) * R;
            var Y = Math.Sin(Angle2) * R;
            if (sign == -1)
            {
                return new Point((int)-X + center.X, center.Y + (int)Y);
            }
            else
            {
                return new Point((int)X + center.X, center.Y - (int)Y);
            }
        }
        public double GetH(double r, double g, double b)
        {
            r = r / 255.0;
            g = g / 255.0;
            b = b / 255.0;

            // h, s, v = hue, saturation, value 
            double cmax = Math.Max(r, Math.Max(g, b)); // maximum of r, g, b 
            double cmin = Math.Min(r, Math.Min(g, b)); // minimum of r, g, b 
            double diff = cmax - cmin; // diff of cmax and cmin. 
            double h = -1, s = -1;

            // if cmax and cmax are equal then h = 0 
            if (cmax == cmin)
                h = 0;

            // if cmax equal r then compute h 
            else if (cmax == r)
                h = (60 * ((g - b) / diff) + 360) % 360;

            // if cmax equal g then compute h 
            else if (cmax == g)
                h = (60 * ((b - r) / diff) + 120) % 360;

            // if cmax equal b then compute h 
            else if (cmax == b)
                h = (60 * ((r - g) / diff) + 240) % 360;
            return h;
        }
        public void Select(MouseEventArgs e)
        {
            var point = new Point();
            if (startSelection.HasValue)
            {
                point = startSelection.Value;
            }
            var x = 0;
            var y = 0;
            var width = e.X - point.X;
            var height = e.Y - point.Y;
            if (width < 0)
            {
                width = Math.Abs(width);
                x = e.X;
            }
            else
            {
                x = point.X;
            }
            if (height < 0)
            {
                height = Math.Abs(height);
                y = e.Y;
            }
            else
            {
                y = point.Y;
            }
            selectedRectangle = new RectangleD(x, y, width, height);
        }
        Shape SelectedShape;
        public void ResizeSelection()
        {
            if (selectedRectangle != null)
            {
                var rect = selectedRectangle;
                var width1 = selectedImage.Width;
                var height1 = selectedImage.Height;
                var width2 = drawingSpace.Width;
                var height2 = drawingSpace.Height;
                var scale1 = (double)width2 / width1;
                var scale2 = (double)height2 / height1;
                var x1 = ((rect.X - drawingSpace.X) / scale1);
                var y1 = ((rect.Y - drawingSpace.Y) / scale2);
                var width3 = (rect.Width / scale1) / selectedImage.Width;
                var height3 = (rect.Height / scale2) / selectedImage.Height;
                ResizeWorkingSpace();
                PlaceByX();
                PlaceByY();
                width2 = drawingSpace.Width;
                height2 = drawingSpace.Height;
                rect.X = (x1 * (double)width2 / width1 + drawingSpace.X);
                rect.Y = (y1 * (double)height2 / height1 + drawingSpace.Y);
                selectedRectangle = rect;
                rect.Width = width3 * drawingSpace.Width;
                rect.Height = height3 * drawingSpace.Height;
            }
        }
        public void Delete()
        {
            if (tool == tools.selection)
            {
                var rect = new Rectangle(0, 0, 0, 0);
                if (selectedRectangle!=null)
                {
                    rect = selectedRectangle;
                }
                for (int x = rect.X; x < rect.X + rect.Width; x++)
                {
                    for (int y = rect.Y; y < rect.Y + rect.Height; y++)
                    {
                        var x1 = (int)((x - drawingSpace.X) / scale);
                        var y1 = (int)((y - drawingSpace.Y) / scale);
                        if (x1 > 0 && x1 < selectedImage.Width)
                        {
                            if (y1 > 0 && y1 < selectedImage.Height)
                            {
                                selectedImage.SetPixel(x1, y1, Color.Transparent);
                            }
                        }
                    }
                }
            }
        }
        public void DrawShapes()
        {
            painting.Clear(Color.Transparent);
            foreach (var shape in shapes)
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
        public void Replace(int deltaX, int deltaY)
        {
            if (deltaX != 0 || deltaY != 0)
            {
                foreach (var shape in shapes)
                {
                    for (int index = 0; index < shape.Points.Count; index++)
                    {
                        var point = shape.Points[index];
                        shape.Points[index] = new Point(point.X + deltaX, point.Y + deltaY);
                    }
                }
            }
        }
        public void RefreshImages()
        {
            pictureBox2.Refresh();
        }
        public void FillTransparecy(Graphics gr)
        {
            var color1 = Color.DimGray;
            var color2 = Color.LightGray;
            var colorNow = color1;
            bool color = true;
            var count = (double)pictureBox1.Width / (a);
            var count2 = (double)pictureBox1.Height / (a);
            if (count > Math.Floor(count))
            {
                count = Math.Floor(count) + 1;
            }
            if (count2 > Math.Floor(count2))
            {
                count2 = Math.Floor(count2) + 1;
            }
            if (count % 2 == 0)
            {
                count++;
            }
            if (count2 % 2 == 0)
            {
                count2++;
            }
            for (int i = 0; i < count; i++)
            {
                for (int y = 0; y < count2; y++)
                {
                    gr.FillRectangle(new SolidBrush(colorNow), new Rectangle((int)(i * a), (int)(y * a), (int)(a), (int)(a)));
                    if (color)
                    {
                        color = false;
                        colorNow = color2;
                    }
                    else
                    {
                        color = true;
                        colorNow = color1;
                    }
                }
            }
        }
        Point lastclick2;
        const int circleDiametr = 20;
        const int border = 3;
        Point lastclick;
        Point? startSelection;
        RectangleD selectedRectangle = null;
        const int a = 50;
        MenuButton selection;
        MenuButton brush;
        MenuButton colorpick;
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
            picker = CreateColorPicker(Color.Red);
            RefreshMe += RefreshImages;
            pictureBox2.MouseWheel += new MouseEventHandler(PictureBox_MouseWheel);
            pictureBox1.MouseWheel += new MouseEventHandler(PictureBox2_MouseWheel);
            point2 = new Point(picker.Width / 2 - rainball.Width / 2 + rainballHeight / 2, 2152);
            painting = Graphics.FromImage(picture);
            CreateMenu();
            Selected = Color.Black;
            GetPosition();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            FillTransparecy(e.Graphics);
            painting.SmoothingMode = SmoothingMode.AntiAlias;
            PlaceByX();
            PlaceByY();
            e.Graphics.FillRectangle(new SolidBrush(Color.White), drawingSpace);
            e.Graphics.DrawImage(selectedImage, drawingSpace);
            xCord.Visualize(e.Graphics);
            yCord.Visualize(e.Graphics);
            VisualCoolDown(e.Graphics);
            DrawShapes();
            if (tool == tools.selection)
            {
                DrawSelection(e.Graphics);
            }
            e.Graphics.DrawImage(picture, 0, 0);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var click1 = xCord.Scroll(e.Location, e.Location);
                var click2 = yCord.Scroll(e.Location, e.Location);
                //tools
                if (e.Y < pictureBox1.Height - 60)
                {
                    if (e.X < pictureBox1.Width - 20)
                    {
                        Brush(e);
                        if (tool == tools.selection)
                        {
                            Select(e);
                        }
                    }
                }
                if(click1||click2)
                {
                    ResizeSelection();
                }
                lastclick2 = e.Location;
                pictureBox1.Refresh();
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
            e.Graphics.DrawImage(rainball, 0, picker.Height + border);
            e.Graphics.FillEllipse(new SolidBrush(Selected), new Rectangle(new Point(point.X - circleDiametr / 2, point.Y - circleDiametr / 2), new Size(circleDiametr, circleDiametr)));
            e.Graphics.DrawEllipse(new Pen(new SolidBrush(Color.White), 3), new Rectangle(new Point(point.X - circleDiametr / 2, point.Y - circleDiametr / 2), new Size(circleDiametr, circleDiametr)));
            e.Graphics.FillEllipse(new SolidBrush(Base), new Rectangle(new Point(point2.X, picker.Height + border), new Size(circleDiametr, circleDiametr)));
            e.Graphics.DrawEllipse(new Pen(new SolidBrush(Color.White), 3), new Rectangle(new Point(point2.X, picker.Height + border), new Size(circleDiametr, circleDiametr)));
            brush.VisalImage(e.Graphics);
            selection.VisalImage(e.Graphics);
            colorpick.VisalImage(e.Graphics);
            //e.Graphics.DrawImage(Properties.Resources.select_none, 0, 0);
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
                            point = e.Location;
                            Selected = picker.GetPixel(point.X, point.Y);
                        }
                    }
                }
                if (e.Location.X + point2.X - lastclick.X > 0 && e.Location.X + point2.X - lastclick.X + circleDiametr < pictureBox3.Width)
                {
                    if (e.Y > picker.Height + border && e.Y - picker.Height < circleDiametr)
                    {
                        var color = rainball.GetPixel(e.Location.X + point2.X - lastclick.X, e.Y - picker.Height);
                        if (color.A > 100)
                        {
                            Base = color;
                            picker = CreateColorPicker(Base);
                            point2 = new Point(e.Location.X + point2.X - lastclick.X, e.Y);
                            Selected = picker.GetPixel(point.X, point.Y);
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
                scale *= 0.5;
                ScaleShapes(0.5);
            }
            else
            {
                scale *= 1.5;
                ScaleShapes(1.5);
            }
            if(selectedRectangle!=null)
            {
                ResizeSelection();
            }
            else
            {
                ResizeWorkingSpace();
            }
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
            lastclick2 = e.Location;
            if (tool == tools.selection)
            {
                startSelection = e.Location;
            }
            if (tool == tools.colorpick)
            {
                Selected = GetColor(e.Location);
                GetPosition();
                pictureBox3.Refresh();
            }
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
            var sel = selection.ClickOnMe(e.Location);
            var bru = brush.ClickOnMe(e.Location);
            var pick = colorpick.ClickOnMe(e.Location);
            if (sel)
            {
                if (tool == tools.selection)
                {
                    tool = tools.none;
                    brush.Selection = false;
                    colorpick.Selection = false;
                    selection.Selection = false;
                }
                else
                {
                    tool = tools.selection;
                    brush.Selection = false;
                    colorpick.Selection = false;
                    selection.Selection = false;
                    selection.Selection = true;
                }
            }
            if (bru)
            {
                if (tool == tools.brush)
                {
                    tool = tools.none;
                    brush.Selection = false;
                    colorpick.Selection = false;
                    selection.Selection = false;
                }
                else
                {
                    tool = tools.brush;
                    brush.Selection = false;
                    colorpick.Selection = false;
                    selection.Selection = false;
                    brush.Selection = true;
                }
            }
            if (pick)
            {
                if (tool == tools.colorpick)
                {
                    tool = tools.none;
                    brush.Selection = false;
                    colorpick.Selection = false;
                    selection.Selection = false;
                }
                else
                {
                    tool = tools.colorpick;
                    brush.Selection = false;
                    colorpick.Selection = false;
                    selection.Selection = false;
                    colorpick.Selection = true;
                }
            }
            pictureBox3.Refresh();
        }

        private void Form4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                Delete();
                pictureBox1.Refresh();
            }
        }
    }
}
