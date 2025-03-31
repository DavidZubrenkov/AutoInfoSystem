using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace AutoInfoSystem
{
    public partial class AddUser : Form
    {
        public AddUser()
        {
            con = con2.con1;
            InitializeComponent();
        }
        Connection con2 = new Connection();
        MySqlConnection con;
        private void AddUser_Load(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            comboBox2.SelectedIndex = 0;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Все поля заполненны правильно?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            try
            {
                if (result == DialogResult.Yes)
                {

                    if (string.IsNullOrWhiteSpace(textBox4.Text) || string.IsNullOrWhiteSpace(textBox5.Text))
                    {
                        MessageBox.Show("Заполните все поля!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    try
                    {
                        string query = @"INSERT INTO user(login,password,RoleId) Values (@log,@pas,(Select id from role where name = @nam))";
                        using (MySqlCommand com = new MySqlCommand(query, con))
                        {
                            com.Parameters.AddWithValue("@log", textBox5.Text.Trim());
                            com.Parameters.AddWithValue("@pas", HashPassword(textBox4.Text.Trim()));
                            com.Parameters.AddWithValue("nam", comboBox2.SelectedItem.ToString());
                            com.ExecuteNonQuery();
                            MessageBox.Show("Пользователь успешно добавлен!", "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            Users u = new Users();
                            this.Hide();
                            u.ShowDialog();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex}", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        static string HashPassword(string password) // шифровка пароля
        {
            using (SHA256 sh256 = SHA256.Create())
            {
                byte[] bytes = sh256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Users u = new Users();
            this.Hide();
            u.ShowDialog();
        }
    }
}
