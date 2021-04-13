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
            //login + save the store visit
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

            //selecting a pizza preset
            Console.WriteLine("Enter 2 for MeatPizza, 3 for Veggie");
            int PizzaChoice = int.Parse(Console.ReadLine());
            var Pizza = context.Pizzas.FirstOrDefault(p => p.Id == PizzaChoice);

            var Order = new Order()
            {
                CustomerId = Customer.Id,
                StoreId = Store.Id,
                DateAndTime = CurrentDt,
            };

            var OrderPizza = new OrderPizza()
            {
                Order = Order,
                Pizza = Pizza,
            };
            context.Add(Order);
            context.Add(OrderPizza);
            context.SaveChanges();
        }
    }
}
