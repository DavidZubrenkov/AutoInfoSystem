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
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                Title = "Выберите CSV файл"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedTable = comboBox1.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(selectedTable))
                {
                    MessageBox.Show("Пожалуйста, выберите таблицу!");
                    return;
                }

                Import(openFileDialog.FileName, selectedTable);
            }
        }

        private void ImportBD_Load(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();
                comboBox1.Items.Clear();
                MySqlCommand com = new MySqlCommand("SHOW TABLES;", con);
                using (MySqlDataReader read = com.ExecuteReader())
                {
                    while (read.Read())
                    {
                        comboBox1.Items.Add(read[0].ToString());
                    }
                }
                if (comboBox1.Items.Count > 0)
                {
                    comboBox1.SelectedIndex = 0;
                    comboBox1.Enabled = true;
                }
                else
                {
                    comboBox1.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                    using (MySqlCommand com = new MySqlCommand(script, con))
                    {
                        com.ExecuteNonQuery();
                        MessageBox.Show("Структура базы данных успешно восстановлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    comboBox1.Items.Clear();
                    MySqlCommand com1 = new MySqlCommand("Show Tables;", con);
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
        private void Import(string path, string table)
        {
            try
            {
                // Читаем CSV файл
                string[] lines = File.ReadAllLines(path, System.Text.Encoding.UTF8);
                if (lines.Length <= 1) // Проверяем, есть ли данные кроме заголовка
                {
                    MessageBox.Show("CSV файл пустой!");
                    return;
                }

                // Получаем заголовки
                string[] headers = lines[0].Split(';').Select(h => h.Trim('"')).ToArray();

                // Подготовка SQL запроса
                string columns = string.Join(",", headers);
                string placeholders = string.Join(",", headers.Select(h => $"@{h}"));
                string sql = $"INSERT INTO {table} ({columns}) VALUES ({placeholders})";

                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    // Добавляем параметры
                    foreach (string header in headers)
                    {
                        cmd.Parameters.Add($"@{header}", MySqlDbType.VarChar);
                    }

                    // Импортируем данные
                    for (int i = 1; i < lines.Length; i++)
                    {
                        string[] values = lines[i].Split(';');
                        for (int j = 0; j < headers.Length; j++)
                        {
                            cmd.Parameters[$"@{headers[j]}"].Value = values[j].Trim('"');
                        }
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Импорт успешно завершен!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
