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
    public partial class employees : Form
    {
        Connection con2 = new Connection();
        MySqlConnection con;
        public employees()
        {
            con = con2.con1;
            InitializeComponent();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Main m = new Main();
            this.Hide();
            m.ShowDialog();
        }

        private void employees_Load(object sender, EventArgs e)
        {
            if(con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            string query = @"SELECT employee.id, employee.Name as 'Имя', LastName 'Фамилия', Patronyc 'Отчество', Profession.Name 'Профессия', Login as 'Логин пользователя' FROM db32.employee
                            Left join profession on profession.Id = employee.ProfessionId
                            Left join user on user.Id = employee.UserId;";
            MySqlDataAdapter ad = new MySqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.Columns[0].Visible = false;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddEmployee ea = new AddEmployee();
            this.Hide();
            ea.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Вы точно хотите удалить сотрудника?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    int id = int.Parse(dataGridView1.SelectedCells[0].Value.ToString());
                    using (MySqlCommand com = new MySqlCommand($"Delete from employee where Id = {id}", con))
                    {
                        com.ExecuteNonQuery();
                        MessageBox.Show("Данные удалены", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        updateData();
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
        public void updateData()
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            string query = @"SELECT employee.id, employee.Name as 'Имя', LastName 'Фамилия', Patronyc 'Отчество', Profession.Name 'Профессия', Login as 'Логин пользователя' FROM db32.employee
                            Left join profession on profession.Id = employee.ProfessionId
                            Left join user on user.Id = employee.UserId;";
            MySqlDataAdapter ad = new MySqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.Columns[0].Visible = false;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            int id = int.Parse(dataGridView1.SelectedCells[0].Value.ToString());
            string name = dataGridView1.SelectedCells[1].Value.ToString();
            string lastname = dataGridView1.SelectedCells[2].Value.ToString();
            string patronyc = dataGridView1.SelectedCells[3].Value.ToString();
            string prof = dataGridView1.SelectedCells[4].Value.ToString();
            string login = dataGridView1.SelectedCells[5].Value.ToString();
            Edit.EditEmployee em = new Edit.EditEmployee(id,name,lastname,patronyc,prof,login);
            this.Hide();
            em.ShowDialog();
        }
    }
}
