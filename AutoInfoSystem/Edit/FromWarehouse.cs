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
    public partial class FromWarehouse : Form
    {
        private int id1;
        public FromWarehouse(int id)
        {
            con = con2.con1;
            id1 = id;
            InitializeComponent();
        }
        Connection con2 = new Connection();
        MySqlConnection con;
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            WareHouse w = new WareHouse();
            this.Hide();
            w.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int numb = int.Parse(textBox1.Text);
                if (numb <= 0)
                {
                    MessageBox.Show("Введено недопустимое значение", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                using (MySqlCommand com = new MySqlCommand($"Select AmountInStock from warehouse where Id = {id1}", con))
                {
                    using (MySqlDataReader read = com.ExecuteReader())
                    {
                        while (read.Read())
                        {
                            if (numb > int.Parse(read[0].ToString()))
                            {
                                MessageBox.Show("Запрашиваемое количество превышает допустимое", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            numb = int.Parse(read[0].ToString()) - numb;
                        }
                        read.Close();
                        if (numb > 0)
                        {
                            using (MySqlCommand com1 = new MySqlCommand($"Update warehouse Set AmountInStock = @numb where id = {id1}", con))
                            {
                                com1.Parameters.AddWithValue("@numb", numb);
                                com1.ExecuteNonQuery();
                                MessageBox.Show("Препараты успешно взяты со склада", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                WareHouse w = new WareHouse();
                                this.Hide();
                                w.Show();
                            }
                        }
                        else
                        {
                            using (MySqlCommand com2 = new MySqlCommand($"Delete from warehouse where id = {id1}", con))
                            {
                                com2.ExecuteNonQuery();
                                MessageBox.Show("Препараты успешно взяты со склада", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                WareHouse w = new WareHouse();
                                this.Hide();
                                w.Show();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка!\n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FromWarehouse_Load(object sender, EventArgs e)
        {
            if(con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && ((char)Keys.Back) != e.KeyChar)
            {
                e.Handled = true;
            }
        }
    }
}
