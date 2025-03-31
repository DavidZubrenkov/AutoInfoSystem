using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AutoInfoSystem
{
    public partial class EditDepartment : Form
    {
        private int d1;
        private int id1;
        private string namerow;
        private int cab;
        public EditDepartment(int d, int id, string NameRow, int cabinet)
        {
            InitializeComponent();
            d1 = d;
            id1 = id;
            namerow = NameRow;
            cab = cabinet;
            con = con2.con1;
        }
        Connection con2 = new Connection();
        MySqlConnection con;
        private void EditDepartment_Load(object sender, EventArgs e)
        {
            textBox1.Text = namerow;
            textBox2.Text = Convert.ToString(cab);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show($"Заполните все поля", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
                    con.Open();
                    using (MySqlCommand com = new MySqlCommand("Update department SET Name = @namee, Cabinet = @cab WHERE Id = @id", con))
                    {
                        com.Parameters.AddWithValue("@namee", textBox1.Text);
                        com.Parameters.AddWithValue("@cab", Convert.ToInt32(textBox2.Text));
                        com.Parameters.AddWithValue("@id", id1);
                        com.ExecuteNonQuery();
                    }
                    MessageBox.Show("Данные успешно обновленны!");
                    Directories d = new Directories(4);
                    d.Show();
                    this.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !Regex.IsMatch(e.KeyChar.ToString(), @"^[А-Яа-яЕё]$"))
            {
                e.Handled = true;
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Directories d = new Directories(4);
            d.Show();
            this.Close();
        }
    }
}
