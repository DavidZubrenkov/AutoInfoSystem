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
    public partial class ImportBD : Form
    {
        public ImportBD()
        {
            InitializeComponent();
        }
        MySqlConnection con = new MySqlConnection("server = localhost;uid=root;pwd=root;database=db32test");
        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void ImportBD_Load(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Login l = new Login();
            this.Hide();
            l.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog
                {
                    Filter = "SQL Files|*.sql",
                    Title = "Выберите SQL файл"
                };
                if (op.ShowDialog() == DialogResult.OK)
                {
                    string script = File.ReadAllText(op.FileName);
                    using(MySqlCommand com = new MySqlCommand(script,con))
                    {
                        com.ExecuteNonQuery();
                        MessageBox.Show("Структура базы данных успешно восстановлена!","Успех",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
                    }
                    comboBox1.Items.Clear();
                    MySqlCommand com1 = new MySqlCommand("Show Tables;",con);
                    MySqlDataReader read = com1.ExecuteReader();
                    while (read.Read())
                    {
                        comboBox1.Items.Add(read[0].ToString());
                    }
                    read.Close();
                    comboBox1.SelectedIndex = 0;
                    comboBox1.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
