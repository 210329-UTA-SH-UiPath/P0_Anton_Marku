﻿using System;
using System.Collections.Generic;

#nullable disable

namespace PbDb.Storing.Entities
{
    public partial class Pizza
    {
        public int Id { get; set; }
        public int CrustId { get; set; }
        public int SizeId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public virtual Crust Crust { get; set; }
        public virtual Size Size { get; set; }
    }
}
