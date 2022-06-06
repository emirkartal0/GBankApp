using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace GBankApp
{
    class MailSender
    {
        private string account = Cryptology.Encrypt("gbannk.offical@hotmail.com");
        private string pass = Cryptology.Encrypt("8105.ttt");
        public MailSender(string subject,string text,string mail)
        {
            
            MailMessage message = new MailMessage();
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(Cryptology.Decrypt(account),Cryptology.Decrypt(pass));
            client.Port = 587;
            client.Host="smtp.live.com";
            client.EnableSsl = true;
            message.To.Add(mail);
            message.From = new MailAddress(Cryptology.Decrypt(account));
            message.Subject = subject;
            message.Body = text;
            int a = 4;
            System.Windows.Forms.MessageBox.Show("Mail is Sent");
            do
            {

                try
                {
                    client.Send(message);
                    System.Windows.Forms.MessageBox.Show("Verification Code is Sent");
                }
                catch (Exception e)
                {
                    a--;
                    if (a == 2)
                    {
                        System.Windows.Forms.MessageBox.Show("Mail could not send  " + e);
                        System.Windows.Forms.MessageBox.Show("Mail could not send");
                    }
                }
            } while (2 < a && a < 4);
        }
    }
}
