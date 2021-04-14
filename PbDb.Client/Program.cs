using System;
using PbDb.Storing.Entities;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using PbDb.Domain.Models;

namespace PbDb.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter 1 if you are a customer, 2 if you are a store owner");
            int StoreOrCustomer = int.Parse(Console.ReadLine());
            if (StoreOrCustomer == 1)
            {
                Run();
            }
            else
            {
                StoreRun();
            }
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
            //finished login

            var Order = new Order()
            {
                CustomerId = Customer.Id,
                StoreId = Store.Id,
                DateAndTime = CurrentDt,
            };
            //this loops and allows the customer to add or complete their order
            Console.WriteLine("Enter 1 to order, 0 if you don't want to order or if you want to finalize your order");
            int OrderOrNot = int.Parse(Console.ReadLine());
            decimal OrderTotalPrice = 0;
            int OrderSizeLimit = 50;
            decimal PriceLimit = 250;
            while (OrderOrNot != 0 && --OrderSizeLimit > -1)
            {
                //SHOULD BE IN a while loop that repeatedly asks if user wants to add another pizza to order
                //selecting a pizza preset
                if (OrderOrNot == 1)
                {
                    Console.WriteLine("Enter 1 for preset Pizza, 2 for custom");
                    int CustomOrPreset = int.Parse(Console.ReadLine());
                    if (CustomOrPreset == 1)
                    {
                        Console.WriteLine("Enter 2 for MeatPizza, 3 for Veggie");
                        int PizzaChoice = int.Parse(Console.ReadLine());
                        var Pizza = context.Pizzas.FirstOrDefault(p => p.Id == PizzaChoice);
                        if (OrderTotalPrice + Pizza.Price > PriceLimit)
                        {
                            OrderOrNot = 0;
                            Console.WriteLine("You've exceed the order price limit, discarding the last selected pizza");
                        }
                        else
                        {
                            var OrderPizza = new OrderPizza()
                            {
                                Order = Order,
                                Pizza = Pizza,
                                Price = Pizza.Price,
                            };
                            context.Add(OrderPizza);
                            OrderTotalPrice += Pizza.Price;
                        }
                    }
                    //custom pizza setup
                    else
                    {
                        Console.WriteLine("Pick a crust type");
                        foreach (Crust Crust in context.Crusts)
                        {
                            Console.WriteLine($"{Crust.Id}-{Crust.Type}: {Crust.Price}");
                        }
                        int CrustChoice = int.Parse(Console.ReadLine());
                        var ChosenCrust = context.Crusts.SingleOrDefault(a => a.Id == CrustChoice);

                        Console.WriteLine("Pick a size type");
                        foreach (Size Size in context.Sizes)
                        {
                            Console.WriteLine($"{Size.Id}-{Size.Type}: {Size.Price}");
                        }
                        int SizeChoice = int.Parse(Console.ReadLine());
                        var ChosenSize = context.Sizes.SingleOrDefault(a => a.Id == SizeChoice);

                        //Has to be modified to allow picking an array of toppings
                        decimal PizzaPrice = 0;
                        List<Topping> ChosenToppings = new List<Topping>();
                        for (int i = 0; i < 5; i++)
                        {
                            Console.WriteLine("Pick a Topping type");
                            foreach (Topping Topping in context.Toppings)
                            {
                                Console.WriteLine($"{Topping.Id}-{Topping.Type}: {Topping.Price}");
                            }
                            int ToppingChoice = int.Parse(Console.ReadLine());
                            var ChosenTopping = context.Toppings.Single(a => a.Id == ToppingChoice);
                            ChosenToppings.Add(ChosenTopping);
                            PizzaPrice += ChosenTopping.Price;
                        }
                        PizzaPrice += ChosenCrust.Price;
                        PizzaPrice += ChosenSize.Price;

                        //var Pizza = context.Pizzas.SingleOrDefault(a => a.Crust == ChosenCrust && a.Size == ChosenSize && a.PizzaToppings == { ChosenTopping} );
                        if (OrderTotalPrice + PizzaPrice > PriceLimit)
                        {
                            OrderOrNot = 0;
                            Console.WriteLine("You've exceed the order price limit, discarding the last selected pizza");
                        }
                        else
                        {
                            var OrderPizza = new OrderPizza()
                            {
                                Order = Order,
                                Price = PizzaPrice,
                            };
                            context.Add(OrderPizza);
                            OrderTotalPrice += PizzaPrice;
                        }
                    }
                    if (OrderOrNot == 1)
                    {
                        Console.WriteLine("Enter 1 to keep ordering, 0 if you want to finalize your order");
                        OrderOrNot = int.Parse(Console.ReadLine());
                    }
                }
            }
            Order.Price = OrderTotalPrice;
            context.Add(Order);
            context.SaveChanges();
            //At this point we have the order and the OrderPizzas in the database,
            //Price is below 250 and # items is < 50
            //let user remove items if they choose

            Console.WriteLine("Enter 1 to modify your order, or 0 to see final price");
            int ModifyChoice = int.Parse(Console.ReadLine());
            while (ModifyChoice == 1)
            {
                var Items = context.OrderPizzas
                                    .Where(a => a.OrderId == Order.Id)
                                    .ToList();
                if (Items.Count > 0)
                {
                    Console.WriteLine("Enter number corresponding to the order item you want removed");
                    foreach (var Item in Items)
                    {
                        Console.WriteLine($"{Item.Id}: Pizza that costs {Item.Price} dollars");
                    }
                    int ItemSelection = int.Parse(Console.ReadLine());
                    var ItemToRemove = context.OrderPizzas.Single(a => a.OrderId == Order.Id && a.Id == ItemSelection);
                    context.OrderPizzas.Remove(ItemToRemove);
                    Order.Price -= ItemToRemove.Price;
                    context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("No items left to remove");
                }
                Console.WriteLine("Enter 1 to keep modifying your order, or 0 to see final price");
                ModifyChoice = int.Parse(Console.ReadLine());
            }

            Console.WriteLine($"Final Price: {Order.Price}");
        }

        static void StoreRun()
        {
            Console.WriteLine("Enter 1 for Domino's, 2 for Pizza Hut");
            int StoreChoice = int.Parse(Console.ReadLine());
            var context = new myDBContext();
            var Store = context.Stores.Single(a => a.Id == StoreChoice);
            Console.WriteLine("Enter 1 for complete order history, 2 for weekly revenue, 3 for monthly");
            int InformationChoice = int.Parse(Console.ReadLine());
            if (InformationChoice == 1)
            {
                var OrderList = context.Orders.Where(a => a.StoreId == Store.Id);
                foreach (var Order in OrderList)
                {
                    Console.WriteLine($"Customer {Order.CustomerId} spent {Order.Price} dollars on {Order.DateAndTime}");
                }
            }
            var revenueRecords = new List<RevenueRecord>();
            SqlConnection connection = new SqlConnection("Server=tcp:p1.database.windows.net,1433;Initial Catalog=myDB;User ID=p1;Password=Pword000");
            if (InformationChoice == 2)
            {
                SqlCommand cmd = new SqlCommand($"SELECT SUM(Price) AS REVENUE, DATEPART(YEAR, DateAndTime) AS YEAR, DATEPART(WEEK, DateAndTime) AS WEEK FROM Orders WHERE StoreID = {StoreChoice} GROUP BY DATEPART(YEAR, DateAndTime), DATEPART(WEEK, DateAndTime);", connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        RevenueRecord revenueRecord = new RevenueRecord();
                        revenueRecord.Revenue = reader.GetDecimal(reader.GetOrdinal("REVENUE"));
                        revenueRecord.Year = reader.GetInt32(reader.GetOrdinal("YEAR"));
                        revenueRecord.Week = reader.GetInt32(reader.GetOrdinal("WEEK"));
                        revenueRecords.Add(revenueRecord);
                    }
                    foreach (RevenueRecord revenueRecord1 in revenueRecords)
                    {
                        Console.WriteLine(revenueRecord1.WeekPrint());
                    }
                }
            }
            else
            {
                //monthly rev
                SqlCommand cmd = new SqlCommand($"SELECT SUM(Price) AS REVENUE, DATEPART(YEAR, DateAndTime) AS YEAR, DATEPART(MONTH, DateAndTime) AS MONTH FROM Orders WHERE StoreID = {StoreChoice} GROUP BY DATEPART(YEAR, DateAndTime), DATEPART(MONTH, DateAndTime);", connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        RevenueRecord revenueRecord = new RevenueRecord();
                        revenueRecord.Revenue = reader.GetDecimal(reader.GetOrdinal("REVENUE"));
                        revenueRecord.Year = reader.GetInt32(reader.GetOrdinal("YEAR"));
                        revenueRecord.Month = reader.GetInt32(reader.GetOrdinal("MONTH"));
                        revenueRecords.Add(revenueRecord);
                    }
                    foreach (RevenueRecord revenueRecord1 in revenueRecords)
                    {
                        Console.WriteLine(revenueRecord1.MonthPrint());
                    }
                }
            }
        }
    }
}
