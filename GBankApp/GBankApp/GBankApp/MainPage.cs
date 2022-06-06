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
    public partial class MainPage : Form
    {
        public double usd, eur, gbp,tl;
        public static SqlConnection connection = Form1.connection;
        internal static string customerID { get; set; }
        public MainPage()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {//Exit button
            Application.Exit();
        }
        bool move;
        int mouseX, mouseY;
        private void MainPage_MouseDown(object sender, MouseEventArgs e)
        {
            move = true;
            mouseX = e.X;
            mouseY = e.Y;
        }
        private void MainPage_MouseUp(object sender, MouseEventArgs e)
        {
            move = false;
        }
        private void MainPage_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                this.SetDesktopLocation(MousePosition.X - mouseX, MousePosition.Y - mouseY);
            }
        }
        private void ReadData()
        {
            connection.Open();
            SqlCommand command = new SqlCommand("Select *from Customers Where CustomerID='" + Cryptology.Encrypt(customerID) + "'", connection);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            textBox1.Text = "    Welcome    " + Cryptology.Decrypt(reader["Name"].ToString().TrimEnd()) + " " + Cryptology.Decrypt(reader["Surname"].ToString().TrimEnd());
            tl = double.Parse(reader["TRY"].ToString().TrimEnd());
            usd = double.Parse(reader["USD"].ToString().TrimEnd());
            eur = double.Parse(reader["EUR"].ToString().TrimEnd());
            gbp = double.Parse(reader["GBP"].ToString().TrimEnd());
            connection.Close();
            label2.Text = tl.ToString() + "-₺";
            label3.Text = usd.ToString() + "-$";
            label4.Text = eur.ToString() + "-€";
            label5.Text = gbp.ToString() + "-£";
        }//ReadData
        private void MainPage_Load(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
            timer1.Enabled = true;
            timer1.Interval = 1000;

            
            pictureBox3.BackgroundImage = ımageList1.Images[14];
            pictureBox8.BackgroundImage = ımageList1.Images[12];
            pictureBox9.BackgroundImage = ımageList1.Images[13];
            pictureBox10.BackgroundImage = ımageList1.Images[10];
            pictureBox11.BackgroundImage = ımageList1.Images[11];
            ReadData();
        }//Load_MainPage

      

        private void pictureBox3_MouseHover(object sender, EventArgs e)
        {//MyAccount Icon
            //pictureBox3.BackColor = Color.Transparent;
            pictureBox3.BackgroundImage = ımageList1.Images[4];
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {//MyAccount Icon
            pictureBox3.BackgroundImage = ımageList1.Images[5];
        }

        private void pictureBox8_MouseHover(object sender, EventArgs e)
        {//ExchangeRates Icon
            pictureBox8.BackgroundImage = ımageList1.Images[1];
        }

        private void pictureBox8_MouseLeave(object sender, EventArgs e)
        {//ExchangeRates Icon
            pictureBox8.BackgroundImage = ımageList1.Images[6];
        }

        private void pictureBox9_MouseHover(object sender, EventArgs e)
        {//Loans Icon
            pictureBox9.BackgroundImage = ımageList1.Images[3];
        }

        private void pictureBox9_MouseLeave(object sender, EventArgs e)
        {//Loans Icon
            pictureBox9.BackgroundImage = ımageList1.Images[7];
        }

        private void pictureBox10_MouseHover(object sender, EventArgs e)
        {//PaymentInvoice Icon
            pictureBox10.BackgroundImage = ımageList1.Images[2];
        }

        private void pictureBox10_MouseLeave(object sender, EventArgs e)
        {//PaymentInvoice Icon
            pictureBox10.BackgroundImage = ımageList1.Images[8];
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {//Döviz işlemleri sayfası
            ExchangeRates.customerID = customerID;
            ExchangeRates exchangeRates = new ExchangeRates();
            exchangeRates.Show();
            this.Hide();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
        }

       

        private void pictureBox10_Click(object sender, EventArgs e)
        {//Fatura ödeme sayfası
            InvoicePayment.customerID = customerID;
            this.Hide();
            InvoicePayment invoicePayment = new InvoicePayment();
            invoicePayment.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //Hesap bilgisi sayfası
            MyAccount.customerID = customerID;
            this.Hide();
            MyAccount myAccount = new MyAccount();
            myAccount.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Döviz işlemleri sayfası
            ExchangeRates.customerID = customerID;
            ExchangeRates exchangeRates = new ExchangeRates();
            exchangeRates.Show();
            this.Hide();
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            //Kredi sayfası
            Loans.customerID = customerID;
            this.Hide();
            Loans exchangeRates = new Loans();
            exchangeRates.Show();
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {//Hesap eharcamalar sayfası
            AccountSummary.customerID = customerID;
            this.Hide();
            AccountSummary summary = new AccountSummary();
            summary.Show();
        }

        private void pictureBox11_MouseHover(object sender, EventArgs e)
        {//AccountSum Icon
            pictureBox11.BackgroundImage = ımageList1.Images[0];
        }

        private void pictureBox11_MouseLeave(object sender, EventArgs e)
        {//AccountSum Icon
            pictureBox11.BackgroundImage = ımageList1.Images[9];
        }

        private void timer1_Tick(object sender, EventArgs e)
        {//Anlık zaman
            DateTime now = DateTime.Now;
            label1.Text = now.ToLongTimeString();
        }
    }//MainPageEnd
}
