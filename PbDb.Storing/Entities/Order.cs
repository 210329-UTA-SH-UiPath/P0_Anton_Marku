using System;
using System.Collections.Generic;

#nullable disable

namespace PbDb.Storing.Entities
{
    public partial class Order
    {
        public Order()
        {
            OrderPizzas = new HashSet<OrderPizza>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int StoreId { get; set; }
        public DateTime DateAndTime { get; set; }
        public decimal? Price { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Store Store { get; set; }
        public virtual ICollection<OrderPizza> OrderPizzas { get; set; }
    }
}
