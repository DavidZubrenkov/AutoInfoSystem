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
    public partial class Users : Form
    {
        Connection con2 = new Connection();
        MySqlConnection con;
        public Users()
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

        private void Users_Load(object sender, EventArgs e)
        {
            con.Open();
            string query = @"SELECT 
                user.Id,
                user.Login,
                user.Password,
                role.Name 
                From user
                Join role on user.RoleId = role.Id";
            MySqlDataAdapter ad = new MySqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            dataGridView1.DataSource = dt;
        }
    }
}
