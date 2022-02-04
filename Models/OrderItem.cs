using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopBridge.Models
{
    public class OrderItem
    {
        public int InventoryOrderID { get; set; }
        public int ProductID { get; set; }
        public Double Price { get; set; }
        public Double Quantity { get; set; }
        public Double Discount { get; set; }
        public Double TotalPrice { get; set; }
    }
}