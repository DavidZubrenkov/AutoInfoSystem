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
    public partial class DirEdit : Form
    {
        private string namerow;
        private int id1;
        private int d1;
        Connection con2 = new Connection();
        public DirEdit(int d,int id,string NameRow)
        {
            InitializeComponent();
            namerow = NameRow;
            id1 = id;
            d1 = d;
        }
        private void DirEdit_Load(object sender, EventArgs e)
        {
            textBox1.Text = namerow;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MySqlConnection con = con2.con1;
            try
            {
                if (textBox1.Text == null)
                {
                    MessageBox.Show($"Заполните все поля", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (d1 == 1)
                {
                    using (con)
                    {
                        con.Open();
                        using (MySqlCommand com = new MySqlCommand("Update category SET Name = @namee WHERE Id = @id", con))
                        {
                            com.Parameters.AddWithValue("@namee", textBox1.Text);
                            com.Parameters.AddWithValue("@id", id1);
                            com.ExecuteNonQuery();
                        }
                        MessageBox.Show("Данные успешно обновленны!");
                        Directories d = new Directories(1);
                        d.Show();
                        this.Close();
                    }
                }
                else if (d1 == 2)
                {
                    using (con)
                    {
                        con.Open();
                        using (MySqlCommand com = new MySqlCommand("Update manufacturer SET Name = @namee WHERE Id = @id", con))
                        {
                            com.Parameters.AddWithValue("@namee", textBox1.Text);
                            com.Parameters.AddWithValue("@id", id1);
                            com.ExecuteNonQuery();
                        }
                        MessageBox.Show("Данные успешно обновленны!");
                        Directories d = new Directories(2);
                        d.Show();
                        this.Close();
                    }
                }
                else if (d1 == 3)
                {
                    using (con)
                    {
                        con.Open();
                        using (MySqlCommand com = new MySqlCommand("Update profession SET Name = @namee WHERE Id = @id", con))
                        {
                            com.Parameters.AddWithValue("@namee", textBox1.Text);
                            com.Parameters.AddWithValue("@id", id1);
                            com.ExecuteNonQuery();
                        }
                        MessageBox.Show("Данные успешно обновленны!");
                        Directories d = new Directories(3);
                        d.Show();
                        this.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"{ex}","Ошибка!",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ')
            {
                e.Handled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (d1 == 1)
            {
                Directories d = new Directories(1);
                d.Show();
                this.Close();
            }
            else if (d1 == 2)
            {
                Directories d = new Directories(2);
                d.Show();
                this.Close();
            }
            else if (d1 == 3)
            {
                Directories d = new Directories(3);
                d.Show();
                this.Close();
            }
        }
    }
}
