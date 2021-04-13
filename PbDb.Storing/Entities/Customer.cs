using System;
using System.Collections.Generic;

#nullable disable

namespace PbDb.Storing.Entities
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime LastTimeOrdered { get; set; }
        public int LastStoreVisited { get; set; }
        public DateTime LastStoreVisitTime { get; set; }

        public virtual Store LastStoreVisitedNavigation { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
