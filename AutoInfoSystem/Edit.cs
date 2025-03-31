using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoInfoSystem
{
    public partial class Edit : Form
    {
        public Edit(string name,string category,string manufacturer, string realeseForm, int shellife)
        {
            InitializeComponent();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Drugs d = new Drugs();
            this.Hide();
            d.ShowDialog();
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !Regex.IsMatch(e.KeyChar.ToString(), @"^[а-яА-ЯеЁ]+$"))
            {
                e.Handled = true;
            }
        }

        private void Edit_Load(object sender, EventArgs e)
        {

        }
    }
}
