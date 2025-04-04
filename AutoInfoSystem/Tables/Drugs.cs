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
        private int pagesize = 10;
        private int currentpage = 1;
        private int totalrows = 0;
        private int totalpages = 0;
        public Drugs()
        {
            InitializeComponent();
            con = con2.con1;
            Drugs_Load();
            PanatgiionIni();
        }
        Connection con2 = new Connection();
        public string query = @"SELECT d.Id, d.Photo as 'DrugImage',d.Name as 'Название',c.Name as 'Категория', d.ReleaseForm as 'Форма выпуска',d.ShelfLife as 'Срок годности (Год)', m.Name as 'Производитель' FROM drug d JOIN category c ON d.CategoryId = c.Id JOIN manufacturer m ON d.ManufacturerId = m.Id";
        public MySqlConnection con;
        DataTable originalTable = new DataTable();
        private void PanatgiionIni()
        {
            label3.Location = new Point(10, dataGridView1.Bottom + 10);
            label3.Width = 150;
            label3.AutoSize = true;

            flowLayoutPanel1.Location = new Point(label3.Right + 10, dataGridView1.Bottom + 10);
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;
            this.KeyPreview = true;
            this.Controls.Add(label3);
            this.Controls.Add(flowLayoutPanel1);
        }
        public void Drugs_Load()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(new string[]
            {
                "Без фильтрации",
                "Категория:Анальгетик, жаропонижающее",
                "Категория:Антибиотик",
                "Категория:Муколитики",
                "Производитель:Berlin-Chemie",
                "Производитель:Sandoz",
                "Производитель:Sanofi"
            });

            // Заполняем comboBox2 (Сортировка)
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(new string[]
            {
                "Без сортировки",
                "Срок годности ↑",
                "Срок годности ↓"
            });

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            if (User.role == 1 || User.role == 2)
            {
                button1.Visible = false;
            }
            if(User.role == 3)
            {
                button2.Visible = false;
                button3.Visible = false;
                button4.Visible = false;
            }
            dataGridView1.RowTemplate.Height = 150;
            UpdateDataGridView();
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);
                string name = dataGridView1.SelectedRows[0].Cells["Название"].Value.ToString();
                string photo = dataGridView1.SelectedRows[0].Cells["DrugImage"].Value.ToString();
                RequestDoin re = new RequestDoin(id,name,photo);
                this.Hide();
                re.ShowDialog();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите препарат для редактирования", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            this.Hide();
            ed.Show();


        }

        private void button3_Click(object sender, EventArgs e)
        {
            con.Close();
            addDrugs dr = new addDrugs();
            this.Hide();
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
            UpdateDataGridView();
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
            try
            {
                DialogResult result = MessageBox.Show("Вы действительно хотите удалить препарат?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    DataGridViewRow row = dataGridView1.SelectedRows[0];
                    using (MySqlCommand com = new MySqlCommand("DELETE from Drug where Id = @id", con))
                    {
                        com.Parameters.AddWithValue("@id", Convert.ToInt32(row.Cells["Id"].Value));
                        com.ExecuteNonQuery();
                        MessageBox.Show("Препарат удалён", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDataGridView();
        }

        private void Drugs_Load(object sender, EventArgs e)
        {

        }
        private void UpdateDataGridView()
        {
            // Базовый запрос
            string countquery = @"SELECT COUNT(*)
                                FROM drug d 
                                JOIN category c ON d.CategoryId = c.Id 
                                JOIN manufacturer m ON d.ManufacturerId = m.Id 
                                WHERE 1=1";

            string query1 = @"SELECT d.Id, d.Photo as 'DrugImage', d.Name as 'Название', c.Name as 'Категория', 
                             d.ReleaseForm as 'Форма выпуска', d.ShelfLife as 'Срок годности (Год)', m.Name as 'Производитель' 
                             FROM drug d 
                             JOIN category c ON d.CategoryId = c.Id 
                             JOIN manufacturer m ON d.ManufacturerId = m.Id 
                             WHERE 1=1";

            // Умный поиск
            string searchText = textBox1.Text.Trim();
            if (searchText.Length >= 3)
            {
                countquery += " AND d.Name LIKE @search";
                query1 += " AND d.Name LIKE @search";
            }

            // Фильтрация по comboBox1
            string selectedFilter = comboBox1.SelectedItem?.ToString();
            if (selectedFilter != "Без фильтрации")
            {
                if (selectedFilter.StartsWith("Категория:"))
                {
                    string category = selectedFilter.Replace("Категория:", "");
                    countquery += " AND c.Name = @category";
                    query1 += " AND c.Name = @category";
                }
                else if (selectedFilter.StartsWith("Производитель:"))
                {
                    string manufacturer = selectedFilter.Replace("Производитель:", "");
                    countquery += " AND m.Name = @manufacturer";
                    query1 += " AND m.Name = @manufacturer";
                }
            }
            string sortOption = comboBox2.SelectedItem?.ToString();
            if (sortOption == "Срок годности ↓")
            {
                query1 += " Order BY d.ShelfLife ASC";
            }
            else if (sortOption == "Срок годности ↑")
            {
                query1 += " Order BY d.ShelfLife DESC";
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            // Выполняем запрос
            using (MySqlCommand com = new MySqlCommand(countquery, con))
            {
                // Добавляем параметры
                if (searchText.Length >= 3)
                {
                    com.Parameters.AddWithValue("@search", "%" + searchText + "%");
                }
                if (selectedFilter != "Без фильтрации")
                {
                    if (selectedFilter.StartsWith("Категория:"))
                    {
                        com.Parameters.AddWithValue("@category", selectedFilter.Replace("Категория:", ""));
                    }
                    else if (selectedFilter.StartsWith("Производитель:"))
                    {
                        com.Parameters.AddWithValue("@manufacturer", selectedFilter.Replace("Производитель:", ""));
                    }
                }
                totalrows = Convert.ToInt32(com.ExecuteScalar());
                totalpages = (int)Math.Ceiling((double)totalrows / pagesize);
                if (currentpage > totalpages && totalpages > 0)
                {
                    currentpage = totalpages;
                }
                else if (totalrows == 0)
                {
                    currentpage = 1;
                }
            }
            int offset = (currentpage - 1) * pagesize;
            query1 += $" Limit {pagesize} OFFSET {offset}";
            using(MySqlCommand datacom = new MySqlCommand(query1,con))
            {
                if (searchText.Length >= 3)
                {
                    datacom.Parameters.AddWithValue("@search", "%" + searchText + "%");
                }
                if (selectedFilter != "Без фильтрации")
                {
                    if (selectedFilter.StartsWith("Категория:"))
                    {
                        datacom.Parameters.AddWithValue("@category", selectedFilter.Replace("Категория:", ""));
                    }
                    else if (selectedFilter.StartsWith("Производитель:"))
                    {
                        datacom.Parameters.AddWithValue("@manufacturer", selectedFilter.Replace("Производитель:", ""));
                    }
                }
                MySqlDataAdapter ad = new MySqlDataAdapter(datacom);
                DataTable pagedtable = new DataTable();
                ad.Fill(pagedtable);
                if (!pagedtable.Columns.Contains("Image"))
                {
                    pagedtable.Columns.Add("Image", typeof(Image));
                }
                foreach(DataRow row in pagedtable.Rows)
                {
                     string photoName = row["DrugImage"].ToString();
                    string imagepath = Path.Combine(imageFolder, photoName);
                    if (!File.Exists(imagepath))
                    {
                        imagepath = defaultpath;
                    }
                    row["Image"] = Image.FromFile(imagepath);
                }
                dataGridView1.DataSource = pagedtable;
                ConfigureDatagridview();
                Pagination();
            }
            con.Close();
        }
        private void ConfigureDatagridview()
        {
            dataGridView1.Columns["Image"].HeaderText = "Фото";
            ((DataGridViewImageColumn)dataGridView1.Columns["Image"]).ImageLayout = DataGridViewImageCellLayout.Zoom;
            dataGridView1.Columns["Image"].DisplayIndex = 0;
            dataGridView1.Columns["Image"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridView1.Columns["Image"].Width = 150;
            dataGridView1.Columns["DrugImage"].Visible = false;
            dataGridView1.Columns["Id"].Visible = false;
        }
        private void Pagination()
        {
            int visibleRows = Math.Min(currentpage * pagesize, totalrows);
            label3.Text = $"Количество записей: {visibleRows} из {totalrows}";
            flowLayoutPanel1.Controls.Clear();
            for(int i = 1;i <= totalpages; i++)
            {
                Button pagebutton = new Button
                {
                    Text = i.ToString(),
                    Width = 30,
                    Enabled = (i != currentpage)
                };
                int pageNum = i;
                pagebutton.Click += (s, e) => GotoPage(pageNum);
                flowLayoutPanel1.Controls.Add(pagebutton);
            }
        }
        private void GotoPage(int page)
        {
            currentpage = page;
            UpdateDataGridView();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDataGridView();
        }

        private void Drugs_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left && currentpage > 1)
            {
                currentpage--;
                UpdateDataGridView();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Right && currentpage < totalpages)
            {
                currentpage++;
                UpdateDataGridView();
                e.Handled = true;
            }
        }
    }
}