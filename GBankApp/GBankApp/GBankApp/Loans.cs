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
    public partial class Loans : Form
    {
        public static SqlConnection connection = MainPage.connection;
        public string loanName,mailAcc;//principal=anapara
        public double bankCharge = 0,instalment=0,principal=0,interestRate=0,expiry=0,loanBalance=0;
        public bool typeOfAcc;
        public double tl;

        internal static string customerID { get; set; }
        public Loans()
        {
            InitializeComponent();
        }
        bool move;
        int mouseX, mouseY;
        private void Loans_MouseDown(object sender, MouseEventArgs e)
        {
            move = true;
            mouseX = e.X;
            mouseY = e.Y;
        }

        private void Loans_MouseUp(object sender, MouseEventArgs e)
        {
            move = false;
        }
        private void setMinMax(string typeOfLoan)
        {
            if (typeOfLoan == "Personal Loan")
            {
                label14.Text = "Max 60 Month";
                label13.Text ="Min 10.000 TL";
                if (double.Parse(textBox1.Text) < 10000)
                {
                    textBox1.Text = "10000";
                }
                if (double.Parse(textBox2.Text) > 60)
                {
                    textBox2.Text = "36";
                }
            }
            else if (typeOfLoan == "Subordinated Loan")
            {
                label14.Text = "Max 120 Month";
                label13.Text = "Min 50.000 TL";
                if (double.Parse(textBox1.Text) < 50000)
                {
                    textBox1.Text = "50000";
                }
                if (double.Parse(textBox2.Text) > 120)
                {
                    textBox2.Text = "48";
                }
            }
            else if (typeOfLoan == "Export Loan")
            {
                label14.Text = "Max 200 Month";
                label13.Text = "Min 100.000 TL";
                if (double.Parse(textBox1.Text) < 100000)
                {
                    textBox1.Text = "100000";
                }
                if (double.Parse(textBox2.Text) > 200)
                {
                    textBox2.Text = "60";
                }
            }
            else if (typeOfLoan == "Housing Loan")
            {
                label14.Text = "Max 120 Month";
                label13.Text = "Min 100.000 TL";
                if (double.Parse(textBox1.Text) < 100000)
                {
                    textBox1.Text = "100000";
                }
                if (double.Parse(textBox2.Text) > 120)
                {
                    textBox2.Text = "36";
                }
            }
        }
        private void calculateValue()
        {//faizli tutar hesaplanması
            principal = bankCharge+(loanBalance * Math.Pow(1+interestRate,expiry));
            instalment = principal / expiry;
            principal = Math.Round(principal,4);
            instalment = Math.Round(instalment,4);
        }
        private void setExplanations(string typeOfLoan)
        {
            setMinMax(typeOfLoan);
            if (typeOfAcc)
            {//Bireysel müşteriler için
                if (typeOfLoan == "Personal Loan")
                {
                    interestRate = 0.0141;
                    bankCharge = 370;
                }
                else if (typeOfLoan == "Subordinated Loan")
                {
                    interestRate = 0.0356;
                    bankCharge = 710;
                }
                else if (typeOfLoan == "Export Loan")
                {
                    interestRate = 0.031;
                    bankCharge = 3500;
                }
                else if (typeOfLoan == "Housing Loan")
                {
                    interestRate = 0.00790;
                    bankCharge = 200;
                }
            }
            else
            {//Kurumsal müşteriler için
                if (typeOfLoan == "Personal Loan")
                {
                    interestRate = 0.0231;
                    bankCharge = 370;
                }
                else if (typeOfLoan == "Subordinated Loan")
                {
                    interestRate = 0.0220;
                    bankCharge = 120;
                }
                else if (typeOfLoan == "Export Loan")
                {
                    interestRate = 0.0150;
                    bankCharge = 1800;
                }
                else if (typeOfLoan == "Housing Loan")
                {
                    interestRate = 0.0220;
                    bankCharge = 750;
                }
            }
            calculateValue();
        }//SetExplanations
        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            label7.Text = now.ToLongTimeString();
            loanName = comboBox1.Text;
            if (textBox1.Text=="")
            {//boş değer hata verir
                textBox1.Text = "0";
            }
            if (textBox1.Text == "")
            {
                textBox1.Text = "1";
            }
            setExplanations(loanName);
            loanBalance = double.Parse(textBox1.Text);
            expiry = double.Parse(textBox2.Text);
            label12.Text = bankCharge.ToString();
            label11.Text = "%"+(interestRate*100).ToString();
            label10.Text = instalment.ToString();
            label9.Text = principal.ToString();//toplam para
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                button4.Enabled = true;
            }
            else
            {
                button4.Enabled = false;
            }
        }
        private bool isAvailable()
        {//Kullanıcının aynı isimde mevcut kredisi var mı?
            bool available=true;
            connection.Open();  
            SqlCommand commandCredit = new SqlCommand("Select * from Loans Where CustomerID='" + Cryptology.Encrypt(customerID) + "' and IsFinished='"+0+"'", connection);
            SqlDataReader reader = commandCredit.ExecuteReader();
            while (reader.Read())
            {
                if (comboBox1.Text.Trim() == reader["TypeOfLoan"].ToString().Trim())
                {
                    available= false;
                    break;
                }
                else 
                {
                    available= true;
                }
            }
            connection.Close();
            return available;
        }
        private void ReadData()
        {
            connection.Open();
            SqlCommand command = new SqlCommand("Select TRY, TypeOfAcc,GmailAccount from Customers Where CustomerID='" + Cryptology.Encrypt(customerID) + "'", connection);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            tl = double.Parse(reader["TRY"].ToString().Trim());
            typeOfAcc = Convert.ToBoolean(reader["TypeOfAcc"].ToString().Trim());
            mailAcc = Cryptology.Decrypt(reader["GmailAccount"].ToString().Trim());
            connection.Close();
        }
        private void InsertSum()
        {
            DateTime date = DateTime.Now;
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
            connection.Open();
            SqlCommand commandProcess = new SqlCommand("Insert into Process (CustomerID,InvoiceID,Task,Amount,Date,IsCredit) values ('" + customerID + "','" + comboBox1.Text.Trim() + "','" + "Getting" + "','" + (+Convert.ToDouble(textBox1.Text.Trim())) + "','" + date.ToLocalTime().ToString() + "','" + 1 + "')", connection);
            commandProcess.ExecuteNonQuery();
            connection.Close();
        }
        private void button4_Click(object sender, EventArgs e)
        {//uygunsa krediyi kaydet
            ReadData();
            if (isAvailable())
            {
                System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
                customCulture.NumberFormat.NumberDecimalSeparator = ".";
                System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
                DateTime date = DateTime.Now;
                connection.Open();
                SqlCommand commandCredit = new SqlCommand("Insert into Loans (CustomerID,TypeOfLoan,Instalment,Amount,Month,Expiry,IsFinished,LastProcessDate,Date) values ('"+Cryptology.Encrypt(customerID)+ "','" + loanName.Trim() + "','" + instalment + "','" + principal + "','" + 0 + "','" + expiry + "','" + 0 + "','"+ date.ToLocalTime().ToString() + "','" + date.ToLocalTime().ToString() + "')", connection);
                commandCredit.ExecuteNonQuery();
                connection.Close();
                connection.Open();
                SqlCommand command = new SqlCommand("Update Customers set TRY='" + (tl + loanBalance) + "' Where CustomerID='" + Cryptology.Encrypt(customerID) + "'", connection);
                command.ExecuteNonQuery();
                connection.Close();
                InsertSum();
                string mailText = "Congratulations!! You get "+loanBalance+" "+loanName.Trim()+". The money is inserted to your balance.         "+date.ToLocalTime().ToString();
                MailSender mail = new MailSender("Getting Credit",mailText,mailAcc);
                MessageBox.Show(loanBalance+"TL inserted your balance");
                checkBox1.Checked = false;
                button4.Enabled = false;
                textBox1.Text = "0";
                textBox2.Text = "1";
            }
            else
            {
                MessageBox.Show("You can not get same credit at the same time","Warning!!");
            }
            ReadData();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            move = true;
            mouseX = e.X;
            mouseY = e.Y;
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            move = false;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                this.SetDesktopLocation(MousePosition.X - mouseX, MousePosition.Y - mouseY);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Click(object sender, EventArgs e)
        {
            MyLoans.customerID = customerID;
            this.Hide();
            MyLoans myLoans = new MyLoans();
            myLoans.Show();
        }

        private void Loans_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                this.SetDesktopLocation(MousePosition.X - mouseX, MousePosition.Y - mouseY);
            }
        }

        private void Loans_Load(object sender, EventArgs e)
        {
            ReadData();
            //label8.BackColor= Color.FromArgb(255, 41, 104, 158);
            button4.Enabled = false;
            timer1.Enabled = true;
            timer1.Interval = 2000;
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            button1.BackColor = Color.Pink;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackColor = Color.Transparent;
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainPage.customerID = customerID;
            MainPage mainPage = new MainPage();
            mainPage.Show();
        }
    }
}
