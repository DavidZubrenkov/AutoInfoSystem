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
    public partial class Directories : Form
    {
        private int p1;
        Connection con2 = new Connection();
        MySqlConnection con;
        public Directories(int p)
        {
            InitializeComponent();
            p1 = p;
            con = con2.con1;
        }
        private void Directories_Load(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
                MySqlDataAdapter ad = new MySqlDataAdapter("SELECT Id, Name as 'Название' from category", con); // выводим данные из справочников в  datagridview
                DataTable dt = new DataTable();
                ad.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.Columns["Id"].Visible = false;
                MySqlDataAdapter ad1 = new MySqlDataAdapter("Select Id, Name as 'Название' from manufacturer", con);
                DataTable dt1 = new DataTable();
                ad1.Fill(dt1);
                dataGridView2.DataSource = dt1;
                dataGridView2.Columns["Id"].Visible = false;
                MySqlDataAdapter ad2 = new MySqlDataAdapter("Select Id, Name as 'Название' from profession;", con);
                DataTable dt2 = new DataTable();
                ad2.Fill(dt2);
                dataGridView4.DataSource = dt2;
                dataGridView4.Columns["Id"].Visible = false;
            if (p1 == 1) // получаем номер datagridview который нужно обновить
            {
                tabControl1.SelectedIndex = 0;
                    MySqlDataAdapter ad4 = new MySqlDataAdapter("SELECT  Id, Name as 'Название' from category", con);
                    DataTable dt4 = new DataTable();
                    ad4.Fill(dt4);
                    dataGridView1.DataSource = dt4;
                    dataGridView1.Columns["Id"].Visible = false;
            }
            else if (p1 == 2)
            {
                tabControl1.SelectedIndex = 1;
                    MySqlDataAdapter ad5 = new MySqlDataAdapter("SELECT Id, Name as 'Название' from manufacturer", con);
                    DataTable dt5 = new DataTable();
                    ad5.Fill(dt5);
                    dataGridView2.DataSource = dt5;
                    dataGridView2.Columns["Id"].Visible = false;
            }
            else if (p1 == 3)
             {
                tabControl1.SelectedIndex = 3;
                using (MySqlConnection con = con2.con1)
                    {
                        MySqlDataAdapter ad6 = new MySqlDataAdapter("SELECT Id, Name as 'Название' from profession", con);
                        DataTable dt6 = new DataTable();
                        ad6.Fill(dt6);
                        dataGridView4.DataSource = dt6;
                        dataGridView4.Columns["Id"].Visible = false;
                    }
                }
            else if (p1 == 4)
            {
                tabControl1.SelectedIndex = 2;
                using (MySqlConnection con = con2.con1)
                {
                    MySqlDataAdapter ad7 = new MySqlDataAdapter("Select Id, Name as 'Название' from profession", con);
                    DataTable dt7 = new DataTable();
                    ad7.Fill(dt7);
                    dataGridView4.DataSource = dt7;
                    dataGridView4.Columns["Id"].Visible = false;
                }
            }
        }
        private void pictureBox8_Click(object sender, EventArgs e) // переход на главную форму
        {
            Main m = new Main();
            this.Hide();
            m.Show();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Main m = new Main();
            this.Hide();
            m.Show();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Main m = new Main();
            this.Hide();
            m.Show();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Main m = new Main();
            this.Hide();
            m.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dataGridView1.SelectedRows[0];
            DirEdit d = new DirEdit(1,Convert.ToInt32(row.Cells["Id"].Value),row.Cells["Название"].Value.ToString()); // передаём данные на форму DirEdit (редактирование справочника)
            this.Close();
            d.Show();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            AddDir d = new AddDir(1); // открываем форму добавления справочника (с номером этого справочника)
            this.Close();
            d.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dataGridView2.SelectedRows[0];
            DirEdit d = new DirEdit(2,Convert.ToInt32(row.Cells["ID"].Value), row.Cells["Название"].Value.ToString()); // передаём данные на форму DirEdit
            this.Close();
            d.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dataGridView4.SelectedRows[0];
            DirEdit d = new DirEdit(4, Convert.ToInt32(row.Cells["ID"].Value), row.Cells["Название"].Value.ToString()); // передаём данные на форму DirEdit
            this.Close();
            d.Show();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            AddDir d = new AddDir(2);
            d.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddDir d = new AddDir(4);
            this.Close();
            d.Show();
        }
    }
}
