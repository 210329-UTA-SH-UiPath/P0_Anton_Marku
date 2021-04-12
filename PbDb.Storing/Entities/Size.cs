using System;
using System.Collections.Generic;

#nullable disable

namespace PbDb.Storing.Entities
{
    public partial class Size
    {
        public Size()
        {
            Pizzas = new HashSet<Pizza>();
        }

        public int Id { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<Pizza> Pizzas { get; set; }
    }
}
