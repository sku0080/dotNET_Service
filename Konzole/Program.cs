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

            /*foreach(string currency in currencies)
            {
                Console.WriteLine(currency);
                // Data.Insert_Record(currency, DateTime.Now);
            }

            foreach(string item in Data.Get_List_Dates())
            {
                Console.WriteLine(item);
            }*/

            Graph graph = new Graph(Data);

            graph.Draw();

            //Console.ReadKey();
        }
    }
}
