using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoInfoSystem
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            MakeButtonRounded(button1,20);
            MakeButtonRounded(button2, 20);
            MakeButtonRounded(button3, 20);
            MakeButtonRounded(button4, 20);
            MakeButtonRounded(button5, 20);
            MakeButtonRounded(button6, 20);
        }
        private void MakeButtonRounded(Button btn, int radius)
        {
            int arcSize = radius * 2;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, arcSize, arcSize, 180, 90);
            path.AddLine(radius, 0, btn.Width - radius, 0);
            path.AddArc(btn.Width - arcSize, 0, arcSize, arcSize, 270, 90);
            path.AddLine(btn.Width, radius, btn.Width, btn.Height - radius);
            path.AddArc(btn.Width - arcSize, btn.Height - arcSize, arcSize, arcSize, 0, 90);
            path.AddLine(btn.Width - radius, btn.Height, radius, btn.Height);
            path.AddArc(0, btn.Height - arcSize, arcSize, arcSize, 90, 90);
            path.AddLine(0, btn.Height - radius, 0, radius);
            path.CloseFigure();
            btn.Region = new Region(path);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Drugs d = new Drugs();
            this.Hide();
            d.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WareHouse h = new WareHouse();
            this.Hide();
            h.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Request r = new Request();
            this.Hide();
            r.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Users u = new Users();
            this.Hide();
            u.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            employees em = new employees();
            this.Hide();
            em.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Login l = new Login();
            this.Hide();
            l.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Directories d = new Directories(0);
            this.Hide();
            d.ShowDialog();
        }
    }
}
