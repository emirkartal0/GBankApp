using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GBankApp
{
    public partial class ForgottenPass : Form
    {
        public static SqlConnection connection = Form1.connection;
        public string verificationCode = "";
        internal static string customerID { get; set; }

        public ForgottenPass()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form = new Form1();
            form.Show();
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            button1.BackColor = Color.Pink;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackColor = Color.Transparent;
        }

        private void button3_MouseHover(object sender, EventArgs e)
        {
            button3.BackColor = Color.DarkGray;
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            button3.BackColor = Color.Transparent;
        }

        int mouseX, mouseY;
        bool move;
        private void ForgottenPass_MouseDown(object sender, MouseEventArgs e)
        {
            move = true;
            mouseX = e.X;
            mouseY = e.Y;
        }

        private void ForgottenPass_MouseUp(object sender, MouseEventArgs e)
        {
            move = false;
        }


        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void SendVerification()
        {
            Random rnd = new Random();
            Random rndDigit = new Random();
            for (int i = 0; i < 3; i++)
            {
                verificationCode += (char)rnd.Next('a', 'z');
                verificationCode += (int)rndDigit.Next(0, 10);
            }
            MailSender mail = new MailSender("Password Change","Your Verification Code= "+verificationCode,textBox1.Text.Trim());
            textBox2.Enabled = true;
            button4.Enabled = true;
        }
        private bool CheckGmail()
        {
            if (Regex.IsMatch(textBox1.Text, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
            {
                return true;
            }
            else
            {
                MessageBox.Show("Uncompatible Gmail Format!!!");
                textBox1.Clear();
                textBox1.BorderStyle = BorderStyle.FixedSingle;
                textBox1.BackColor = Color.LightPink;
                textBox1.ForeColor = Color.Black;
                return  false;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (CheckGmail())
            {
                bool a = false;
                connection.Open();
                SqlCommand command = new SqlCommand("Select GmailAccount from Customers ", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["GmailAccount"].ToString().Trim() == Cryptology.Encrypt(textBox1.Text.Trim()))
                    {
                        a = true;
                        break;
                    }
                }
                if (a)
                {
                    SendVerification();
                }
                else
                {
                    MessageBox.Show("This Account could not Found","Error!!");
                }
                connection.Close();
            }
        }
        private bool CheckPass()
        {
            if (Regex.IsMatch(textBox3.Text.Trim(), @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,10}$"))
            {// at least 1 upper/1 lower/1 number
                return true;
            }
            else
            {
                
                MessageBox.Show("Uncompatible Password Format!!!");
                MessageBox.Show("Password should be 6-10 text lenght and include at least 1 Upper Letter / 1 Lower Letter / 1 Number", "--Reminder--");
                textBox3.Clear();
                textBox3.BorderStyle = BorderStyle.FixedSingle;
                textBox3.BackColor = Color.LightPink;
                textBox3.ForeColor = Color.Black;
                return false;
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox3.Text.Trim()==textBox4.Text.Trim())
            {
                if (CheckPass())
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("Update Customers set Password='"+Cryptology.Encrypt(textBox3.Text.Trim())+"' ", connection);
                    MessageBox.Show("Password Updated","Successful");
                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("Password do not match");
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (textBox2.Text==verificationCode)
            {
                panel2.Visible = true;
            }
            else
            {
                MessageBox.Show("You Type Verification Code Improperly","Error!!");
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ForgottenPass_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                this.SetDesktopLocation(MousePosition.X-mouseX,MousePosition.Y-mouseY);
            }
        }

    }
}
