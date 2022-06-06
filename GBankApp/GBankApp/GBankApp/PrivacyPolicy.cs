using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GBankApp
{
    public partial class PrivacyPolicy : Form
    {
        public PrivacyPolicy()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            SignUpPage signUpPage = new SignUpPage();
            signUpPage.Show();
        }

        private void PrivacyPolicy_Load(object sender, EventArgs e)
        {
            axAcroPDF1.LoadFile("C:\\Users\\GBankApp\\GBankApp\\GBankApp\\Privacy-Policy-of-Gbank-Group.pdf");
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape) 
            {
                this.Close();
                SignUpPage signUpPage = new SignUpPage();
                signUpPage.Show();
            }
            bool res = base.ProcessCmdKey(ref msg, keyData);
            return res;
        }
    }
}
