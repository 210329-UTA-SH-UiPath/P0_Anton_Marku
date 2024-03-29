﻿using System;
using System.Collections.Generic;

#nullable disable

namespace PbDb.Storing.Entities
{
    public partial class OrderPizza
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int? PizzaId { get; set; }
        public decimal Price { get; set; }

        public virtual Order Order { get; set; }
        public virtual Pizza Pizza { get; set; }
    }
}
