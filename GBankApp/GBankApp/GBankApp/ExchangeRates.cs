using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;//Giriş Çıkış işlemleri
using System.Net;
using System.Xml;
using System.Data.SqlClient;
using System.Globalization;

namespace GBankApp
{
    public partial class ExchangeRates : Form
    {
        internal static string customerID { get; set; }
        public double usd, eur, gbp, tl;
        public double usdSelling, eurSelling, gbpSelling, tlSelling;
        public double usdBuying, eurBuying, gbpBuying, tlBuying;
        public static SqlConnection connection = MainPage.connection;
        public ExchangeRates()
        {
            InitializeComponent();
        }
        bool move;
        int mouseX, mouseY;
        private void ExchangeRates_MouseDown(object sender, MouseEventArgs e)
        {
            move = true;
            mouseX = e.X;
            mouseY = e.Y;
        }

        private void ExchangeRates_MouseLeave(object sender, EventArgs e)
        {
            
        }

        private void ExchangeRates_MouseUp(object sender, MouseEventArgs e)
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
        private void ExchangeBuy(string currency,double amount)
        {
            bool isAvailable=false;
            string kodMetin="";
            double result = 0;
            if (currency == "USD")
            {
                MessageBox.Show((usdSelling).ToString());
                MessageBox.Show("usd selling" +usdSelling.ToString());
                MessageBox.Show(tl.ToString()); 
                kodMetin = "Update Customers set " + currency + "='" + (usd + amount) + "',TRY='" + Math.Round(tl - (amount * usdSelling),4) + "' Where CustomerID='" + Cryptology.Encrypt(customerID) + "'";
                if (amount * usdSelling <= tl)
                {
                    isAvailable = true;
                }
                result = amount * usdSelling;
            }
            else if (currency == "EUR")
            {
                kodMetin = "Update Customers set " + currency + "='" + (eur + amount) + "',TRY='" + Math.Round(tl - (amount * eurSelling), 4) + "' Where CustomerID='" + Cryptology.Encrypt(customerID) + "'";
                if (amount * eurSelling <= tl)
                {
                    isAvailable = true;
                }
                result = amount*eurSelling;
            }
            else if (currency == "GBP")
            {
                kodMetin = "Update Customers set " + currency + "='" + (gbp + amount) + "',TRY='" + Math.Round(tl - (amount * gbpSelling), 4) + "' Where CustomerID='" + Cryptology.Encrypt(customerID) + "'";
                if (amount * gbpSelling <= tl)
                {
                    isAvailable = true;
                }
                result = amount * gbpSelling;
            }
            if (isAvailable)
            {
                //MessageBox.Show(gbpSelling.ToString());
                //MessageBox.Show(kodMetin+"sonucs");
                connection.Open();
                SqlCommand command = new SqlCommand(kodMetin, connection);
                command.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Count : "+result,"Successful");
            }
            else
            {
                MessageBox.Show("Not Enough Balance");
            }
            ListData();
            
        }//ExchangeBuying
        private void ExchangeSell(string currency, double amount)
        {
            bool isAvailable = false;
            string kodMetin = "";
            double result = 0;
            if (currency == "USD")
            {
                kodMetin = "Update Customers set " + currency + "='" + (usd - amount) + "',TRY='" + Math.Round(tl + (amount * usdBuying), 4) + "' Where CustomerID='" + Cryptology.Encrypt(customerID) + "'";
                if (usd>=amount)
                {
                    isAvailable = true;
                    result = amount * usdBuying;
                }
            }
            else if (currency == "EUR")
            {
                kodMetin = "Update Customers set " + currency + "='" + (eur - amount) + "',TRY='" + Math.Round(tl + (amount * eurBuying), 4) + "' Where CustomerID='" + Cryptology.Encrypt(customerID) + "'";
                if (eur>=amount)
                {
                    isAvailable = true;
                    result = amount *eurBuying;
                }
            }
            else if (currency == "GBP")
            {
                kodMetin = "Update Customers set " + currency + "='" + (gbp - amount) + "',TRY='" + Math.Round(tl + (amount * gbpBuying), 4) + "' Where CustomerID='" + Cryptology.Encrypt(customerID) + "'";
                if (gbp>=amount)
                {
                    isAvailable = true;
                    result = amount * eurBuying;
                }
            }
            if (isAvailable)
            {
                connection.Open();
                SqlCommand command = new SqlCommand(kodMetin, connection);
                command.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Count : " + result, "Successful");
            }
            else
            {
                MessageBox.Show("Not Enough Balance");
            }
            ListData();

        }//ExchangeSelling
        private void ListData()
        {
            connection.Open();
            SqlCommand command = new SqlCommand("Select *from Customers Where CustomerID='" + Cryptology.Encrypt(customerID) + "'", connection);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();//gerek yok herhalde????
            tl = Convert.ToDouble(reader["TRY"].ToString().TrimEnd());
            usd = Convert.ToDouble(reader["USD"].ToString().TrimEnd());
            eur = Convert.ToDouble(reader["EUR"].ToString().TrimEnd());
            gbp = Convert.ToDouble(reader["GBP"].ToString().TrimEnd());
            connection.Close();
            label16.Text = tl.ToString(CultureInfo.InvariantCulture) + " Turkish Liras";
            textBox5.Text = usd.ToString(CultureInfo.InvariantCulture);
            textBox4.Text = eur.ToString(CultureInfo.InvariantCulture);
            textBox6.Text = gbp.ToString(CultureInfo.InvariantCulture);
        }
        private void timer1_Tick(object sender, EventArgs e)
        { //Gereksiz
        }//timer
        public string createLastXML(DateTime now,int a)
        {

            string day = ((now.Day)-a).ToString();
            string month = (now.Month).ToString();
            string year = (now.Year).ToString();
            if (int.Parse(day)<10)
            {
                day = "0" + day;
            }
            if (int.Parse(month) < 10)
            {
                month = "0" + month;
            }
            string yesterdayXML = "https://www.tcmb.gov.tr/kurlar/" +year+month+"/"+day+month+year+".xml";
            return yesterdayXML;
        }
        public string calculateFluctuation(double once,double sonra)
        {
            string degisim = (((sonra - once) / once) * 100).ToString("N4");
            return degisim;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ExchangeBuy("USD",double.Parse(textBox1.Text));
            textBox1.Clear();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            ExchangeBuy("EUR", double.Parse(textBox2.Text));
            textBox2.Clear();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ExchangeBuy("GBP", double.Parse(textBox3.Text));
            textBox3.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ExchangeSell("USD", double.Parse(textBox1.Text));
            textBox1.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ExchangeSell("EUR", double.Parse(textBox2.Text));
            textBox2.Clear();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ExchangeSell("GBP", double.Parse(textBox3.Text));
            textBox3.Clear();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text=="DOLAR")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
                textBox1.TextAlign = HorizontalAlignment.Left;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "DOLAR";
                textBox1.ForeColor = Color.Silver;
                textBox1.TextAlign = HorizontalAlignment.Center;
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "EURO")
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
                textBox2.TextAlign = HorizontalAlignment.Left;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "EURO";
                textBox2.ForeColor = Color.Silver;
                textBox2.TextAlign = HorizontalAlignment.Center;
            }
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            if (textBox3.Text == "POUND")
            {
                textBox3.Text = "";
                textBox3.ForeColor = Color.Black;
                textBox3.TextAlign = HorizontalAlignment.Left;
            }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                textBox3.Text = "POUND";
                textBox3.ForeColor = Color.Silver;
                textBox3.TextAlign = HorizontalAlignment.Center;
            }
        }

        private void ExchangeRates_Load(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;  
            timer1.Enabled = true;
            timer1.Interval = 3000;
            ListData();
            BackGroundRates USD = new BackGroundRates("USD");
            if (USD.selling == USD.sellingLast)
            {
                label1.Text = "Borsa Tarihi:    " + USD.dateInfo+"                                                                                 Comparison Failed !!!";
                label1.ForeColor = Color.Red;
            }
            else
            {
                label1.Text = "Borsa Tarihi:    " + USD.dateInfo;
            }
            usdBuying = USD.buying ;
            label4.Text = USD.sellingLast.ToString();
            if (USD.selling>USD.sellingLast)
            {
                label3.ForeColor = Color.Green;
            }
            else if (USD.selling < USD.sellingLast)
            {
                label3.ForeColor = Color.Red;
            }
            usdSelling = USD.selling;
            BackGroundRates EUR = new BackGroundRates("EUR");
            //label1.Text = USD.dateInfo;
            eurBuying = EUR.buying;
            label7.Text = EUR.sellingLast.ToString();
            if (EUR.selling > EUR.sellingLast)
            {
                label6.ForeColor = Color.Green;
            }
            else if (EUR.selling < EUR.sellingLast)
            {
                label6.ForeColor = Color.Red;
            }
            eurSelling = EUR.selling;
            BackGroundRates GBP = new BackGroundRates("GBP");
            //label1.Text = USD.dateInfo;
            gbpBuying = GBP.buying;
            label10.Text = GBP.sellingLast.ToString();
            if (GBP.selling > GBP.sellingLast)
            {
                label9.ForeColor = Color.Green;
            }
            else if (GBP.selling < GBP.sellingLast)
            {
                label9.ForeColor = Color.Red;
            }
            gbpSelling = GBP.selling;
            label2.Text = USD.buying.ToString();
            label5.Text = EUR.buying.ToString();
            label8.Text = GBP.buying.ToString();
            label3.Text=usdSelling + "     %" + calculateFluctuation(USD.sellingLast, USD.selling);
            label6.Text=eurSelling + "     %" + calculateFluctuation(EUR.sellingLast, EUR.selling);
            label9.Text=gbpSelling + "     %" + calculateFluctuation(GBP.sellingLast, GBP.selling);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainPage.customerID = customerID;
            MainPage mainPage = new MainPage();
            mainPage.Show();
        }

        private void button2_MouseHover(object sender, EventArgs e)
        {
            button2.BackColor = Color.DarkGray;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.BackColor = Color.Transparent;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ExchangeRates_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                this.SetDesktopLocation(MousePosition.X - mouseX, MousePosition.Y - mouseY);
            }
        }
    }
}
