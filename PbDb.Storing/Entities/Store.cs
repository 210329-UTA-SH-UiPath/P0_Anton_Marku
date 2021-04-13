using System;
using System.Collections.Generic;

#nullable disable

namespace PbDb.Storing.Entities
{
    public partial class Store
    {
        public Store()
        {
            Customers = new HashSet<Customer>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
