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
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
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
            dataGridView1.Columns[2].Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Вы точно хотите удалить пользователя?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    int id = int.Parse(dataGridView1.SelectedCells[0].Value.ToString());
                    using (MySqlCommand com = new MySqlCommand("DELETE FROM user where Id = @id", con))
                    {
                        com.Parameters.AddWithValue("@id", id);
                        com.ExecuteNonQuery();
                        MessageBox.Show("Пользователь успешно удалён!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        LoadData();
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void LoadData()
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
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

        private void button2_Click(object sender, EventArgs e)
        {
            int id = int.Parse(dataGridView1.SelectedCells[0].Value.ToString());
            string login = dataGridView1.SelectedCells[1].Value.ToString();
            string role = dataGridView1.SelectedCells[3].Value.ToString();
            EditUser u = new EditUser(id, login, role);
            this.Hide();
            u.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddUser u = new AddUser();
            this.Hide();
            u.ShowDialog();
        }
    }
}
