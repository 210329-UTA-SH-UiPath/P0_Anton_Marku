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
                    LastStoreVisited = Store.Id,
                    LastStoreVisitTime = CurrentDt,
                };
                context.Add(Customer);
                context.SaveChanges();
            }
            else
            {
                //has to check 24 hour limit for store and 2 hour limit for ordering
                //need to do calculations with datetimes
                //if store diff from last visited and time since lsv > 24 hours, no go
                //if store same from lv and < 2 hours since last order, no go
                //else go
                if (Store.Id != Customer.LastStoreVisited && CurrentDt < Customer.LastStoreVisitTime.AddHours(24))
                {
                    Console.WriteLine($"You are still locked into {context.Stores.Single(a => a.Id == Customer.LastStoreVisited).Name}");
                }
                else if (CurrentDt < Customer.LastTimeOrdered.AddHours(2))
                {
                    Console.WriteLine("You can only order once every 2 hours");
                }
                else
                {
                    Customer.LastTimeOrdered = CurrentDt;
                    Customer.LastStoreVisited = Store.Id;
                    Customer.LastStoreVisitTime = CurrentDt;
                }
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
