using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AutoInfoSystem
{
    public partial class Drugs : Form
    {
        private string imageFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DrugsImages");
        private string defaultpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DrugsImages", "STOCK.png");
        public Drugs()
        {
            InitializeComponent();
            con = con2.con1;
            Drugs_Load();
        }
        Connection con2 = new Connection();
        public string query = @"SELECT d.Id, d.Photo as 'DrugImage',d.Name as 'Название',c.Name as 'Категория', d.ReleaseForm as 'Форма выпуска',d.ShelfLife as 'Срок годности (Год)', m.Name as 'Производитель' FROM drug d JOIN category c ON d.CategoryId = c.Id JOIN manufacturer m ON d.ManufacturerId = m.Id";
        public MySqlConnection con;
        DataTable originalTable = new DataTable();

        public void Drugs_Load()
        {
            originalTable.Clear();
            dataGridView1.RowTemplate.Height = 150;
            if(con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            else { }
                    using(MySqlCommand com = new MySqlCommand(query,con))
                    using(MySqlDataAdapter ad = new MySqlDataAdapter(com))
                    {
                        ad.Fill(originalTable);
                    }
            if (!originalTable.Columns.Contains("Image"))
            {
                originalTable.Columns.Add("Image", typeof(Image));
            }
            foreach (DataRow row in originalTable.Rows)
                {
                    string photoName = row["DrugImage"].ToString();
                    string imagepath = Path.Combine(imageFolder, photoName);
                    if (!File.Exists(imagepath))
                    {
                        imagepath = defaultpath;
                    }
                    row["Image"] = Image.FromFile(imagepath);
                }
                dataGridView1.DataSource = originalTable;
                dataGridView1.Columns["Image"].HeaderText = "Фото";
                ((DataGridViewImageColumn)dataGridView1.Columns["Image"]).ImageLayout = DataGridViewImageCellLayout.Zoom;
                dataGridView1.Columns["Image"].DisplayIndex = 0;
            dataGridView1.Columns["Image"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridView1.Columns["Image"].Width = 150;
            dataGridView1.Columns["DrugImage"].Visible = false;
            dataGridView1.Columns["Id"].Visible = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            RequestDoin re = new RequestDoin();
            re.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выбериет препарат для редактирования", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DataGridViewRow row = dataGridView1.SelectedRows[0];
            int drugId = Convert.ToInt32(row.Cells["id"].Value);
            string drugName = row.Cells["Название"].Value.ToString();
            string categoryname = row.Cells["Категория"].Value.ToString();
            string manufacturername = row.Cells["Производитель"].Value.ToString();
            string realeseform = row.Cells["Форма выпуска"].Value.ToString();
            int shelflife = Convert.ToInt32(row.Cells["Срок годности (Год)"].Value);
            string imageName = row.Cells["DrugImage"].Value.ToString();
            DrugEdit ed = new DrugEdit(drugId, drugName, categoryname, manufacturername, realeseform, shelflife, imageName,this);
            ed.ShowDialog();


        }

        private void button3_Click(object sender, EventArgs e)
        {
            con.Close();
            addDrugs dr = new addDrugs(this);
            dr.ShowDialog();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (User.role == 1)
            {
                Main m = new Main();
                this.Hide();
                m.ShowDialog();
            }
            else if (User.role == 2)
            {
                MainGM m = new MainGM();
                this.Hide();
                m.ShowDialog();
            }
            else if (User.role == 3)
            {
                MainSM m = new MainSM();
                this.Hide();
                m.ShowDialog();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim().Length >= 3)
            {
                string query1 = $@"SELECT d.Id, d.Photo as 'DrugImage',d.Name as 'Название',c.Name as 'Категория', d.ReleaseForm as 'Форма выпуска',d.ShelfLife as 'Срок годности (Год)', m.Name as 'Производитель' FROM drug d JOIN category c ON d.CategoryId = c.Id JOIN manufacturer m ON d.ManufacturerId = m.Id WHERE d.Name LIKE @search";

                    using (MySqlCommand com = new MySqlCommand(query1, con))
                    {
                        com.Parameters.AddWithValue("@search", "%" + textBox1.Text.Trim() + "%");
                        MySqlDataAdapter ad = new MySqlDataAdapter(com);
                        DataTable filtredTable = new DataTable();
                        ad.Fill(filtredTable);
                    if (!filtredTable.Columns.Contains("Image"))
                    {
                        filtredTable.Columns.Add("Image", typeof(Image));
                    }
                    foreach (DataRow row in filtredTable.Rows)
                        {
                            string photoName = row["DrugImage"].ToString();
                            string imagepath = Path.Combine(imageFolder, photoName);
                            if (!File.Exists(imagepath))
                            {
                                imagepath = defaultpath;
                            }
                            row["Image"] = Image.FromFile(imagepath);
                        }
                        dataGridView1.DataSource = filtredTable;
                        dataGridView1.Columns["Image"].HeaderText = "Фото";
                        ((DataGridViewImageColumn)dataGridView1.Columns["Image"]).ImageLayout = DataGridViewImageCellLayout.Zoom;
                        dataGridView1.Columns["Image"].DisplayIndex = 0;
                        dataGridView1.Columns["Image"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        dataGridView1.Columns["Image"].Width = 150;
                    dataGridView1.Columns["DrugImage"].Visible = false;
                        dataGridView1.Columns["Id"].Visible = false;
                    }
            }
            else
            {
                dataGridView1.DataSource = originalTable;
            }
        }

        private void Drugs_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsLetterOrDigit(e.KeyChar) && !Char.IsWhiteSpace(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите удалить препарат?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                    using (MySqlCommand com = new MySqlCommand("DELETE from Drug where Id = @id", con))
                    {
                        com.Parameters.AddWithValue("@id", Convert.ToInt32(row.Cells["Id"].Value));
                        com.ExecuteNonQuery();
                        MessageBox.Show("Данные удалены", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        using (MySqlCommand com1 = new MySqlCommand(query, con))
                        {
                            MySqlDataAdapter ad = new MySqlDataAdapter(com1);
                            DataTable filtredTable = new DataTable();
                            ad.Fill(filtredTable);
                        if (!filtredTable.Columns.Contains("Image"))
                        {
                            filtredTable.Columns.Add("Image", typeof(Image));
                        }
                        foreach (DataRow row1 in filtredTable.Rows)
                        {
                            string photoName = row1["DrugImage"].ToString();
                            string imagepath = Path.Combine(imageFolder, photoName);
                            if (!File.Exists(imagepath))
                            {
                                imagepath = defaultpath;
                            }
                            row1["Image"] = Image.FromFile(imagepath);
                        }
                        dataGridView1.DataSource = filtredTable;
                        dataGridView1.Columns["Image"].HeaderText = "Фото";
                        ((DataGridViewImageColumn)dataGridView1.Columns["Image"]).ImageLayout = DataGridViewImageCellLayout.Zoom;
                        dataGridView1.Columns["Image"].DisplayIndex = 0;
                        dataGridView1.Columns["Image"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        dataGridView1.Columns["Image"].Width = 150;
                        dataGridView1.Columns["DrugImage"].Visible = false;
                        dataGridView1.Columns["Id"].Visible = false;
                        Drugs_Load();
                    }
                    }
            }
            else
            {
                return;
            }
        }
    }
}