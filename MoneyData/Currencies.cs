using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace dotNET_Currencies
{
    public class Currencies
    {
        private string Database;

        public Currencies(string database)
        {
            this.Database = database;
        }

        public List<string> DownloadData()
        {
            string url = "http://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt";
            string[] Currencies = { "USD", "EUR", "GBP" };

            List<String> CurrencyList = new List<string>();

            using(WebClient wc = new WebClient())
            {
                string txt = wc.DownloadString(url);

                foreach (string Currency in Currencies)
                    CurrencyList.Add(Currency + " " + ParseRate(txt, Currency));
            }

            return CurrencyList;
        }

        private double? ParseRate(string txt, string code)
        {
            string[] rows = txt.Split(new char[] { '\n' });
            char[] colSplitChars = new char[] { '|' };

            foreach (string row in rows)
            {
                string[] cols = row.Split(colSplitChars);

                if (cols.Length < 3)
                    continue;

                if (cols[3] == code)
                    return double.Parse(cols[4], CultureInfo.InvariantCulture);
            }

            return null;
        }

        public void Insert_Record(string Record, DateTime date)
        {
            XmlDocument doc = new XmlDocument();
            string[] Currency = Record.Split(new char[] { ' ' });

            if (File.Exists(this.Database))
            {

                XmlTextWriter tw;
                tw = new XmlTextWriter(this.Database, Encoding.UTF8);

                tw.WriteStartDocument();
                tw.WriteStartElement("Currency");
                tw.WriteEndElement();
                tw.Close();
            }

            FileStream fs = new FileStream(this.Database, FileMode.Open);
            doc.Load(fs);

            XmlElement codeElem = doc.CreateElement(Currency[0]);

            XmlElement valueElem = doc.CreateElement("Value");
            XmlText valueText = doc.CreateTextNode(Currency[1]);

            XmlElement dateElem = doc.CreateElement("Date");
            XmlText dateText = doc.CreateTextNode(date.ToString());

            valueElem.AppendChild(valueText);
            codeElem.AppendChild(valueElem);

            dateElem.AppendChild(dateText);
            codeElem.AppendChild(dateElem);

            doc.DocumentElement.AppendChild(codeElem);

            fs.Close();
            doc.Save(this.Database);
        }

        public List<double> Get_List_Code(string Code)
        {
            XmlNodeList list;
            XmlDocument doc = new XmlDocument();
            FileStream file = new FileStream(this.Database, FileMode.Open);
            doc.Load(file);
            CultureInfo culture = CultureInfo.CurrentCulture;
            list = doc.SelectNodes("//" + Code + "/Value");
            List<double> Code_list = new List<double>();
            foreach (XmlElement item in list)
            {
                Code_list.Add(double.Parse(item.InnerText, culture));
            }

            file.Close();
            return Code_list;
        }

        public List<string> Get_List_Dates()
        {
            XmlNodeList list;
            XmlDocument doc = new XmlDocument();
            FileStream file = new FileStream(this.Database, FileMode.Open);
            doc.Load(file);
            CultureInfo culture = CultureInfo.CurrentCulture;
            list = doc.SelectNodes("//EUR/Date");
            List<string> Date_list = new List<string>();
            foreach (XmlElement item in list)
            {
                Date_list.Add(item.InnerText);
            }

            file.Close();
            return Date_list;
        }
    }
}
