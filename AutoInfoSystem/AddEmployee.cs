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

        private void button5_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Заполните все обязательные поля!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string query = @"INSERT INTO employee (Name, LastName, Patronyc, ProfessionId, UserId) 
                 VALUES (@Name, @LastName, @Patronyc, 
                         (SELECT Id FROM profession WHERE Name = @profname), 
                         (SELECT Id FROM user WHERE Login = @login))";
            using (MySqlCommand com = new MySqlCommand(query,con))
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
            }
        }
    }
}
 