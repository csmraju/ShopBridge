using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ShopBridge.Models;
using BridgeDA;


namespace ShopBridge.Controllers
{
    public class InventoryController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<InventoryOrder> Get()
        {
            List<InventoryOrder> LinventoryOrder = new List<InventoryOrder>();
            List<OrderItem> LOrderItems = new List<OrderItem>();
            using (BridgeEntities db = new BridgeEntities())
            {
                foreach (BridgeDA.tblInventoryOrder mInventoryOrders in db.tblInventoryOrders.ToList())
                {
                    InventoryOrder inventoryOrder = new InventoryOrder();
                    inventoryOrder.BillNo = mInventoryOrders.BillNo;
                    inventoryOrder.CustomerId = (int)mInventoryOrders.CustomerId;
                    inventoryOrder.OrderDt = (DateTime)mInventoryOrders.OrderDt;
                    inventoryOrder.SupplierId = (int)mInventoryOrders.SupplierId;
                    foreach (BridgeDA.tblOrderItem OrderItems in db.tblOrderItems.Where(o=>o.InventoryOrderID == mInventoryOrders.Id).ToList())
                    {
                        OrderItem orderItem = new OrderItem();
                        orderItem.Discount = (Double) OrderItems.Discount;
                        orderItem.InventoryOrderID = (int)OrderItems.InventoryOrderID;
                        orderItem.Price = (Double)OrderItems.Price;
                        orderItem.ProductID = (int)OrderItems.ProductID;
                        orderItem.Quantity = (Double)OrderItems.Quantity;
                        orderItem.TotalPrice = (Double)OrderItems.TotalPrice;
                        LOrderItems.Add(orderItem);
                    }
                    inventoryOrder.orderItems = LOrderItems;
                    LinventoryOrder.Add(inventoryOrder);
                }
            }
            return LinventoryOrder.ToList().AsEnumerable();
        }

        // GET api/<controller>/5
        public HttpResponseMessage Get(int id)
        {
            List<OrderItem> LOrderItems = new List<OrderItem>();
            InventoryOrder inventoryOrder = new InventoryOrder();
            using (BridgeEntities db = new BridgeEntities())
            {
                BridgeDA.tblInventoryOrder mInventoryOrders = db.tblInventoryOrders.Where(o => o.Id == id).FirstOrDefault();

                if (mInventoryOrders != null)
                {
                    inventoryOrder.BillNo = mInventoryOrders.BillNo;
                    inventoryOrder.CustomerId = (int)mInventoryOrders.CustomerId;
                    inventoryOrder.OrderDt = (DateTime)mInventoryOrders.OrderDt;
                    inventoryOrder.SupplierId = (int)mInventoryOrders.SupplierId;
                    foreach (BridgeDA.tblOrderItem OrderItems in db.tblOrderItems.Where(o => o.InventoryOrderID == mInventoryOrders.Id).ToList())
                    {
                        OrderItem orderItem = new OrderItem();
                        orderItem.Discount = (Double)OrderItems.Discount;
                        orderItem.InventoryOrderID = (int)OrderItems.InventoryOrderID;
                        orderItem.Price = (Double)OrderItems.Price;
                        orderItem.ProductID = (int)OrderItems.ProductID;
                        orderItem.Quantity = (Double)OrderItems.Quantity;
                        orderItem.TotalPrice = (Double)OrderItems.TotalPrice;
                        LOrderItems.Add(orderItem);
                    }
                    inventoryOrder.orderItems = LOrderItems;
                    return Request.CreateResponse(HttpStatusCode.OK, mInventoryOrders);
                }
                else
                {
                    //sending response as error status code NOT FOUND with meaningful message.  
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Inventory Order Not Found");
                }
              
            }
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody] InventoryOrder inventoryOrder)
        {
            try
            {
                using (BridgeEntities db = new BridgeEntities())
                {
                    BridgeDA.tblInventoryOrder mInventoryOrders = new tblInventoryOrder();
                    mInventoryOrders.BillNo = inventoryOrder.BillNo;
                    mInventoryOrders.CustomerId = inventoryOrder.CustomerId;
                    mInventoryOrders.OrderDt = inventoryOrder.OrderDt;
                    mInventoryOrders.SupplierId = inventoryOrder.SupplierId;
                    //To add an new orders record  
                    db.tblInventoryOrders.Add(mInventoryOrders);

                    //Save the submitted record  
                    db.SubmitChanges();
                    using (BridgeEntities db1 = new BridgeEntities())
                    {
                        foreach (OrderItem orderItem in inventoryOrder.orderItems)
                        {
                            BridgeDA.tblOrderItem OrderItems = new tblOrderItem();
                            OrderItems.Discount = (Decimal)orderItem.Discount;
                            OrderItems.InventoryOrderID = mInventoryOrders.Id;
                            OrderItems.Price = (Decimal)orderItem.Price;
                            OrderItems.ProductID = (int)orderItem.ProductID;
                            OrderItems.Quantity = (Decimal)orderItem.Quantity;
                            OrderItems.TotalPrice = (Decimal)orderItem.TotalPrice;

                            db1.tblOrderItems.Add(OrderItems);

                            //Save the submitted record  
                            db1.SubmitChanges();
                        }
                    }
                                           
                    var msg = Request.CreateResponse(HttpStatusCode.Created, inventoryOrder);

                   msg.Headers.Location = new Uri(Request.RequestUri + mInventoryOrders.Id.ToString());

                    return msg;
                }
            }
            catch (Exception ex)
            {

                //return response as bad request  with exception message.  
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Put(int id, [FromBody] InventoryOrder inventoryOrder)
        {
            using (BridgeEntities db = new BridgeEntities())
            {
                //fetching and filter specific Invenory id record   
                var mInventoryOrders = (from a in db.tblInventoryOrders where a.Id == id select a).FirstOrDefault();

                //checking fetched or not with the help of NULL or NOT.  
                if (mInventoryOrders != null)
                {
                    //set received _member object properties with memberdetail  
                    mInventoryOrders.BillNo = inventoryOrder.BillNo;
                    mInventoryOrders.CustomerId = inventoryOrder.CustomerId;
                    mInventoryOrders.OrderDt = inventoryOrder.OrderDt;
                    mInventoryOrders.SupplierId = inventoryOrder.SupplierId;
                    //save set allocation.  
                    db.SubmitChanges();

                    using (BridgeEntities db1 = new BridgeEntities())
                    {
                        foreach (OrderItem orderItem in inventoryOrder.orderItems)
                        {
                            BridgeDA.tblOrderItem OrderItems = (from a in db.tblOrderItems where a.InventoryOrderID == orderItem.InventoryOrderID && a.ProductID == orderItem.ProductID  select a).FirstOrDefault();
                            OrderItems.Discount = (Decimal)orderItem.Discount;
                            OrderItems.InventoryOrderID = mInventoryOrders.Id;
                            OrderItems.Price = (Decimal)orderItem.Price;
                            OrderItems.ProductID = (int)orderItem.ProductID;
                            OrderItems.Quantity = (Decimal)orderItem.Quantity;
                            OrderItems.TotalPrice = (Decimal)orderItem.TotalPrice;

                            //Save the submitted record  
                            db1.SubmitChanges();
                        }
                    }

                    //return response status as successfully updated with member entity  
                    return Request.CreateResponse(HttpStatusCode.OK, mInventoryOrders);
                }
                else
                {
                    //return response error as NOT FOUND  with message.  
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Inventory Order Not Found");
                }
            }


        }

        // DELETE api/<controller>/5  
        public HttpResponseMessage Delete(int id)
        {

            try
            {
                using (BridgeEntities db = new BridgeEntities())
                {
                    //fetching and filter specific member id record   
                    var mInventoryOrders = (from a in db.tblInventoryOrders where a.Id == id select a).FirstOrDefault();

                    //checking fetched or not with the help of NULL or NOT.  
                    if (mInventoryOrders != null)
                    {
                        int _id = mInventoryOrders.Id;
                        db.tblInventoryOrders.Remove(mInventoryOrders);
                        db.SubmitChanges();

                        using (BridgeEntities db1 = new BridgeEntities())
                        {
                            foreach (BridgeDA.tblOrderItem mOrderItems in (from a in db.tblOrderItems where a.InventoryOrderID == _id select a))
                            {
                                db1.tblOrderItems.Remove(mOrderItems);
                                db1.SubmitChanges();
                            }
                        }
                                //return response status as successfully deleted with member id  
                                return Request.CreateResponse(HttpStatusCode.OK, id);
                    }
                    else
                    {
                        //return response error as Not Found  with exception message.  
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Inventory OrderNot found " + id.ToString());
                    }
                }
            }

            catch (Exception ex)
            {

                //return response error as bad request  with exception message.  
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }
    }
}
