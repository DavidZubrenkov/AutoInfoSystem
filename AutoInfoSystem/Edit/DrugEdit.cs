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
    public partial class DrugEdit : Form
    {
        private int drugid;
        private string imagename;
        private string imagefolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DrugsImages");
        Connection con2 = new Connection();
        MySqlConnection con;
        private Drugs parentdru;
        string categ;
        private string selecteimagepath = "";
        private string oldImageName = "";
        string manuf;
        public DrugEdit(int id, string name,string category,string manufacturer, string realeseform, int shelflife,string photo, Drugs parent)
        {
            this.parentdru = parent;
            categ = category;
            manuf = manufacturer;
            InitializeComponent();
            drugid = id;
            imagename = photo;
            textBox5.Text = name;
            comboBox1.SelectedItem = realeseform;
            textBox4.Text = shelflife.ToString();
            string imagePath = Path.Combine(imagefolder, photo);
            if (File.Exists(imagePath))
            {
                pictureBox1.Image = Image.FromFile(imagePath);
            }
            con = con2.con1;
        }

        private void DrugEdit_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                MySqlCommand com = new MySqlCommand("Select Name from category", con);
                MySqlDataReader read = com.ExecuteReader();
                while (read.Read())
                {
                    comboBox3.Items.Add(read["Name"].ToString());
                }
                read.Close();
                MySqlCommand com1 = new MySqlCommand("Select Name from manufacturer", con);
                MySqlDataReader read1 = com1.ExecuteReader();
                while (read1.Read())
                {
                    comboBox2.Items.Add(read1["Name"].ToString());
                }
                read1.Close();
                comboBox3.SelectedItem = categ;
                comboBox2.SelectedItem = manuf;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog of = new OpenFileDialog())
            {
                of.Title = "Выберите изображение";
                of.Filter = "Изображения (*.jpeg;*.jpg;*.png) | *.jpeg;*.jpg;*.png";
                of.RestoreDirectory = true;
                if (of.ShowDialog() == DialogResult.OK)
                {
                    selecteimagepath = of.FileName;
                    pictureBox1.Image = Image.FromFile(selecteimagepath);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Все поля заполнены правильно?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question); // подтверждаем добавление

                if (result == DialogResult.Yes)
                {
                    if (string.IsNullOrWhiteSpace(textBox4.Text) || string.IsNullOrWhiteSpace(textBox5.Text))
                    {
                        MessageBox.Show("Заполните все поля!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    string NewName = textBox5.Text;
                    string newCategory = comboBox3.SelectedItem.ToString();
                    string newManufacturer = comboBox2.SelectedItem.ToString();
                    string newReleaseForm = comboBox1.SelectedItem.ToString();
                    int newshellife = int.Parse(textBox4.Text);
                    string newimagename = oldImageName;

                    if (!string.IsNullOrWhiteSpace(selecteimagepath))
                    {
                        newimagename = Path.GetFileName(selecteimagepath);
                        string SavePath = Path.Combine(imagefolder, newimagename);
                        string oldImagePath = Path.Combine(imagefolder, oldImageName);
                        if (oldImageName != "STOCK.png" && File.Exists(oldImageName))
                        {
                            File.Delete(oldImagePath);
                        }
                        File.Copy(selecteimagepath, SavePath, true);
                    }
                    string updateQuery = @"
                Update drug
                Set Name = @name,
                CategoryId = (SELECT Id FROM category Where Name = @CategoryName),
                ReleaseForm = @ReleaseForm,
                ShelfLife = @Shelflife,
                ManufacturerId = (SELECT Id FROM manufacturer WHERE Name = @ManufacturerName),
                Photo = @Photo
                Where Id = @Id";
                    using (MySqlCommand com = new MySqlCommand(updateQuery, con))
                    {
                        com.Parameters.AddWithValue("@Id", drugid);
                        com.Parameters.AddWithValue("@name", NewName);
                        com.Parameters.AddWithValue("@CategoryName", newCategory);
                        com.Parameters.AddWithValue("@ReleaseForm", newReleaseForm);
                        com.Parameters.AddWithValue("@Shelflife", newshellife);
                        com.Parameters.AddWithValue("@ManufacturerName", newManufacturer);
                        com.Parameters.AddWithValue("@Photo", newimagename);
                        com.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Препарат успешно отредактирован", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        Drugs d = new Drugs();
                        this.Hide();
                        d.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!Char.IsLetterOrDigit(e.KeyChar) && !Char.IsWhiteSpace(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Drugs d = new Drugs();
            this.Hide();
            d.ShowDialog();
        }
    }
}
