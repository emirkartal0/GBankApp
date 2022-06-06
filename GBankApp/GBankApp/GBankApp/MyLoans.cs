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
    public partial class MyLoans : Form
    {
        public static SqlConnection connection = MainPage.connection;
        public double tl, bankCharge = 0, instalment = 0, principal = 0, interestRate = 0, expiry = 0, month = 0, loanBalance = 0;
        public string loanName,idName,lastProcessDate,creditDate;
        public string mailAcc;

        internal static string customerID { get; set; }

        private void button3_MouseHover(object sender, EventArgs e)
        {
            button3.BackColor = Color.DarkGray;
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            button3.BackColor = Color.Transparent;
        }
        bool move=false;
        int mouseX, mouseY;
        private void MyLoans_MouseDown(object sender, MouseEventArgs e)
        {
            move = true;
            mouseX = e.X;
            mouseY = e.Y;
        }
        public void ListData()
        {
            connection.Open();
            SqlCommand command = new SqlCommand("Select *from Loans Where CustomerID='" + Cryptology.Encrypt(customerID) + "'", connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ListViewItem item = new ListViewItem();
                item.Text = reader["id"].ToString().Trim();
                item.SubItems.Add(reader["TypeOfLoan"].ToString().Trim());
                item.SubItems.Add(reader["Amount"].ToString().Trim()+"-TL");
                item.SubItems.Add(reader["Instalment"].ToString().Trim()+"-TL");
                item.SubItems.Add(reader["Month"].ToString().Trim()+"/"+ reader["Expiry"].ToString().Trim());
                item.SubItems.Add(reader["LastProcessDate"].ToString().Trim());
                item.SubItems.Add(reader["Date"].ToString().Trim());
                if (Convert.ToInt32(reader["IsFinished"].ToString())==1)
                {
                    item.SubItems.Add("Finished");
                }
                else
                {
                    item.SubItems.Add("Resuming");
                }
                listView1.Items.Add(item);
            }
            connection.Close();
        }
        public void refreshBalance()
        {
            connection.Open();
            SqlCommand command = new SqlCommand("Select TRY,GmailAccount from Customers Where CustomerID='" + Cryptology.Encrypt(customerID)+"'",connection);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            tl = Convert.ToDouble(reader["TRY"].ToString().Trim());
            mailAcc = Cryptology.Decrypt(reader["GmailAccount"].ToString().Trim());
            connection.Close();
            label11.Text = tl.ToString();

        }
        private void MyLoans_Load(object sender, EventArgs e)
        {
            ListData();
            refreshBalance();
        }
        int id=0;
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            id = int.Parse(listView1.SelectedItems[0].SubItems[0].Text);
            DetailsSelectedData();
            if (month<expiry)
            {
                flowLayoutPanel1.BackColor = Color.PaleTurquoise;
                label7.Visible = false;
                button2.Enabled = true;
            }
            else
            {
                label7.Visible = true;
                flowLayoutPanel1.BackColor = Color.PaleGreen;
            }
        }
        private void DetailsSelectedData()
        {
            connection.Open();
            SqlCommand command = new SqlCommand("Select * from Loans Where id='"+id+"'",connection);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            idName = reader["id"].ToString();
            loanName = reader["TypeOfLoan"].ToString();
            lastProcessDate = reader["LastProcessDate"].ToString();
            instalment = Convert.ToDouble(reader["Instalment"].ToString());
            month = Convert.ToDouble(reader["Month"].ToString());
            expiry=Convert.ToDouble(reader["Expiry"].ToString());
            principal = Convert.ToDouble(reader["Amount"].ToString());
            connection.Close();
            textBox6.Text = idName;
            textBox5.Text = loanName;
            textBox4.Text = lastProcessDate;
            textBox3.Text = instalment.ToString();
            textBox2.Text = month.ToString() + "/" + expiry.ToString();
            textBox1.Text=principal.ToString();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CleanSelected()
        {
            textBox6.Clear();
            textBox5.Clear();
            textBox4.Clear();
            textBox3.Clear();
            textBox2.Clear();
            textBox1.Clear();
            button2.Enabled = false;
        }
        private void InsertSum()
        {
            DateTime date = DateTime.Now;
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
            connection.Open();
            SqlCommand commandProcess = new SqlCommand("Insert into Process (CustomerID,InvoiceID,Task,Amount,Date,IsCredit) values ('" + customerID + "','" + loanName + "','" + "Payment" + "','" + (-instalment) + "','" + date.ToLocalTime().ToString() + "','" + 1 + "')", connection);
            commandProcess.ExecuteNonQuery();
            connection.Close();
        }
        private void PaymentInstalment()
        {
            
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
            DateTime date = DateTime.Now;
            connection.Open();
            if (month+1==expiry)
            {
                SqlCommand command = new SqlCommand("Update Loans set Amount='" + 0+ "', Month='" + (month + 1) + "', LastProcessDate='" + date.ToLocalTime() + "', IsFinished='"+1+"' Where id='" + id + "' ", connection);
                command.ExecuteNonQuery();
                string mailText = "Congratulations!! You Paid  " + loanName.Trim() + ". Thanks for working with us.         " + date.ToLocalTime().ToString();
                MailSender mail = new MailSender("Loans Payment",mailText,mailAcc);
            }
            else
            {
                SqlCommand command = new SqlCommand("Update Loans set Amount='" + (principal - instalment) + "', Month='" + (month + 1) + "', LastProcessDate='" + date.ToLocalTime() + "' Where id='" + id + "' ", connection);
                command.ExecuteNonQuery();
            }
            connection.Close();
            InsertSum();
            connection.Open();
            SqlCommand commandCustomer = new SqlCommand("Update Customers set TRY='" + (tl-instalment)+"'",connection);
            commandCustomer.ExecuteNonQuery();
            connection.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {//Pay button
            DetailsSelectedData();
            MessageBox.Show(""+tl);
            if (tl>=instalment)
            {
                PaymentInstalment();
                MessageBox.Show("Payment Successful");
            }
            else
            {
                MessageBox.Show("Not Enough Balance","Error");
            }
            refreshBalance();
            CleanSelected();
            listView1.Items.Clear();
            ListData();
        }

        private void MyLoans_MouseUp(object sender, MouseEventArgs e)
        {
            move = false;
        }

        private void MyLoans_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                this.SetDesktopLocation(MousePosition.X - mouseX, MousePosition.Y - mouseY);
            }
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackColor = Color.Transparent;
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            button1.BackColor = Color.LightPink;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Loans.customerID = customerID;
            Loans loans = new Loans();
            loans.Show();
        }

        public MyLoans()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
