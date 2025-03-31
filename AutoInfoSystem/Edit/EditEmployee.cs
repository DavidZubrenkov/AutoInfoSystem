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
namespace AutoInfoSystem.Edit
{
    public partial class EditEmployee : Form
    {
        private int id1;
        private string name1;
        private string lastname1;
        private string patronyc1;
        private string prof1;
        private string userid1;
        public EditEmployee(int id, string name, string lastname,string patronyc,string prof,string userid)
        {
            id1 = id;
            name1 = name;
            lastname1 = lastname;
            patronyc1 = patronyc;
            prof1 = prof;
            userid1 = userid;
            con = con2.con1;
            InitializeComponent();
        }
        Connection con2 = new Connection();
        MySqlConnection con;

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            employees em = new employees();
            this.Hide();
            em.ShowDialog();
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
            MySqlCommand com = new MySqlCommand("Select login from user", con);
            MySqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                comboBox3.Items.Add(reader["login"].ToString()); // добавляем логин в combobox
            }
            reader.Close();
        }
        private void EditEmployee_Load(object sender, EventArgs e)
        {
            if(con.State == ConnectionState.Open)
            {
                con.Close();
            }
            textBox5.Text = name1;
            textBox4.Text = lastname1;
            textBox1.Text = patronyc1;
            con.Open();
            loadLogin();
            loadProfession();
            comboBox1.SelectedItem = prof1;
            comboBox3.SelectedItem = userid1;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Все поля заполнены правильно?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question); // подтверждаем добавление

                if (result == DialogResult.Yes)
                {
                    if (string.IsNullOrWhiteSpace(textBox5.Text) || string.IsNullOrWhiteSpace(textBox4.Text))
                    {
                        MessageBox.Show("Заполните все обязательные поля!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    string query = $@"Update employee Set 
                Name = @Name, LastName = @LastName, Patronyc = @Patronyc, ProfessionId = (SELECT Id FROM profession WHERE Name = @profname), UserId = (SELECT Id FROM user WHERE Login = @login) where id = {id1};";
                    using (MySqlCommand com = new MySqlCommand(query, con))
                    {
                        com.Parameters.AddWithValue("@Name", textBox5.Text.Trim());
                        com.Parameters.AddWithValue("@LastName", textBox4.Text.Trim());
                        com.Parameters.AddWithValue("@Patronyc", textBox1.Text.Trim());
                        com.Parameters.AddWithValue("@profname", comboBox1.SelectedItem.ToString());
                        if (comboBox3.SelectedItem == null)
                        {
                            com.Parameters.AddWithValue("@login", null);
                        }
                        else
                        {
                            com.Parameters.AddWithValue("@login", comboBox3.SelectedItem.ToString());
                        }
                        com.ExecuteNonQuery();
                        MessageBox.Show("Сотрудник успешно отредактирован", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        employees em = new employees();
                        this.Hide();
                        em.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
