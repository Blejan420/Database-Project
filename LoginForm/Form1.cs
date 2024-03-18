using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing.Imaging;

namespace LoginForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {   
            InitializeComponent();
            this.BackgroundImage = Image.FromFile(@"W:\repositories\LoginForm\LoginForm\download.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        SqlConnection con = new SqlConnection();
        SqlCommand cmd;
        SqlDataReader rdr;
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtusername.Text))
            {
                MessageBox.Show("Enter the username");
                txtusername.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtpassword.Text))
            {
                MessageBox.Show("Enter the password");
                txtpassword.Focus();
                return;
            }   
            try
            {
                con = new SqlConnection("Data Source=ANDREI\\SQLEXPRESS; Initial Catalog=cabinet_veterinar; Integrated Security=True");
                con.Open();
                cmd = new SqlCommand("select * from users where username = '" + txtusername.Text + "' and PASSWORD = '" + txtpassword.Text + "'");
                cmd.Connection = con;
                rdr= cmd.ExecuteReader();
                if(rdr.Read())
                {
                    Form2 form2 = new Form2();
                    form2.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Enter a valid username and password!");
                    txtusername.Text = "";
                    txtpassword.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtusername_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
