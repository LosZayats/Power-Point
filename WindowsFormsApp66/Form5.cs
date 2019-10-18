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
    public partial class Form5 : Form
    {
        int row = 0;
        int coulumn = 0;
        public Form5()
        {
            InitializeComponent();
            this.BackgroundImage = UserControl2.Gradient.MakeGradient(Color.Indigo, UserControl2.Direction.right, this);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            coulumn = (int)numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            row = (int)numericUpDown2.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1.Table table = new Form1.Table(coulumn, row, Form1.PageNumForOut);            
        }
    }
}
