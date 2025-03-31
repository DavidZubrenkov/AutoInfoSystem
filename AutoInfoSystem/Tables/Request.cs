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
using Word = Microsoft.Office.Interop.Word;
using MySql.Data.MySqlClient;

namespace AutoInfoSystem
{
    public partial class Request : Form
    {
        Connection con2 = new Connection();
        MySqlConnection con;
        public int year;
        public Request()
        {
            con = con2.con1;
            InitializeComponent();
        }
        private string defaultpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DrugsImages", "STOCK.png");
        private string imageFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DrugsImages");
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

        private void Request_Load(object sender, EventArgs e)
        {
            try
            {
                if (User.role == 1 || User.role == 3)
                {
                    comboBox1.Visible = false;
                    comboBox3.Visible = false;
                    comboBox2.Visible = false;
                    button2.Visible = false;
                    this.Size = new Size(1327, 540);
                }
                dataGridView1.RowTemplate.Height = 150;
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();
                addDate();
                DataTable dt = new DataTable();
                string query = @"SELECT 
	        r.Id as id,
            r.Photo as photo,
            d.name AS Название,
            CONCAT(e.LastName, ' ', e.Name, ' ', COALESCE(e.Patronyc, '')) AS Сотрудник,
            r.Amount as Количество,
            r.Date as 'Дата поступления заявки',
             r.Status as Статус
            FROM 
            request r
            LEFT JOIN Drug d ON r.DrugId = d.Id
            LEFT JOIN employee e ON r.UserId = e.UserId;";
                MySqlDataAdapter ad = new MySqlDataAdapter(query, con);
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
                dataGridView1.Columns["id"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void addDate()
        {
            using (MySqlCommand com = new MySqlCommand("Select DISTINCT Date from request",con))
            {
                using(MySqlDataReader read = com.ExecuteReader())
                {
                    while (read.Read())
                    {
                        DateTime date = Convert.ToDateTime(read[0]);
                        string formatedate = date.ToString("yyyy.MM.dd");
                        comboBox1.Items.Add(formatedate);
                        comboBox3.Items.Add(formatedate);
                    }
                    read.Close();
                }
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string status = dataGridView1.SelectedCells[6].Value.ToString();
            if (status == "Одобрен")
            {
                comboBox2.Enabled = false;
            }
            else
            {
                comboBox2.SelectedItem = dataGridView1.SelectedCells[6].Value.ToString();
                comboBox2.Enabled = true;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox2.SelectedItem.ToString() == "Одобрен")
                {
                    DialogResult result = MessageBox.Show("Вы точно хотите одобрит заявку?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        int id = int.Parse(dataGridView1.SelectedCells[0].Value.ToString());
                        using (MySqlCommand com2 = new MySqlCommand($"Select status from request where Id = {id}",con))
                        {
                            using(MySqlDataReader read = com2.ExecuteReader())
                            {
                                while (read.Read())
                                {
                                    if(read[0].ToString() == "Одобрен")
                                    {
                                        MessageBox.Show("Данная заявка уже одобренна!","Предупреждение",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                                        return;
                                    }
                                }
                                read.Close();
                            }
                        }
                        string name = dataGridView1.SelectedCells[2].Value.ToString();
                        using (MySqlCommand com1 = new MySqlCommand($"Select ShelfLife from drug where name = '{name}'", con))
                        {
                            using (MySqlDataReader read = com1.ExecuteReader())
                            {
                                while (read.Read())
                                {
                                    year = int.Parse(read[0].ToString());
                                }
                                read.Close();
                            }
                        }
                        string date = DateTime.Now.AddYears(year).ToString("yyyy.MM.dd");
                        string query = "Insert into warehouse(DrugId,AmountInStock,ShelfLife,Photo) VALUES ((SELECT Id from drug where Name = @drugname),@Amoun,@Slife,@Phot)";
                        using (MySqlCommand com = new MySqlCommand(query, con))
                        {
                            com.Parameters.AddWithValue("@drugname", dataGridView1.SelectedCells[2].Value.ToString());
                            com.Parameters.AddWithValue("@Amoun", int.Parse(dataGridView1.SelectedCells[4].Value.ToString()));
                            com.Parameters.AddWithValue("@Slife", date);
                            com.Parameters.AddWithValue("@Phot", dataGridView1.SelectedCells[1].Value.ToString());
                            com.ExecuteNonQuery();
                            MessageBox.Show("Препарат добавлен на склад", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                        string query1 = $"Update request set Status = 'Одобрен' where id = {id}";
                        using (MySqlCommand com1 = new MySqlCommand(query1, con))
                        {
                            com1.ExecuteNonQuery();
                            LoadNew();
                        }
                    }
                }
                else
                {
                    int id = int.Parse(dataGridView1.SelectedCells[0].Value.ToString());
                    string query1 = $"Update request set Status = @stat where id = {id}";
                    using (MySqlCommand com2 = new MySqlCommand(query1, con))
                    {
                        com2.Parameters.AddWithValue("@stat",comboBox2.SelectedItem.ToString());
                        com2.ExecuteNonQuery();
                        LoadNew();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadNew()
        {
            if (User.role == 1 || User.role == 3)
            {
                comboBox1.Visible = false;
                comboBox3.Visible = false;
                comboBox2.Visible = false;
                button4.Visible = false;
                this.Size = new Size(1327, 540);
            }
            dataGridView1.RowTemplate.Height = 150;
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            addDate();
            DataTable dt = new DataTable();
            string query = @"SELECT 
	        r.Id as id,
            r.Photo as photo,
            d.name AS Название,
            CONCAT(e.LastName, ' ', e.Name, ' ', COALESCE(e.Patronyc, '')) AS Сотрудник,
            r.Amount as Количество,
            r.Date as 'Дата поступления заявки',
             r.Status as Статус
            FROM 
            request r
            LEFT JOIN Drug d ON r.DrugId = d.Id
            LEFT JOIN employee e ON r.UserId = e.UserId;";
            MySqlDataAdapter ad = new MySqlDataAdapter(query, con);
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
            dataGridView1.Columns["id"].Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Вы точно хотите удалить сотрудника?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    int id = int.Parse(dataGridView1.SelectedCells[0].Value.ToString());
                    using (MySqlCommand com = new MySqlCommand($"Delete from request where id = {id}", con))
                    {
                        com.ExecuteNonQuery();
                        MessageBox.Show("Заявка удалена", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        LoadNew();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверяем, выбраны ли даты
                if (comboBox1.SelectedItem == null || comboBox3.SelectedItem == null)
                {
                    MessageBox.Show("Пожалуйста, выберите начальную и конечную даты!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Получаем даты из комбобоксов
                DateTime startDate = DateTime.ParseExact(comboBox1.SelectedItem.ToString(),
                    "yyyy.MM.dd", System.Globalization.CultureInfo.InvariantCulture);
                DateTime endDate = DateTime.ParseExact(comboBox3.SelectedItem.ToString(),
                    "yyyy.MM.dd", System.Globalization.CultureInfo.InvariantCulture);

                // Проверяем, что начальная дата не больше конечной
                if (startDate > endDate)
                {
                    MessageBox.Show("Начальная дата не может быть больше конечной даты!",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Создаем запрос для получения одобренных заявок в заданном диапазоне дат
                string query = @"SELECT 
                    d.name AS Название,
                    CONCAT(e.LastName, ' ', e.Name, ' ', COALESCE(e.Patronyc, '')) AS Сотрудник,
                     r.Amount as Количество
                    FROM 
                    request r
                    LEFT JOIN Drug d ON r.DrugId = d.Id
                    LEFT JOIN employee e ON r.UserId = e.UserId
                    WHERE 
                    r.Status = 'Одобрен' 
                    AND r.Date BETWEEN @startDate AND @endDate;";

                DataTable dt = new DataTable();
                using (MySqlCommand com = new MySqlCommand(query, con))
                {
                    com.Parameters.AddWithValue("@startDate", startDate.ToString("yyyy-MM-dd"));
                    com.Parameters.AddWithValue("@endDate", endDate.ToString("yyyy-MM-dd"));
                    MySqlDataAdapter ad = new MySqlDataAdapter(com);
                    ad.Fill(dt);
                }

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Нет одобренных заявок за указанный период!",
                        "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Создаем диалоговое окно для выбора пути сохранения
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Документ Word (*.docx)|*.docx";
                saveFileDialog.Title = "Сохранить отчет";
                saveFileDialog.FileName = $"Отчет_по_заявкам_{startDate.ToString("yyyyMMdd")}_{endDate.ToString("yyyyMMdd")}.docx"; // Предложенное имя файла
                saveFileDialog.DefaultExt = "docx";
                saveFileDialog.AddExtension = true;

                // Показываем диалоговое окно и проверяем, выбрал ли пользователь путь
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    MessageBox.Show("Сохранение отменено пользователем.", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Получаем путь для сохранения файла
                string filePath = saveFileDialog.FileName;

                // Создаем документ Word
                Word.Application wordApp = new Word.Application();
                Word.Document doc = wordApp.Documents.Add();

                try
                {
                    // Добавляем заголовок
                    Word.Paragraph header = doc.Paragraphs.Add();
                    header.Range.Text = $"Отчет по одобренным заявкам с {startDate.ToString("dd.MM.yyyy")} по {endDate.ToString("dd.MM.yyyy")}";
                    header.Range.Font.Size = 16;
                    header.Range.Font.Bold = 1;
                    header.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    header.Range.InsertParagraphAfter();

                    // Добавляем информацию о сотруднике, если есть
                    if (!string.IsNullOrEmpty(User.fio))
                    {
                        Word.Paragraph employeeInfo = doc.Paragraphs.Add();
                        employeeInfo.Range.Text = $"Составил: {User.fio}";
                        employeeInfo.Range.Font.Size = 12;
                        employeeInfo.Range.InsertParagraphAfter();
                    }

                    // Добавляем пустую строку
                    doc.Paragraphs.Add();

                    // Проходим по всем записям и добавляем их в документ
                    foreach (DataRow row in dt.Rows)
                    {
                        Word.Paragraph drugInfo = doc.Paragraphs.Add();
                        drugInfo.Range.Text = $"Препарат: {row["Название"]}";
                        drugInfo.Range.Font.Size = 12;
                        drugInfo.Range.InsertParagraphAfter();

                        Word.Paragraph employee = doc.Paragraphs.Add();
                        employee.Range.Text = $"Сотрудник: {row["Сотрудник"]}";
                        employee.Range.Font.Size = 12;
                        employee.Range.InsertParagraphAfter();

                        Word.Paragraph amount = doc.Paragraphs.Add();
                        amount.Range.Text = $"Количество препаратов: {row["Количество"]}";
                        amount.Range.Font.Size = 12;
                        amount.Range.InsertParagraphAfter();

                        // Добавляем пустую строку между записями
                        doc.Paragraphs.Add();
                    }

                    // Сохраняем документ по указанному пути
                    doc.SaveAs(filePath);
                    doc.Close();
                    wordApp.Quit();

                    MessageBox.Show($"Отчет успешно сохранен по пути: {filePath}", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при создании отчета: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    doc.Close(false);
                    wordApp.Quit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
} 
