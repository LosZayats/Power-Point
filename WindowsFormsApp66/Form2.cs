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
    public partial class Form2 : Form
    {
        public Form2(bool ButtVis)
        {
            InitializeComponent();
            button1.BackgroundImage = UserControl2.Gradient.MakeGradient(Color.Red, UserControl2.Direction.right, button1);
            button2.BackgroundImage = UserControl2.Gradient.MakeGradient(Color.Indigo, UserControl2.Direction.left, button2);
            button3.BackgroundImage = UserControl2.Gradient.MakeGradient(Color.BurlyWood, UserControl2.Direction.right, button3);
            if (ButtVis)
            {
                
            }
            else
            {
                button1.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
                this.MinimumSize = new Size(this.Width,255 + 72);
                this.MaximumSize = new Size(this.Width, 255 + 72);                
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void fontDialog1_Apply(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
            if(Form1.workTextBox != null)
            {
                Form1.workTextBox.Font = fontDialog1.Font;
                Form1.texts[Form1.richTextBoxes.IndexOf(Form1.workTextBox)].myFont = fontDialog1.Font;
            }            
            Form1.workTextBox.Size =new Size(new Point((int)Form1.PageNow.page.MeasureString(Form1.workTextBox.Text, fontDialog1.Font).Width+40, (int)Form1.PageNow.page.MeasureString(Form1.workTextBox.Text, fontDialog1.Font).Height+20));
            
        }

        private void fontDialog1_HelpRequest(object sender, EventArgs e)
        {
        }

        private void userControl11_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var ind = Form1.richTextBoxes.IndexOf(Form1.workTextBox);
            if(Form1.layers[ind]>=1)
            {
                var ind2 = Form1.layers.IndexOf(Form1.layers[ind] - 1);
                Form1.layers[ind2]++;
                Form1.layers[ind]--;
            }
        }
    }
}
