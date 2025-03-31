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
    public partial class MainSM : Form
    {
        public MainSM()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Drugs d = new Drugs();
            this.Hide();
            d.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WareHouse w = new WareHouse();
            this.Hide();
            w.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Request r = new Request();
            this.Hide();
            r.ShowDialog();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Login l = new Login();
            this.Hide();
            l.ShowDialog();
        }

        private void MainSM_Load(object sender, EventArgs e)
        {

        }
    }
}
