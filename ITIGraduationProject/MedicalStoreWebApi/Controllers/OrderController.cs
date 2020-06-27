using MedicalStoreWebApi.Models;
using MedicalStoreWebApi.Models.Enums;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MedicalStoreWebApi.Controllers
{
    [RoutePrefix("api/Order")]
    public class OrderController : ApiController
    {
        private MedicalStoreDbContext db;
            public OrderController()
        {
            db = new MedicalStoreDbContext();
        }
        [Authorize(Roles ="Admin,Customer")]
        [Route("GetAllOrders")]
        public IHttpActionResult GetOrders()
        {
            var order = db.Orders.ToList();
            if (order.Count == 0)
            {
                return NotFound();
            }
            return Ok(order);
        }
        [Authorize(Roles = "Admin,Customer")]
        [Route("GetOrder")]
        public async Task<IHttpActionResult> GetOrder(int id)
        {
            var order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [Authorize(Roles = "Admin,Customer")]
        [Route("GetUserOrders")]
        public IHttpActionResult GetUserOrder(string userId)
        {
            var userOrders =  db.Orders.Where(ww => ww.UserId.ToLower() == userId.ToLower()).ToList();
            if (userOrders == null)
            {
                return NotFound();
            }
            return Ok(userOrders);
        }

        //[Authorize(Roles = "Customer")]
        //public async Task<IHttpActionResult> PostOrder( Order order)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Orders.Add(order);
        //        await db.SaveChangesAsync();
        //        return Ok(order);
        //    }
        //    return BadRequest();
        //}

        [Authorize(Roles = "Admin,Customer")]
        [Route("DeleteOrder")]
        public async Task<IHttpActionResult> DeleteOrder(int id)
        {
            var order = await db.Orders.FindAsync(id);
            if(order == null)
            {
                return NotFound();
            }
            db.Orders.Remove(order);
            await db.SaveChangesAsync();

            return Ok();
        }
        [Authorize(Roles ="Admin,Customer")]
        [Route("EditOrder")]
        public async Task<IHttpActionResult> PutOrder(Order order)
        {
           // var ord = await db.Orders.FindAsync(order.Id);
            if (ModelState.IsValid)
            {
                db.Entry(order).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();
                return Ok(order);
            }
           
            return BadRequest();
        }

        [Route("AddOrderWithItems")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IHttpActionResult> PostOrderWithItems(Order order)
        {
            var userId = User.Identity.GetUserId();
            order.UserId = userId;
            var cartitems = db.Carts.Where(i => i.UserId.ToLower() == userId.ToLower()).ToList();
            decimal totalprice = 0;
            foreach (var item in cartitems)
            {
                totalprice += item.Quantity * db.Products.Find(item.ProductId).Price;
            }
            order.TotalPrice = totalprice;
            order.DateAdded = DateTime.Now;
            order.OrderStatus = Orderstatus.Confirmed;
            order.OrderItems = new List<OrderItems>();
            foreach (var item in cartitems)
            {
                order.OrderItems.Add(new OrderItems() { ProductId = item.ProductId, Quantity = item.Quantity });
                db.Carts.Remove(item);
            }
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                await db.SaveChangesAsync();
                return Ok(order);
            }
            return BadRequest();
        }


        [Authorize(Roles = "Admin,Customer")]
        [Route("EditStatus")]
        public async Task<IHttpActionResult> PutStatus(Order order)
        {
             
            if (order.Id != 0)
            {
                var orderResult = await db.Orders.FindAsync(order.Id);
                
                //modifying cancel status is denied 
                if(orderResult.OrderStatus == Orderstatus.Cancelled)
                {
                    return BadRequest();
                }

                //increase the stock quantity if order is cancelled  
                if(order.OrderStatus == Orderstatus.Cancelled)
                {
                    var orderItems = db.OrderItems.Where(o => o.OrderId == order.Id).ToList();
                    var allProducts = db.Products;
                    foreach(var item in orderItems)
                    {
                        var products = allProducts.Where(p => p.Id == item.ProductId).ToList();
                        foreach(var product in products)
                        {
                            if(item.ProductId == product.Id)
                            {
                                product.QuantityInStock += item.Quantity;
                            }
                        }
                    }
                }

                orderResult.OrderStatus = order.OrderStatus;
                await db.SaveChangesAsync();
                return Ok(order);
            }

            return BadRequest();
        }
    }
}
