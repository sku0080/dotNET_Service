using System;
using System.Collections.Generic;
using dotNET_Currencies;

namespace Konzole
{
    class Program
    {
        static void Main(string[] args)
        {
            Currencies Data = new Currencies("database.xml");

            List<string> currencies = new List<string>();
            currencies = Data.DownloadData();

            foreach(string currency in currencies)
            {
                Console.WriteLine(currency);
                Data.Insert_Record(currency, DateTime.Now);
            }

            Console.ReadKey();
        }
    }
}
