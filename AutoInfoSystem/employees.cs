using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace AutoInfoSystem
{
    public partial class employees : Form
    {
        Connection con2 = new Connection();
        MySqlConnection con;
        public employees()
        {
            con = con2.con1;
            InitializeComponent();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Main m = new Main();
            this.Hide();
            m.ShowDialog();
        }

        private void employees_Load(object sender, EventArgs e)
        {
            con.Open();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddEmployee ea = new AddEmployee();
            ea.ShowDialog();
        }
    }
}
