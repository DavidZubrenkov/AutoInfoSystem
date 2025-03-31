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
    public partial class addDepartment : Form
    {
        public addDepartment()
        {
            InitializeComponent();
        }
        Connection con2 = new Connection();
        private void addDepartment_Load(object sender, EventArgs e)
        {
            MySqlConnection con = con2.con1;
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Поле не должно быть пустым!");
            }
                con.Open();
                using (MySqlCommand com = new MySqlCommand("INSERT INTO department (Name,Cabinet) VALUES (@name,@cab)", con))
                {
                    com.Parameters.AddWithValue("@name", textBox1.Text);
                    com.Parameters.AddWithValue("@cab", Convert.ToInt32(textBox2.Text));
                    com.ExecuteNonQuery();
                    MessageBox.Show("Данные успешно занесены в таблицу!");
                }
            Directories d = new Directories(4);
            d.Show();
        }
    }
}
