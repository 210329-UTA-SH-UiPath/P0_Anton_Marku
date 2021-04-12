using System.Globalization;
using System;
using PbDb.Storing.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PbDb.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Run();
        }

        static void Run()
        {
            var context = new myDBContext();
            Console.WriteLine("Enter your user ID");
            string CustomerName = Console.ReadLine();
            string StoreName = "";
            Console.WriteLine("Enter 1 for Domino's, 2 for Pizza Hut");
            string Selection = Console.ReadLine();
            switch (Selection)
            {
                case "1":
                    StoreName = "Dominoes";
                    break;
                case "2":
                    StoreName = "PizzaHut";
                    break;
            }
            var CurrentDt = DateTime.Now;
            var Store = context.Stores.Single(a => a.Name == StoreName);
            var Customer = context.Customers.SingleOrDefault(a => a.Name == CustomerName);
            if (Customer == null)
            {
                Customer = new Customer()
                {
                    Name = CustomerName,
                    LastTimeOrdered = CurrentDt,
                };
                context.Add(Customer);
                context.SaveChanges();
            }
            else
            {
                Customer.LastTimeOrdered = CurrentDt;
            }
            var StoreVisit = context.StoreVisits.SingleOrDefault(a => a.CustomerId == Customer.Id);
            if (StoreVisit == null)
            {
                StoreVisit = new StoreVisit()
                {
                    Customer = Customer,
                    Store = Store,
                    LastVisitTime = CurrentDt,
                };
                context.Add(StoreVisit);
                context.SaveChanges();
            }
            else
            {
                StoreVisit.StoreId = Store.Id;
                StoreVisit.LastVisitTime = CurrentDt;
            }
            context.SaveChanges();
        }

        static string PickStore()
        {
            string Store = "";
            Console.WriteLine("Enter 1 for Domino's, 2 for Pizza Hut");
            string Selection = Console.ReadLine();
            switch (Selection)
            {
                case "1":
                    Store = "Domino's";
                    break;
                case "2":
                    Store = "Pizza Hut";
                    break;
            }
            return Store;
        }
    }
}
