using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoInfoSystem.Tables
{
    public partial class AllDataEmployee : Form
    {
        public AllDataEmployee(string name,string lastname,string patronyc,string prof,string userr)
        {
            InitializeComponent();
        }

        private void AllDataEmployee_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            employees em = new employees();
            this.Hide();
            em.ShowDialog();
        }
    }
}
