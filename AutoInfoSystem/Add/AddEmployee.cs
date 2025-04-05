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
    public partial class AddEmployee : Form
    {
        Connection con2 = new Connection();
        MySqlConnection con;
        public AddEmployee()
        {
            con = con2.con1;
            InitializeComponent();
        }

        private void AddEmployee_Load(object sender, EventArgs e)
        {
            if(con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            loadProfession();
            loadLogin();
        }
        private void loadProfession()
        {
            MySqlCommand com = new MySqlCommand("Select name from profession", con);
            MySqlDataReader reader = com.ExecuteReader();

            while (reader.Read())
            {
                comboBox1.Items.Add(reader["name"].ToString()); // Добавляем профессию в comboBox
            }
            comboBox1.SelectedIndex = 0;
            reader.Close();
        }
        private void loadLogin()
        {
            MySqlCommand com = new MySqlCommand("Select login from user",con);
            MySqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                comboBox3.Items.Add(reader["login"].ToString()); // добавляем логин в combobox
            }
            reader.Close();
        }
        // добавляем нового сотрудника
        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Все поля заполнены правильно?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question); // подтверждаем добавление
            try
            {
                if (result == DialogResult.Yes)
                {
                    if (string.IsNullOrWhiteSpace(textBox5.Text) || string.IsNullOrWhiteSpace(textBox4.Text) || !maskedTextBox1.MaskFull)
                    {
                        MessageBox.Show("Заполните все обязательные поля!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    int a = maskedTextBox1.TextLength;
                    string query = @"INSERT INTO employee (Name, LastName, Patronyc, ProfessionId, UserId,Telephone) 
                 VALUES (@Name, @LastName, @Patronyc, 
                         (SELECT Id FROM profession WHERE Name = @profname), 
                         (SELECT Id FROM user WHERE Login = @login),
                         @Telephone)";
                    using (MySqlCommand com = new MySqlCommand(query, con))
                    {
                        com.Parameters.AddWithValue("@Name", textBox5.Text.Trim());
                        com.Parameters.AddWithValue("@LastName", textBox4.Text.Trim());
                        com.Parameters.AddWithValue("@Patronyc", textBox1.Text.Trim());
                        com.Parameters.AddWithValue("@profname", comboBox1.SelectedItem.ToString());
                        com.Parameters.AddWithValue("@Telephone", maskedTextBox1.Text);
                        if (comboBox3.SelectedItem == null)
                        {
                            com.Parameters.AddWithValue("@login", null);
                        }
                        else
                        {
                            com.Parameters.AddWithValue("@login", comboBox3.SelectedItem.ToString());
                        }
                        com.ExecuteNonQuery();
                        MessageBox.Show("Сотрудник успешно добавлени", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        employees em = new employees();
                        this.Hide();
                        em.ShowDialog();
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

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            employees em = new employees();
            this.Hide();
            em.ShowDialog();
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!IsValidChar(e.KeyChar, allowHyphen: false))
            {
                e.Handled = true; // Запрещаем ввод
            }
        }
        private bool IsValidChar(char c, bool allowHyphen)
        {
            return (c >= 'А' && c <= 'Я') || (c >= 'а' && c <= 'я') || c == 'ё' || c == 'Ё' ||
                   (allowHyphen && c == '-') || c == '\b'; // Разрешаем backspace
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!IsValidChar(e.KeyChar, allowHyphen: true))
            {
                e.Handled = true; // Запрещаем ввод
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!IsValidChar(e.KeyChar, allowHyphen: false))
            {
                e.Handled = true; // Запрещаем ввод
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (!string.IsNullOrEmpty(tb.Text))
            {
                tb.Text = char.ToUpper(tb.Text[0]) + tb.Text.Substring(1);
                tb.SelectionStart = tb.Text.Length; // Ставим курсор в конец
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (!string.IsNullOrEmpty(tb.Text))
            {
                tb.Text = char.ToUpper(tb.Text[0]) + tb.Text.Substring(1);
                tb.SelectionStart = tb.Text.Length; // Ставим курсор в конец
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (!string.IsNullOrEmpty(tb.Text))
            {
                tb.Text = char.ToUpper(tb.Text[0]) + tb.Text.Substring(1);
                tb.SelectionStart = tb.Text.Length; // Ставим курсор в конец
            }
        }
    }
}
 