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

namespace GBankApp
{
    public partial class Form1 : Form
    {
        public static SqlConnection connection = new SqlConnection("Data Source=DESKTOP-MP4S6AS;Initial Catalog=GBankData; Integrated Security=TRUE");
        //Yerel data base ismini yaz
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {//Çıkış butonu işlevi
            Application.Exit();
        }

        bool move;
        int mouseX, mouseY;
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {//Pencereye tıkladığında hareket
            move = true;
            mouseX = e.X;
            mouseY = e.Y;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {//tıklamayı bıraktığında hareket
            move = false;
        }

        private void button2_MouseHover(object sender, EventArgs e)
        {//login butonu hover
            button2.BackColor = Color.NavajoWhite;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {//login butonu leave
            button2.BackColor = Color.Transparent;
        }

      

        private void Form1_Load(object sender, EventArgs e)
        {//Form1 çalışınca gerçekleşir
         //this.BackgroundImage = ımageList3.Images[0];
         //textBox1.size;
            timer1.Interval = 3000;
            timer1.Enabled = true;//timer 3 sn olarak aktif
            checkBox1.Enabled = false;//Şifreyi görme devre dışı
            pictureBox1.Image = ımageList1.Images[0];//başlangıç reklam
        }
        int i = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {//her belirlenen saniyede imagelist1 i dolaş
            i++;
            if (i==3)
            {
                i = 0;
            }
            pictureBox1.Image = ımageList1.Images[i];
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text=="Customer Number")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Customer Number";
                textBox1.ForeColor = Color.Silver;

            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "Password")
            {
                //şifre girilince
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
                textBox2.UseSystemPasswordChar = true;
                checkBox1.Enabled = true;
                checkBox1.Checked = true;
                
            }
        }
        
        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                //şifre boş bırakılırsa
                textBox2.Text = "Password";//silik password
                textBox2.ForeColor = Color.Silver;
                textBox2.UseSystemPasswordChar = false; // şifreleme kapalı
                checkBox1.Checked = false; // checkbox devre dışı
                checkBox1.Enabled = false;
                
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {//şifre görme tıklanırsa image değiş
            if (checkBox1.Checked)
            {
                textBox2.UseSystemPasswordChar = true;
                checkBox1.BackgroundImage = ımageList2.Images[0];
                //Şifreleme etkin
            }
            else
            {
                textBox2.UseSystemPasswordChar = false;
                checkBox1.BackgroundImage = ımageList2.Images[1];
                checkBox1.BackColor = Color.FromArgb(53, 66, 89);
                //şifreleme devre dışı
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {// open signup yazısı
            

        }
        bool isLoggedIn;
        private void button2_Click(object sender, EventArgs e)
        {//login button
            string customerID = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();
            isLoggedIn = true;
            connection.Open();
            SqlCommand command = new SqlCommand("Select *from Customers ", connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (customerID == Cryptology.Decrypt(reader["CustomerID"].ToString().TrimEnd()) && password == Cryptology.Decrypt(reader["Password"].ToString().TrimEnd()))
                {//doğru olan bulununca mainpage formunun name deişkenine ata
                    isLoggedIn = true;
                    MainPage.customerID = customerID;
                    break;//sağlandı mı durdur okumayı
                }
                else
                {
                    isLoggedIn = false;
                }
            }
            connection.Close();
            if (isLoggedIn)
            {
                this.Hide();
                MainPage mainPage = new MainPage();
                mainPage.Show();
            }
            else
            {
                MessageBox.Show("Customer Number or Password is incorrect","Failed");
                textBox1.Clear();
                textBox2.Clear();
            }
        }//login button

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                textBox2.Focus();
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button2.Focus();
            }
        }

   
        private void label1_Click(object sender, EventArgs e)
        {
            this.Hide();
            ForgottenPass forgotten = new ForgottenPass();
            forgotten.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SignUpPage signUpPage = new SignUpPage();
            this.Hide();
            signUpPage.Show();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {//hareket etkin ise mouse işlevine göre hareket et
                this.SetDesktopLocation(MousePosition.X - mouseX, MousePosition.Y - mouseY);
            }
            
        }
    }
}
