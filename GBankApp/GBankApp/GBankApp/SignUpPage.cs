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
using System.Text.RegularExpressions;

namespace GBankApp
{
    public partial class SignUpPage : Form
    {
        SqlConnection connection = Form1.connection;
        public bool typeOfAcc;
        public string verificationCode="";
        public SignUpPage()
        {
            InitializeComponent();
        }

        private void SignUpPage_Load(object sender, EventArgs e)
        {
            radioButton1.Image = ımageList1.Images[1];
            radioButton2.Image = ımageList2.Images[1];
        }
        
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {//retail choice
            if (radioButton1.Checked)
            {
                radioButton1.BackColor = Color.LightGray;
                radioButton1.FlatAppearance.BorderSize = 1;
                radioButton1.Image = ımageList1.Images[0];
            }
            else
            {
                radioButton1.BackColor = Color.Transparent;
                radioButton1.FlatAppearance.BorderSize = 0;
                radioButton1.Image = ımageList1.Images[1];
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {//corporate choice
            if (radioButton2.Checked)
            {
                label5.Text ="Corporate Num :";
                radioButton2.BackColor = Color.LightGray;
                radioButton2.FlatAppearance.BorderSize = 1;
                radioButton2.Image = ımageList2.Images[0];
            }
            else
            {
                label5.Text = "Identification :";
                radioButton2.BackColor = Color.Transparent;
                radioButton2.FlatAppearance.BorderSize = 0;
                radioButton2.Image = ımageList2.Images[1];
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {//retail label
            radioButton1.Checked = true;
            radioButton2.Checked = false;
        }

        private void label2_Click(object sender, EventArgs e)
        {//corporate label
            radioButton2.Checked = true;
            radioButton1.Checked = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {//close button
            Application.Exit();
        }

        bool move;
        int mouseX, mouseY;
        private void SignUpPage_MouseDown(object sender, MouseEventArgs e)
        {
            move = true;
            mouseX = e.X;
            mouseY = e.Y;
        }

        private void SignUpPage_MouseUp(object sender, MouseEventArgs e)
        {
            move = false;
        }

        private void SignUpPage_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                this.SetDesktopLocation(MousePosition.X-mouseX,MousePosition.Y-mouseY);
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {//Privacy agreement checkbox
                if (checkBox1.Checked)
                {
                    button3.Enabled = true;
                }
                else
                {
                    button3.Enabled = false;
                }
        }
        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Name")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }
        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Name";
                textBox1.ForeColor = Color.Silver;
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "Surname")
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "Surname";
                textBox2.ForeColor = Color.Silver;
            }
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            if (textBox3.Text == "Identification Number")
            {
                textBox3.Text = "";
                textBox3.BackColor = Color.Azure;
                textBox3.ForeColor = Color.Black;
            }
        }
        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                textBox3.Text = "Identification Number";
                textBox3.BackColor = Color.Azure;
                textBox3.ForeColor = Color.Silver;
            }
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            if (textBox4.Text == ".....@gmail.com")
            {
                textBox4.Text = "";
                textBox4.BackColor = Color.Azure;
                textBox4.ForeColor = Color.Black;
            }
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            if (textBox4.Text == "")
            {
                textBox4.Text = ".....@gmail.com";
                textBox4.BackColor = Color.Azure;
                textBox4.ForeColor = Color.Silver;
            }
        }

        private void textBox5_Enter(object sender, EventArgs e)
        {
            if (textBox5.Text == "0(___)_______")
            {
                textBox5.Text = "";
                textBox5.BackColor = Color.Azure;
                textBox5.ForeColor = Color.Black;
            }
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            if (textBox5.Text == "")
            {
                textBox5.Text = "0(___)_______";
                textBox5.BackColor = Color.Azure;
                textBox5.ForeColor = Color.Silver;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {//Privacy agreement pdf
            PrivacyPolicy privacyPolicy = new PrivacyPolicy();
            privacyPolicy.Show();
        }
        bool isGmailRight = false, isPhoneRight = false, isTcRight = false, isPassRight = false;
        
        public void checkTheRegex()
        {
            if (Regex.IsMatch(textBox4.Text, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
            {
                   isGmailRight = true;
            }
            else
            {
                isGmailRight = false;
                MessageBox.Show("Uncompatible Gmail Format!!!");
                textBox4.Clear();
                textBox4.BorderStyle = BorderStyle.FixedSingle;
                textBox4.BackColor = Color.LightPink;
                textBox4.ForeColor = Color.Black;
            }
            if (Regex.IsMatch(textBox5.Text, @"^(0(\d{3})(\d{3})(\d{2})(\d{2}))$"))
            {
                isPhoneRight = true;
            }
            else
            {
                isPhoneRight = false;
                MessageBox.Show("Uncompatible Phone Format!!!");
                textBox5.Clear();
                textBox5.BorderStyle = BorderStyle.FixedSingle;
                textBox5.BackColor = Color.LightPink;
                textBox5.ForeColor = Color.Black;
            }
            if (Regex.IsMatch(textBox3.Text, @"(\d{11})$"))
            {
                isTcRight = true;
            }
            else
            {
                isTcRight = false;
                MessageBox.Show("Uncompatible TC Format!!!");
                textBox3.Clear();
                textBox3.BorderStyle = BorderStyle.FixedSingle;
                textBox3.BackColor = Color.LightPink;
                textBox3.ForeColor = Color.Black;
            }
            if (Regex.IsMatch(textBox7.Text.Trim(), @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,10}$"))
            {// at least 1 upper/1 lower/1 number
                isPassRight = true;
            }
            else
            {
                isPassRight = false;
                MessageBox.Show("Uncompatible Password Format!!!");
                MessageBox.Show("Password should be 6-10 text lenght and include at least 1 Upper Letter / 1 Lower Letter / 1 Number","--Reminder--");
                textBox7.Clear();
                textBox7.BorderStyle = BorderStyle.FixedSingle;
                textBox7.BackColor = Color.LightPink;
                textBox7.ForeColor = Color.Black;
            }
        }
        private bool isAvailable()
        {
            connection.Open();
            SqlCommand commandProcess = new SqlCommand("Select IdentificationNO from Customers", connection);
            SqlDataReader reader = commandProcess.ExecuteReader();
            while (reader.Read())
            {
                if (Cryptology.Encrypt(textBox3.Text.Trim())==reader["IdentificationNO"].ToString().Trim())
                {
                    MessageBox.Show("This Identification number has an account","Failed");
                    connection.Close();
                    return false;
                }
            }
            connection.Close();
            return true;
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
            verificationCode = "a1w3f3";
            MailSender mail = new MailSender("Creating an Account", "Your Verification Code= " + verificationCode, textBox4.Text.Trim());
            textBox2.Enabled = true;
            button4.Enabled = true;
        }
        private void button3_Click(object sender, EventArgs e)
        {//sign-up button
            checkTheRegex();//Checking Format of text for Tc-PhoneNum-Gmail 
            if (isTcRight&&isPhoneRight&&isGmailRight&&isPassRight&&isAvailable())
            {
                if (radioButton1.Checked)
                {
                    typeOfAcc = true;
                }
                else
                {
                    typeOfAcc = false;
                }
                if (textBox6.Text.Trim()==verificationCode)
                {
                    connection.Open();
                    string customerID = textBox3.Text.Substring(6);
                    SqlCommand command = new SqlCommand("Insert into Customers (Name,Surname,IdentificationNO,GmailAccount,PhoneNumber,TRY,USD,GBP,EUR,CustomerID,Password,TypeOfAcc) values ('" + Cryptology.Encrypt(textBox1.Text) + "','" + Cryptology.Encrypt(textBox2.Text) + "','" + Cryptology.Encrypt(textBox3.Text) + "','" + Cryptology.Encrypt(textBox4.Text) + "','" + Cryptology.Encrypt(textBox5.Text) + "','" + 500 + "','" + 0 + "','" + 0 + "','" + 0 + "','" + Cryptology.Encrypt(customerID) + "','" + Cryptology.Encrypt(textBox7.Text) + "','" + typeOfAcc + "')", connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Your CustomerID is last 5 number of your Personal Identification", "Don't Forget");
                    MessageBox.Show("Successfull", "Sign Up");
                    Form1 form1 = new Form1();
                    this.Close();
                    form1.Show();
                }
                else
                {
                    MessageBox.Show("Verification Code could not be matched up");
                }
                
            }
            else
            {
                MessageBox.Show("Failed","Sign Up");
            }
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {//Close button
            button1.BackColor = Color.Pink;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {//Close button
            button1.BackColor = Color.Transparent;
        }

        private void button5_Click(object sender, EventArgs e)
        { //Return back button
            Form1 form1 = new Form1();
            this.Close();
            form1.Show();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            move = true;
            mouseX = e.X;
            mouseY = e.Y;
            connection.Open();
            SqlCommand command = new SqlCommand("Insert into Customers (Name,Surname) values ('" +textBox1.Text + "','" +textBox2.Text + "')", connection);
            command.ExecuteNonQuery();
            connection.Close();
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            move = false;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                this.SetDesktopLocation(MousePosition.X - mouseX, MousePosition.Y - mouseY);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {//verify button
            checkBox1.Enabled = true;
            textBox6.Enabled = true;
            label8.Enabled = true;
            SendVerification();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //silme_silme_ReturnBack button
        }
    }
}
