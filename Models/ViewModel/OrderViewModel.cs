using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class OrderViewModel
    {
        public long ID { set; get; }
        public string ShipName { get; set; }
        public string ShipMobile { get; set; }
        public string ShipEmail { get; set; }
        public string ShipAddress { get; set; }
        public decimal? Price { get; set; }
        public bool Status { get; set; }
    }
}
