using Microsoft.Office.Interop.Excel;
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
    public partial class AccountSummary : Form
    {
        internal static string customerID { get; set; }
        public static SqlConnection connection = MainPage.connection;
        public AccountSummary()
        {
            InitializeComponent();
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            button1.BackColor = Color.Pink;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackColor = Color.Transparent;
        }

        private void button1_Click(object sender, EventArgs e)
        {
               
        }
        bool move;
        int mouseX, mouseY;

        private void AccountSummary_MouseUp(object sender, MouseEventArgs e)
        {
            move = false;
        }
        private void AccountSummary_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                this.SetDesktopLocation(MousePosition.X-mouseX,MousePosition.Y-mouseY);
            }
        }

        private void button2_MouseHover(object sender, EventArgs e)
        {
            button2.BackColor = Color.DarkGray;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.BackColor = Color.Transparent;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainPage.customerID = customerID;
            MainPage mainPage = new MainPage();
            mainPage.Show();
        }
        private void ListData()
        {
            connection.Open();
            SqlCommand command = new SqlCommand("Select *from Process Where CustomerID='"+customerID+"'",connection);
            SqlDataReader reader=command.ExecuteReader();
            while (reader.Read())
            {
                ListViewItem item = new ListViewItem();
                item.Text = reader["id"].ToString().Trim();
                item.SubItems.Add(reader["InvoiceID"].ToString().Trim());
                item.SubItems.Add(reader["Task"].ToString().Trim());
                item.SubItems.Add(reader["Amount"].ToString().Trim());
                item.SubItems.Add(reader["Date"].ToString().Trim());
                listView1.Items.Add(item);
            }
            connection.Close();
        }

        private void AccountSummary_Load(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            //this.listView1.ColumnWidthChanging += new ColumnWidthChangingEventHandler(listView1_ColumnWidthChanging);
            ListData();
            ShowNameBalance();
            columnHeader1.Width = 80;//sütun genişlikleri ince ayarlama
            columnHeader2.Width = 122;
            columnHeader3.Width = 70;
            columnHeader4.Width = 90;
            columnHeader5.Width = 170;
        }
        private void ShowNameBalance()
        {
            connection.Open();
            SqlCommand command = new SqlCommand("Select *from Customers Where CustomerID='" + Cryptology.Encrypt(customerID) + "'", connection);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            label11.Text = Cryptology.Decrypt(reader["Name"].ToString().TrimEnd()) +" "+ Cryptology.Decrypt(reader["Surname"].ToString().TrimEnd())+ "      Balance: " + reader["TRY"].ToString().TrimEnd()  + " TL";
            connection.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog save = new SaveFileDialog() { Filter = "Excel Workbook|*.xls", ValidateNames = true })
            {
                if (save.ShowDialog()==DialogResult.OK)
                {
                    Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                    Workbook workbook =app.Workbooks.Add(XlSheetType.xlWorksheet);
                    Worksheet worksheet = (Worksheet)app.ActiveSheet;
                    app.Visible = false;
                    worksheet.Cells[1, 1] = label1.Text;
                    worksheet.Cells[1, 2] = label2.Text;
                    worksheet.Cells[1, 3] = label3.Text;
                    worksheet.Cells[1, 4] = label4.Text;
                    worksheet.Cells[1, 5] = label5.Text;
                    int row = 2;
                    foreach (ListViewItem item in listView1.Items)
                    {
                        worksheet.Cells[row, 1] = item.SubItems[0].Text;
                        worksheet.Cells[row, 2] = item.SubItems[1].Text;
                        worksheet.Cells[row, 3] = item.SubItems[2].Text;
                        worksheet.Cells[row, 4] = item.SubItems[3].Text;
                        worksheet.Cells[row, 5] = item.SubItems[4].Text;
                        row++;
                    }
                    workbook.SaveAs(save.FileName,XlFileFormat.xlWorkbookDefault,Type.Missing,Type.Missing,true,false,XlSaveAsAccessMode.xlNoChange,XlSaveConflictResolution.xlLocalSessionChanges,Type.Missing,Type.Missing);
                    app.Quit();
                    MessageBox.Show("Your summary data has been successfully exported","Message",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
            }
        }

        private void AccountSummary_MouseDown(object sender, MouseEventArgs e)
        {
            move = true;
            mouseX = e.X;
            mouseY = e.Y;
        }
    }
}
