using System;
using System.Collections.Generic;

#nullable disable

namespace PbDb.Storing.Entities
{
    public partial class Topping
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
    }
}
