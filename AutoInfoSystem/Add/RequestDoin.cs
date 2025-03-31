using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AutoInfoSystem
{
    public partial class RequestDoin : Form
    {
        Connection con2 = new Connection();
        MySqlConnection con;
        private string name1;
        private int id1;
        private string photo1;
        private string imageFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DrugsImages"); 
        public RequestDoin(int id,string name,string photo)
        {
            con = con2.con1;
            name1 = name;
            id1 = id;
            photo1 = photo;
            InitializeComponent();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Все поля заполнены правильно?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (string.IsNullOrWhiteSpace(textBox2.Text))
                    {
                        MessageBox.Show("Заполните все обязательные поля!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (int.Parse(textBox2.Text) > 1000)
                    {
                        MessageBox.Show("Нельзя запрашивать более чем 1000 препаратов за раз!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(textBox1.Text))
                    {
                        using (MySqlCommand com = new MySqlCommand("Insert into request(DrugId,Amount,Date,Status,Photo) Values(@Drugi,@Amoun,@Dat,@Stat,@Phot)", con))
                        {
                            com.Parameters.AddWithValue("@DrugI", id1);
                            com.Parameters.AddWithValue("@Amoun", int.Parse(textBox2.Text));
                            com.Parameters.AddWithValue("@Dat", textBox3.Text);
                            com.Parameters.AddWithValue("@Stat", "В рассмотрении");
                            com.Parameters.AddWithValue("@Phot", photo1);
                            com.ExecuteNonQuery();
                            MessageBox.Show("Заявки удачно добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }
                    else
                    {
                        using (MySqlCommand com = new MySqlCommand("Insert into request(DrugId,UserId,Amount,Date,Status,Photo) Values(@Drugi,@Useri,@Amoun,@Dat,@Stat,@Phot)", con))
                        {
                            com.Parameters.AddWithValue("@DrugI", id1);
                            com.Parameters.AddWithValue("@Useri", User.idEmpl);
                            com.Parameters.AddWithValue("@Amoun", int.Parse(textBox2.Text));
                            com.Parameters.AddWithValue("@Dat", textBox3.Text);
                            com.Parameters.AddWithValue("@Stat", "В рассмотрении");
                            com.Parameters.AddWithValue("@Phot", photo1);
                            com.ExecuteNonQuery();
                            MessageBox.Show("Заявки удачно добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            Drugs d = new Drugs();
                            this.Hide();
                            d.ShowDialog();
                        }
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

        private void RequestDoin_Load(object sender, EventArgs e)
        {
            con.Open();
            textBox1.Text = User.fio;
            textBox5.Text = name1;
            textBox3.Text = DateTime.Now.ToString("yyyy-MM-dd");
            string imagepath = Path.Combine(imageFolder, photo1);
            if (File.Exists(imagepath))
            {
                pictureBox1.Image = Image.FromFile(imagepath);
            }
            else
            {

            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
            {
                e.Handled = true; // Отменяем ввод, если символ не цифра или backspace
            }
        }

        private void pictureBox4_Click_1(object sender, EventArgs e)
        {
            Drugs d = new Drugs();
            this.Hide();
            d.ShowDialog();
        }
    }
}
