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
    public partial class WareHouse : Form
    {
        public WareHouse()
        {
            InitializeComponent();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (User.role == 1)
            {
                Main m = new Main();
                this.Hide();
                m.ShowDialog();
            }
            else if (User.role == 2)
            {
                MainGM m = new MainGM();
                this.Hide();
                m.ShowDialog();
            }
            else if (User.role == 3)
            {
                MainSM m = new MainSM();
                this.Hide();
                m.ShowDialog();
            }
        }
    }
}
