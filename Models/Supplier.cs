using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopBridge.Models
{
    public class Supplier
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Place { get; set; }
        public string ContactNo { get; set; }
        public string EMailID { get; set; }
        public List<InventoryOrder> orderInventories { get; set; }
    }
}