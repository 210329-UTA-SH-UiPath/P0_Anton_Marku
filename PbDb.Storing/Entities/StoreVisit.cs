using System;
using System.Collections.Generic;

#nullable disable

namespace PbDb.Storing.Entities
{
    public partial class StoreVisit
    {
        public int StoreId { get; set; }
        public int CustomerId { get; set; }
        public DateTime LastVisitTime { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Store Store { get; set; }
    }
}
