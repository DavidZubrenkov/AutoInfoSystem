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
    public partial class AddDir : Form
    {
        private int i1;
        public AddDir(int i)
        {
            InitializeComponent();
            i1 = i;
        }
        Connection con2 = new Connection();
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !(e.KeyChar >= 'А' && e.KeyChar <= 'я') && e.KeyChar != 'ё' && e.KeyChar != 'Ё')
            {
                e.Handled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void AddDir_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MySqlConnection con = con2.con1;
            con.Open();
            try
            {
                if (textBox1.Text == null)
                {
                    MessageBox.Show("Поле не должно быть пустым!");
                }
                if (i1 == 1)
                {
                    using (MySqlCommand com = new MySqlCommand("INSERT INTO category (Name) VALUES (@name)", con))
                    {
                        com.Parameters.AddWithValue("@name", textBox1.Text);
                        com.ExecuteNonQuery();
                        MessageBox.Show("Данные успешно занесены в таблицу!");
                    }

                    Directories d = new Directories(1);
                    d.Show();
                    this.Close();
                }
                else if (i1 == 2)
                {
                    using (MySqlCommand com = new MySqlCommand("INSERT INTO manufacturer (Name) VALUES (@name)", con))
                    {
                        com.Parameters.AddWithValue("@name", textBox1.Text);
                        com.ExecuteNonQuery();
                        MessageBox.Show("Данные успешно занесены в таблицу!");
                    }

                    Directories d = new Directories(2);
                    d.Show();
                    this.Close();
                }
                else if (i1 == 3)
                {
                    using (MySqlCommand com = new MySqlCommand("INSERT INTO profession (Name) VALUES (@name)", con))
                    {
                        com.Parameters.AddWithValue("@name", textBox1.Text);
                        com.ExecuteNonQuery();
                        MessageBox.Show("Данные успешно занесены в таблицу!");
                    }
                    Directories d = new Directories(3);
                    d.Show();
                    this.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"{ex}", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (i1 == 1)
            {
                Directories d = new Directories(1);
                d.Show();
                this.Close();
            }
            else if (i1 == 2)
            {
                Directories d = new Directories(2);
                d.Show();
                this.Close();
            }
            else if (i1 == 3)
            {
                Directories d = new Directories(3);
                d.Show();
                this.Close();
            }
        }
    }
}
