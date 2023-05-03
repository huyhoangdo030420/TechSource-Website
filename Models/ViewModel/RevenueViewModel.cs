
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

namespace Models.ViewModel
{
    public partial class RevenueViewModel
    {
        public long ID { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
