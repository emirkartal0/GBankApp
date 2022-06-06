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
    public partial class MyAccount : Form
    {
        internal static string customerID { get; set; }
        public static SqlConnection connection = MainPage.connection;
        public MyAccount()
        {
            InitializeComponent();
        }
        private void MyAccount_Load(object sender, EventArgs e)
        {
            button3.BackColor = Color.FromArgb(255, 60, 130,200) ;
            button4.BackColor = Color.FromArgb(255, 60, 130, 200);
            panel1.Visible = false;
            panel1.Enabled = false;
            ListData();
        }
        bool move;
        int mouseX, mouseY;
        private void MyAccount_MouseDown(object sender, MouseEventArgs e)
        {
            move = true;
            mouseX = e.X;
            mouseY = e.Y;
        }

        private void MyAccount_MouseUp(object sender, MouseEventArgs e)
        {
            move = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            button1.BackColor = Color.Pink;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackColor = Color.Transparent;
        }
        private void ListData()
        {
            connection.Open();
            SqlCommand command = new SqlCommand("Select *from Customers Where CustomerID='" + Cryptology.Encrypt(customerID) + "'", connection);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            textBox1.Text = Cryptology.Decrypt(reader["Name"].ToString().Trim()) + Cryptology.Decrypt(reader["Surname"].ToString().Trim());
            textBox5.Text = Cryptology.Decrypt(reader["IdentificationNO"].ToString().Trim());
            textBox2.Text = Cryptology.Decrypt(reader["GmailAccount"].ToString().Trim());
            textBox3.Text = Cryptology.Decrypt(reader["PhoneNumber"].ToString().Trim());
            connection.Close();
        }
        private bool isAvailable(string data)
        {
            if (Regex.IsMatch(data, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
            {
                UpdateInfo("GmailAccount",data);
                return true;
            }
            else if (Regex.IsMatch(data, @"^(0(\d{3})(\d{3})(\d{2})(\d{2}))$"))
            {
                UpdateInfo("PhoneNumber", data);
                return true;
            }
            else
            {
                return false;
            }
        }
        private void UpdateInfo(string stamp,string data)
        {
            string kodMetin = "";
            kodMetin = "Update Customers set " + stamp+"='"+ Cryptology.Encrypt(data) + "' Where CustomerID='" + Cryptology.Encrypt(customerID) + "'";
            connection.Open();
            SqlCommand command = new SqlCommand(kodMetin, connection);
            command.ExecuteNonQuery();
            connection.Close();//telefon doğrulamasında connection kapatılmıyor!!! sebebini bulamadım
            ListData();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox3.Enabled = true;
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (isAvailable(textBox2.Text))
            {
                MessageBox.Show("Update Successsful"); 
                textBox2.Enabled = false;
            }
            else
            {
                MessageBox.Show("Entry Format is not Correct!!");
            }
            ListData();
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {

            if (isAvailable(textBox3.Text))
            {
                MessageBox.Show("Update Successsful");
                textBox3.Enabled = false;
            }
            else
            {
                MessageBox.Show("Entry Format is not Correct!!");
            }
            ListData();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainPage.customerID = customerID;
            MainPage mainPage = new MainPage();
            mainPage.Show();
        }

        private void button5_MouseHover(object sender, EventArgs e)
        {
            button5.BackColor = Color.DarkGray;
        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
            button5.BackColor = Color.Transparent;
        }

        private void MyAccount_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
        }
        private bool CheckPass()
        {//Henüz test edilmedi
            connection.Open();
            SqlCommand command = new SqlCommand("Select Password from Customers Where CustomerID='" + Cryptology.Encrypt(customerID)+"'", connection);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            if ((textBox6.Text.Trim()) == Cryptology.Decrypt(reader["Password"].ToString().TrimEnd()))
            {
                connection.Close();
                return true;
            }
            else
            {
                connection.Close();
                return false;
            }

        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (CheckPass())
                {
                    panel1.Visible = true;
                    panel1.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Wrong Password");
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox7.Text==textBox8.Text&& Regex.IsMatch(textBox7.Text.Trim(), @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,10}$"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Update Customers set Password='" + Cryptology.Encrypt(textBox7.Text.Trim())+"'Where CustomerID='"+Cryptology.Encrypt(customerID)+"' ",connection);
                command.ExecuteNonQuery();
                connection.Close();
                textBox6.Clear();
                textBox7.Clear();
                textBox8.Clear();
                panel1.Visible = false;
                panel1.Enabled = false;
                label10.Visible = false;
                MessageBox.Show("Updated","Successful");
            }
            else
            {
                MessageBox.Show("Password should be 6-10 text lenght and include at least 1 Upper Letter / 1 Lower Letter / 1 Number", "--Reminder--");
                label10.Visible = true;
            }
            ListData();
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                MessageBox.Show("If you have a debt, you can not delete");
                MessageBox.Show("If your account has any money, the money will be confiscated");
                button7.Enabled = true;
            }
            else
            {
                button7.Enabled = false;
            }
        }

        private bool isRemovable()
        {
            connection.Open();
            SqlCommand command = new SqlCommand("Select IsFinished from Loans Where CustomerID='" + Cryptology.Encrypt(customerID) + "'", connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (!Convert.ToBoolean(reader["IsFinished"].ToString()))
                {//isfinished değil 1 ise false döndür
                    connection.Close();
                    return false;
                }
            }
            connection.Close();
            return true;
        }
        private void button7_Click(object sender, EventArgs e)
        {
            if (isRemovable())
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Delete from Customers where CustomerID='"+Cryptology.Encrypt(customerID)+"'",connection);
                command.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Your Account has been deleted","Successful");
                this.Hide();
                Form1 form = new Form1();
                form.Show();
            }
            else
            {
                MessageBox.Show("You have a credit which has not been paid !","Failed");
            }
        }

        private void MyAccount_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {//hareket etkin ise mouse işlevine göre hareket et
                this.SetDesktopLocation(MousePosition.X - mouseX, MousePosition.Y - mouseY);
            }
        }
    }
}
