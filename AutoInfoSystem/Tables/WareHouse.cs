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
    public partial class WareHouse : Form
    {
        public WareHouse()
        {
            InitializeComponent();
            con = con2.con1;
        }
        private string defaultpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DrugsImages", "STOCK.png");
        private string imageFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DrugsImages");
        Connection con2 = new Connection();
        MySqlConnection con;
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
        private void WareHouse_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(new string[]
            {
                "Без сортировки",
                "Количество на складе ↑",
                "Количество на складе ↓"
            });
            comboBox1.SelectedIndex = 0;
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            if(User.role == 1 || User.role == 2)
            {
                button1.Visible = false;
            }
            dataGridView1.RowTemplate.Height = 150;
            string query = @"SELECT warehouse.Id,drug.Name as 'Препарат', AmountInStock 'Количество на складе', warehouse.ShelfLife 'Годен до', warehouse.Photo as 'photo' FROM db32.warehouse
            join drug on drug.Id = warehouse.DrugId;";
            MySqlDataAdapter ad = new MySqlDataAdapter(query,con);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            if (!dt.Columns.Contains("Image"))
            {
                dt.Columns.Add("Image", typeof(Image));
            }
            foreach (DataRow row in dt.Rows)
            {
                string photoname = row["photo"].ToString();
                string imagepath = Path.Combine(imageFolder, photoname);
                if (!File.Exists(imagepath))
                {
                    imagepath = defaultpath;
                }
                row["Image"] = Image.FromFile(imagepath);
            }
            dataGridView1.DataSource = dt;
            dataGridView1.Columns["Image"].HeaderText = "Фото";
            ((DataGridViewImageColumn)dataGridView1.Columns["Image"]).ImageLayout = DataGridViewImageCellLayout.Zoom;
            dataGridView1.Columns["Image"].DisplayIndex = 0;
            dataGridView1.Columns["Image"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridView1.Columns["Image"].Width = 150;
            dataGridView1.Columns["photo"].Visible = false;
            dataGridView1.Columns["Id"].Visible = false;


        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Вы точно хотите удалить препарат со склада?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    int id = int.Parse(dataGridView1.SelectedCells[0].Value.ToString());
                    using (MySqlCommand com = new MySqlCommand($"Delete from warehouse where Id = {id}", con))
                    {
                        com.ExecuteNonQuery();
                        MessageBox.Show("Препарат успешно удалён со склада", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        loaddata();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void loaddata()
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            if (User.role == 1 || User.role == 2)
            {
                button1.Visible = false;
            }
            dataGridView1.RowTemplate.Height = 150;
            string query = @"SELECT warehouse.Id,drug.Name as 'Препарат', AmountInStock 'Количество на складе', warehouse.ShelfLife 'Годен до', warehouse.Photo as 'photo' FROM db32.warehouse
            join drug on drug.Id = warehouse.DrugId;";
            MySqlDataAdapter ad = new MySqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            if (!dt.Columns.Contains("Image"))
            {
                dt.Columns.Add("Image", typeof(Image));
            }
            foreach (DataRow row in dt.Rows)
            {
                string photoname = row["photo"].ToString();
                string imagepath = Path.Combine(imageFolder, photoname);
                if (!File.Exists(imagepath))
                {
                    imagepath = defaultpath;
                }
                row["Image"] = Image.FromFile(imagepath);
            }
            dataGridView1.DataSource = dt;
            dataGridView1.Columns["Image"].HeaderText = "Фото";
            ((DataGridViewImageColumn)dataGridView1.Columns["Image"]).ImageLayout = DataGridViewImageCellLayout.Zoom;
            dataGridView1.Columns["Image"].DisplayIndex = 0;
            dataGridView1.Columns["Image"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridView1.Columns["Image"].Width = 150;
            dataGridView1.Columns["photo"].Visible = false;
            dataGridView1.Columns["Id"].Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Не выбран ни один препарат", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int id = int.Parse(dataGridView1.SelectedCells[0].Value.ToString());
            FromWarehouse frm = new FromWarehouse(id);
            this.Hide();
            frm.ShowDialog();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDataGridView();
        }
        private void UpdateDataGridView()
        {
            dataGridView1.RowTemplate.Height = 150;
            string query = @"SELECT warehouse.Id, drug.Name as 'Препарат', AmountInStock as 'Количество на складе', 
                            warehouse.ShelfLife as 'Годен до', warehouse.Photo as 'photo' 
                            FROM db32.warehouse 
                            JOIN drug ON drug.Id = warehouse.DrugId;";

            MySqlDataAdapter ad = new MySqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            ad.Fill(dt);

            // Добавляем столбец для изображений
            if (!dt.Columns.Contains("Image"))
            {
                dt.Columns.Add("Image", typeof(Image));
            }

            // Загружаем изображения
            foreach (DataRow row in dt.Rows)
            {
                string photoname = row["photo"].ToString();
                string imagepath = Path.Combine(imageFolder, photoname);
                if (!File.Exists(imagepath))
                {
                    imagepath = defaultpath;
                }
                row["Image"] = Image.FromFile(imagepath);
            }

            // Применяем сортировку
            string sortOption = comboBox1.SelectedItem?.ToString();
            if (sortOption == "Количество на складе ↑")
            {
                dt.DefaultView.Sort = "Количество на складе ASC";
            }
            else if (sortOption == "Количество на складе ↓")
            {
                dt.DefaultView.Sort = "Количество на складе DESC";
            }
            // Если выбрано "Без сортировки", сортировка не применяется

            // Привязываем данные к DataGridView
            dataGridView1.DataSource = dt;
            dataGridView1.Columns["Image"].HeaderText = "Фото";
            ((DataGridViewImageColumn)dataGridView1.Columns["Image"]).ImageLayout = DataGridViewImageCellLayout.Zoom;
            dataGridView1.Columns["Image"].DisplayIndex = 0;
            dataGridView1.Columns["Image"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridView1.Columns["Image"].Width = 150;
            dataGridView1.Columns["photo"].Visible = false;
            dataGridView1.Columns["Id"].Visible = false;
        }

    }
}
