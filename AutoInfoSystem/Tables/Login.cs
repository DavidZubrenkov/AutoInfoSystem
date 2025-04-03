using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Application = System.Windows.Forms.Application;
using MessageBox = System.Windows.Forms.MessageBox;

namespace AutoInfoSystem
{
    public partial class Login : Form
    {
        int i = 0;
        private Random rand = new Random();
        private Image[] images;
        private string[] CaptchaNames = {"Bw9R","Px5Y","LZ52","VB6O"};
        private int currentImageIndex = -1;
        Connection con2 = new Connection();
        MySqlConnection con;
        public Login()
        {
            con = con2.con1;
            load_images();
            InitializeComponent();
        }
        private void load_images()
        {
            images = new Image[4];
            images[0] = Properties.Resources.captcha1;
            images[1] = Properties.Resources.captcha2;
            images[2] = Properties.Resources.captcha3;
            images[3] = Properties.Resources.captcha4;
        }
        private void ShowRandImage()
        {
            int newIndex;
            do
            {
                newIndex = rand.Next(0, images.Length);
            } while (newIndex == currentImageIndex);
            currentImageIndex = newIndex;
            pictureBox6.Image = images[currentImageIndex];
            textBox3.Clear();
        }
        private void Login_Load(object sender, EventArgs e)
        {
            if(con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            try
            {
         
            }
            catch(Exception ex)
            {
                MessageBox.Show($"{ex}","error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            MakeButtonRounded(button1,20);
            MakeButtonRounded(button2, 20);
            pictureBox1.Image = (Image)Properties.Resources.ResourceManager.GetObject("Login1");
            pictureBox2.Image = (Image)Properties.Resources.ResourceManager.GetObject("Password");
        }
        private void MakeButtonRounded(Button btn, int radius) // делаем кнопку круглой
        {
            int arcSize = radius * 2;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, arcSize, arcSize, 180, 90);
            path.AddLine(radius, 0, btn.Width - radius, 0);
            path.AddArc(btn.Width - arcSize, 0, arcSize, arcSize, 270, 90);
            path.AddLine(btn.Width, radius, btn.Width, btn.Height - radius);
            path.AddArc(btn.Width - arcSize, btn.Height - arcSize, arcSize, arcSize, 0, 90);
            path.AddLine(btn.Width - radius, btn.Height, radius, btn.Height);
            path.AddArc(0, btn.Height - arcSize, arcSize, arcSize, 90, 90);
            path.AddLine(0, btn.Height - radius, 0, radius);
            path.CloseFigure();
            btn.Region = new Region(path);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "admin" && textBox2.Text == "admin")
            {
                ImportBD b = new ImportBD();
                this.Hide();
                b.ShowDialog();
                return;
            }
            try
            {
                if(i == 1)
                {
                    string userInput = textBox3.Text;
                    string correct = CaptchaNames[currentImageIndex];
                    if(userInput == correct)
                    {
                        if (AuthUser(textBox1.Text, HashPassword(textBox2.Text))) // если пароль и логин правильные - входим
                        {
                            if (User.role == 1) // получаем роль пользователя и направляем его на главную форму его роли
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
                        else
                        {

                        }
                    }
                    else
                    {
                        i++
                    }
                }
                else
                {
                    if (AuthUser(textBox1.Text, HashPassword(textBox2.Text))) // если пароль и логин правильные - входим
                    {
                        if (User.role == 1) // получаем роль пользователя и направляем его на главную форму его роли
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
                    else
                    {
                            this.Size = new Size(766, 451);
                            pictureBox5.Location = new Point(731, -8);
                            ShowRandImage();
                            i++;
                        MessageBox.Show("Неверный логин или пароль!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }            
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = !textBox2.UseSystemPasswordChar;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
        }
        private bool AuthUser(string login,string password)
        {
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();
                using (con)
                {
                    using (MySqlCommand com = new MySqlCommand("SELECT Id, RoleId FROM user WHERE Login = @login AND Password = @password", con)) // получаем id роли у пользователя
                    {
                        com.Parameters.AddWithValue("@login", login);
                        com.Parameters.AddWithValue("@password", password);
                        using (MySqlDataReader reader = com.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int userId = reader.GetInt32("Id");
                                User.idEmpl = reader.GetInt32("Id");
                                User.role = reader.GetInt32("RoleId");
                                reader.Close();
                                using (MySqlCommand com1 = new MySqlCommand("SELECT CONCAT(LastName,' ', Name, ' ', COALESCE(Patronyc, '')) FROM employee WHERE UserId = @userId", con))
                                {
                                    com1.Parameters.AddWithValue("@userId", userId);
                                    using (MySqlDataReader read = com1.ExecuteReader())
                                    {
                                        if (read.Read())
                                        {
                                            User.fio = read.GetString(0);
                                            read.Close();
                                        }
                                    }
                                }
                                return true;
                            }
                        }
                        object result = com.ExecuteScalar();
                        if (result != null)
                        {
                            User.role = Convert.ToInt32(result);
                            return true;
                        }
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = !textBox2.UseSystemPasswordChar;
        }
        static string HashPassword(string password) // шифровка пароля
        {
            using(SHA256 sh256 = SHA256.Create())
            {
                byte[] bytes = sh256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach(byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы точно хотите выйти?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question); // подтверждаем добавление

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
            else
            {
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ShowRandImage();
        }
    }
}
