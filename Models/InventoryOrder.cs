using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopBridge.Models
{
    public class InventoryOrder
    {
        public string BillNo { get; set; }
        public DateTime OrderDt { get; set; }
        public int SupplierId { get; set; }
        public int CustomerId { get; set; }
        public List<OrderItem> orderItems { get; set; }
    }
}