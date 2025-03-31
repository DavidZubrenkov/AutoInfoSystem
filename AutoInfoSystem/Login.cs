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
        public string connection = "server=localhost;uid=root;pwd=;database=db32;";
        public Login()
        {
            InitializeComponent();
        }
        private void Login_Load(object sender, EventArgs e)
        {
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
        private void MakeButtonRounded(Button btn, int radius)
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

            if (AuthUser(textBox1.Text, HashPassword(textBox2.Text)))
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
                else if(User.role == 3)
                {
                    MainSM m = new MainSM();
                    this.Hide();
                    m.ShowDialog();
                }

            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!","ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
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
            using (MySqlConnection con = new MySqlConnection(connection))
            {
                using(MySqlCommand com = new MySqlCommand("SELECT RoleId FROM user WHERE Login = @login AND Password = @password",con))
                {
                    com.Parameters.AddWithValue("@login", login);
                    com.Parameters.AddWithValue("@password", password);
                    con.Open();
                    object result = com.ExecuteScalar();
                    if(result != null)
                    {
                        User.role = Convert.ToInt32(result);
                        return true;
                    }
                    return false;
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = !textBox2.UseSystemPasswordChar;
        }
        static string HashPassword(string password)
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
            Application.Exit();
        }
    }
}
