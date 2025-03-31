using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoInfoSystem
{
    public partial class MainGM : Form
    {
        public MainGM()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Request r = new Request();
            this.Hide();
            r.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WareHouse w = new WareHouse();
            this.Hide();
            w.ShowDialog();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Login l = new Login();
            this.Hide();
            l.ShowDialog();
        }

        private void MainGM_Load(object sender, EventArgs e)
        {

        }
    }
}
