using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoInfoSystem.Tables
{
    public partial class AllDataEmployee : Form
    {
        private string name1;
        private string lastname1;
        private string patronyc1;
        private string prof1;
        private string userid1;
        private string telephone1;
        public AllDataEmployee(string name,string lastname,string patronyc,string prof,string user,string telephone)
        {
            telephone1 = telephone;
            name1 = name;
            lastname1 = lastname;
            patronyc1 = patronyc;
            prof1 = prof;
            userid1 = user;
            InitializeComponent();
        }

        private void AllDataEmployee_Load(object sender, EventArgs e)
        {
            textBox5.Text = name1;
            textBox4.Text = lastname1;
            textBox1.Text = patronyc1;
            textBox2.Text = prof1;
            textBox3.Text = userid1;
            maskedTextBox1.Text = telephone1;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            employees em = new employees();
            this.Hide();
            em.ShowDialog();
        }
    }
}
