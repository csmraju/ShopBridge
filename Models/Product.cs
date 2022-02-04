using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopBridge.Models
{
    public class Product
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string Unit { get; set; }
        public Decimal Price { get; set; }
        public Decimal Quantity { get; set; }

    }
}