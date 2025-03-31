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
    public partial class EditUser : Form
    {
        private int id1;
        private string login1;
        private string role1;
        Connection con2 = new Connection();
        MySqlConnection con = new MySqlConnection();
        public EditUser(int id, string login, string role)
        {
            con = con2.con1;
            id1 = id;
            login1 = login;
            role1 = role;
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Все поля заполненны правильно?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (string.IsNullOrWhiteSpace(textBox4.Text) || string.IsNullOrWhiteSpace(textBox5.Text))
                    {
                        MessageBox.Show("Заполните все поля!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    try
                    {
                        string query = $@"Update user SET login = @log, password = @pass, Roleid = (Select id from role where Name = @nam) where id = {id1}";
                        using (MySqlCommand com = new MySqlCommand(query, con))
                        {
                            com.Parameters.AddWithValue("@log", textBox5.Text);
                            com.Parameters.AddWithValue("@nam", comboBox2.SelectedItem.ToString());
                            com.Parameters.AddWithValue("@pass", HashPassword(textBox4.Text));
                            com.ExecuteNonQuery();
                            MessageBox.Show("Данные о пользователе успешно обновленны", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            Users u = new Users();
                            this.Hide();
                            u.ShowDialog();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Произошла ошибка!\n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditUser_Load(object sender, EventArgs e)
        {
            if(con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            comboBox2.SelectedIndex = 0;
            comboBox2.SelectedItem = role1;
            textBox5.Text = login1;
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
