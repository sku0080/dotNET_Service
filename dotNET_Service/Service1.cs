﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.ServiceProcess;
using System.Text;
using System.Xml;

namespace dotNET_Service
{
    public partial class ServiceDotNet : ServiceBase
    {
        private static string Database_Name = "\\database.xml";
        private string Database_Filepath = (AppDomain.CurrentDomain.BaseDirectory + Database_Name);

        private string Msg_Body = "";

        public ServiceDotNet()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Currencies Data = new Currencies(Database_Filepath);

            List<string> currencies = new List<string>();
            currencies = Data.DownloadData();

            foreach (string currency in currencies)
            {
                Msg_Body += currency + "\n";
                Data.Insert_Record(currency, DateTime.Now);
            }

            SendMail();
        }

        protected override void OnStop()
        {

        }

        public void SendMail()
        {
            try
            {
                SmtpClient MailServer = new SmtpClient("smtp.gmail.com", 587);
                MailServer.EnableSsl = true;
                MailServer.Credentials = new System.Net.NetworkCredential("vitezslav.skura@gmail.com", "AppleGoogle970120");

                MailMessage Msg = new MailMessage("vitezslav.skura@gmail.com", "vita.skura@email.cz");
                Msg.Subject = "Service Project";
                Msg.Body = this.Msg_Body;

                MailServer.Send(Msg);
            }

            catch (Exception e)
            {
                throw new ApplicationException();
            }
        }
    }

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

            using (WebClient wc = new WebClient())
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

            if (!File.Exists(this.Database))
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
    }
}
