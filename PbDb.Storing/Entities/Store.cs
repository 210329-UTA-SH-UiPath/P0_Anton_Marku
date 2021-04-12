using System;
using System.Collections.Generic;

#nullable disable

namespace PbDb.Storing.Entities
{
    public partial class Store
    {
        public Store()
        {
            Orders = new HashSet<Order>();
            StoreVisits = new HashSet<StoreVisit>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<StoreVisit> StoreVisits { get; set; }
    }
}
