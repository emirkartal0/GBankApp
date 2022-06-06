using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GBankApp
{
    public partial class InvoicePayment : Form
    {
        internal static string customerID { get; set; }
        public double tl,total,charge;
        public static SqlConnection connection = MainPage.connection;
        
        public InvoicePayment()
        {
            InitializeComponent();
        }

        bool move;
        int mouseX, mouseY;
        private void InvoicePayment_MouseDown(object sender, MouseEventArgs e)
        {
            move = true;
            mouseX = e.X;
            mouseY = e.Y;
        }

        private void InvoicePayment_MouseUp(object sender, MouseEventArgs e)
        {
            move = false;
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            button1.BackColor = Color.Pink;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackColor = Color.Transparent;
        }

        private void button2_MouseHover(object sender, EventArgs e)
        {
            button2.BackColor = Color.DarkGray;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.BackColor = Color.Transparent;
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainPage.customerID = customerID;
            MainPage mainPage = new MainPage();
            mainPage.Show();
        }
        private void ReadData()
        {
            connection.Open();
            SqlCommand command = new SqlCommand("Select *from Customers Where CustomerID='" + Cryptology.Encrypt(customerID) + "'", connection);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            tl = Convert.ToDouble(reader["TRY"].ToString().TrimEnd());
            label11.Text = "Welcome: " + Cryptology.Decrypt(reader["Name"].ToString().TrimEnd()) + "      Balance: " + tl + " TL";
            connection.Close();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (textBox2.Text=="")
            {
                textBox2.Text = "0";
            }
            if (comboBox1.Text.Trim() == "Tax")
            {
                charge= 12.5;
            }
            else if (comboBox1.Text.Trim() == "Water" || comboBox1.Text.Trim() == "Electric")
            {
                charge= 3.5;
            }
            else if (comboBox1.Text.Trim() == "Customs")
            {
                charge= Convert.ToDouble(textBox2.Text.Trim()) * 0.12;
            }
            else if (comboBox1.Text.Trim() == "Copayment")
            {
                charge = 128.8;
            }
            else
            {
                charge = Convert.ToDouble(textBox2.Text.Trim())*0.20;
            }
            total = Convert.ToDouble(textBox2.Text.Trim()) + charge;
            textBox3.Text = charge.ToString();
            textBox4.Text = total.ToString();
        }

        private void InvoicePayment_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Interval = 3000;
            groupBox2.Visible = false;
            label10.Visible = false;
            ReadData();
        }
        private bool CheckTotal()
        {
            if (total<=tl) {
                return true;
            }
            else 
            {
                return false; 
            }
            
        }
        private bool isAvailable()
        {
            connection.Open();
            SqlCommand commandProcess = new SqlCommand("Select InvoiceID from Process", connection);
            SqlDataReader reader = commandProcess.ExecuteReader();
            while (reader.Read())
            {
                if (textBox1.Text.Trim() == reader["InvoiceID"].ToString().Trim())
                {
                    connection.Close();
                    return  false;
                    //break;//sağlandı mı durdur okumayı
                }
            }
            connection.Close();
            return true;
        }
        private void LastProcess()
        {
            DateTime date = DateTime.Now;
            groupBox2.Visible = true;
            label6.Text = comboBox1.Text.Trim()+" Invoice";
            label7.Text = textBox1.Text.Trim();
            label8.Text = "Paid invoice = " + total;
            label9.Text = date.ToLocalTime().ToString();
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            if (CheckTotal() && isAvailable())
            {
                System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
                customCulture.NumberFormat.NumberDecimalSeparator = ".";
                System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
                connection.Open();
                SqlCommand command = new SqlCommand("Update Customers set TRY='" + (tl - Convert.ToDouble(textBox2.Text.Trim())) + "' Where CustomerID='" + Cryptology.Encrypt(customerID) + "'", connection);
                command.ExecuteNonQuery();
                connection.Close();
                connection.Open();
                SqlCommand commandProcess = new SqlCommand("Insert into Process (CustomerID,InvoiceID,Task,Amount,Date,IsCredit) values ('"+customerID+ "','" + textBox1.Text.Trim() + "','" + comboBox1.Text.Trim() + "','" + (-Convert.ToDouble(textBox2.Text.Trim())) + "','" + date.ToLocalTime().ToString() + "','" + 0 + "')", connection);
                commandProcess.ExecuteNonQuery();//girilen değer küsüratlı ise hata veriyor
                connection.Close();
                LastProcess();
                comboBox1.Text = "";textBox1.Text = "";textBox2.Text = "";
                MessageBox.Show("Payment Successful","Accepted");
                label10.Visible = false;
            }
            else
            {
                label10.Visible = true;
                label10.Text = "Balance is not enough or the invoice has been paid";
            }
            ReadData();
        }
        private void InvoicePayment_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                this.SetDesktopLocation(MousePosition.X - mouseX, MousePosition.Y - mouseY);
            }
        }
    }
}
