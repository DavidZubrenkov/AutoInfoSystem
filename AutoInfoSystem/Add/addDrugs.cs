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
    public partial class addDrugs : Form
    {
        public addDrugs()
        {
            InitializeComponent();
            con = con21.con1;
        }
        MySqlConnection con;
        Connection con21 = new Connection();
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Drugs d = new Drugs();
            this.Hide();
            d.ShowDialog();
        }
        private string selecteimagepath = "";
        private string recourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DrugsImages");

        private void addDrugs_Load(object sender, EventArgs e)
        {
                try
                {
                    con.Open();
                    MySqlCommand com = new MySqlCommand("Select Name from category",con); // получаем имена из таблицы категории
                    MySqlDataReader read = com.ExecuteReader();
                    while (read.Read())
                    {
                        comboBox3.Items.Add(read["Name"].ToString()); // выводим их в combobox
                    }
                    read.Close();
                    comboBox3.SelectedIndex = 0;
                    MySqlCommand com1 = new MySqlCommand("Select Name from manufacturer",con); // получаем имена из таблицы производитель
                    MySqlDataReader read1 = com1.ExecuteReader();
                    while (read1.Read())
                    {
                        comboBox2.Items.Add(read1["Name"].ToString()); // выводим их в combobox
                    }
                    read1.Close();
                    comboBox2.SelectedIndex = 0;
                    comboBox1.SelectedIndex = 0;
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"{ex}","Ошибка!",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) // разрешаем ввод только цифр и управляющих клавиш
            {
                e.Handled = true;
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar) && e.KeyChar != (char)Keys.Back) //  разрешает ввод только букв, цифр и пробелов, а также позволяет использовать клавишу Backspace.
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e) // выбор изображения для лекарства
        {
            using(OpenFileDialog of = new OpenFileDialog())
            {
                of.Title = "Выберите изображение";
                of.Filter = "Изображения (*.jpeg;*.jpg;*.png) | *.jpeg;*.jpg;*.png";
                of.RestoreDirectory = true;
                if (of.ShowDialog() == DialogResult.OK)
                {
                    selecteimagepath = of.FileName;
                    pictureBox1.Image = Image.FromFile(of.FileName);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e) // Добавление нового лекарства в базу данных
        {
            try
            {
                DialogResult result = MessageBox.Show("Все поля заполненны правильно?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (string.IsNullOrWhiteSpace(textBox4.Text) || string.IsNullOrWhiteSpace(textBox5.Text)) // Проверка, что обязательные поля не пустые
                    {
                        MessageBox.Show("Заполните все поля!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    // Получение данных из полей ввода
                    string drugname = textBox5.Text.Trim();
                    int shelftime = Convert.ToInt32(textBox4.Text.Trim());
                    string categoryname = comboBox3.SelectedItem.ToString();
                    string realeseform = comboBox1.SelectedItem.ToString();
                    string manufucturerNmae = comboBox2.SelectedItem.ToString();
                    string imageName = Path.GetFileName(selecteimagepath);
                    string savePath = Path.Combine(recourcePath, imageName);
                    if (Path.GetFileName(selecteimagepath) == "") // Сохранение изображения в папку проекта
                    {

                    }
                    else
                    {

                        Directory.CreateDirectory(recourcePath);
                        File.Copy(selecteimagepath, savePath, true);
                    }
                    string query = @"
                                Insert into drug (name,CategoryId,ReleaseForm,ShelfLife,ManufacturerId,Photo)
                                Values (
                                @Name,
                                (Select Id From Category Where Name = @CategoryName),
                                @Releaseform,
                                @ShelfLife,
                                (Select Id from Manufacturer Where Name = @ManufacturerName),
                                @Photo)";
                    using (MySqlCommand com = new MySqlCommand(query, con))
                    {
                        com.Parameters.AddWithValue("@Name", drugname);
                        com.Parameters.AddWithValue("@CategoryName", categoryname);
                        com.Parameters.AddWithValue("@Releaseform", realeseform);
                        com.Parameters.AddWithValue("@ShelfLife", shelftime);
                        com.Parameters.AddWithValue("@ManufacturerName", manufucturerNmae);
                        com.Parameters.AddWithValue("@Photo", imageName);
                        com.ExecuteNonQuery();
                    }
                    MessageBox.Show("Лекарство успешно добавлено!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Drugs d = new Drugs();
                    this.Hide();
                    d.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox4_Click_1(object sender, EventArgs e)
        {
            Drugs d = new Drugs();
            this.Hide();
            d.Show();
        }
    }
}
