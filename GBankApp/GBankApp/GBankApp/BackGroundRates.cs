using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace GBankApp
{
    class BackGroundRates
    {
        DateTime now = DateTime.Now;
        public string dateInfo;
        public double buying;
        public double selling;
        public double sellingLast;
        public string bugun= "https://www.tcmb.gov.tr/kurlar/today.xml";
        public string kodMetinSelling="";
        public string kodMetinBuying="";


        
        public string createLastXML(int a)
        {
            string day = ((now.Day) - a).ToString();
            string month = (now.Month).ToString();
            string year = (now.Year).ToString();
            if (int.Parse(day) < 10)
            {
                day = "0" + day;
            }
            if (int.Parse(month) < 10)
            {
                month = "0" + month;
            }
            string yesterdayXML = "https://www.tcmb.gov.tr/kurlar/" + year + month + "/" + day + month + year + ".xml";
            return yesterdayXML;
        }
        public double OldData(int term)
        {
            XmlDocument xmlDoc1 = new XmlDocument();
            string loadString = createLastXML(term);
            try
            {
                xmlDoc1.Load(loadString);

            }
            catch (System.Net.WebException)
            {
                return OldData(term-1);
            }
            return double.Parse(xmlDoc1.SelectSingleNode(kodMetinSelling).InnerXml);
        }
        public BackGroundRates(string kod)
        { 
            XmlDocument xmlDoc = new XmlDocument();
            int i = 0;
            do
            {//web sitesi sürekli hata veriyordu program patlamasını önleyici try catch
                try
                {
                    xmlDoc.Load(bugun);
                    i = 1;
                }
                catch (System.Net.WebException)
                {
                    i = 0;
                }
            } while (i==0);
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
            DateTime date = Convert.ToDateTime(xmlDoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
            dateInfo = date.ToShortDateString();//sitenin tarihi
            kodMetinSelling = "Tarih_Date / Currency[@Kod = '" + kod + "']/BanknoteSelling";
            kodMetinBuying = "Tarih_Date / Currency[@Kod = '" + kod + "']/BanknoteBuying";
            selling = double.Parse(xmlDoc.SelectSingleNode(kodMetinSelling).InnerXml);//, System.Globalization.CultureInfo.InvariantCulture
            buying = double.Parse(xmlDoc.SelectSingleNode(kodMetinBuying).InnerXml);
            sellingLast=OldData(4);

        }//Consturctor Method
    }
}
