using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class SalesViewModel
    {
        public long ID { set; get; }
        public long ProductId { set; get; }
        public long? CustomerID { get; set; }

        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string ShipName { get; set; }

        public int? Quantity { get; set; }
        public string ShipMobile { get; set; }

      
        public string ShipEmail { get; set; }

        public DateTime? CreateDate { get; set; }
        public string ShipAddress { get; set; }
        public decimal TotalOrder { get; set; }
        public decimal Total { get; set; }
        public decimal Price { get; set; }

        public bool? Status { get; set; }
    }
}
