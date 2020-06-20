﻿using MedicalStoreWebApi.Models;
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
        [Authorize(Roles ="Admin,Customer")]
        [Route("GetOrder")]
        public async Task<IHttpActionResult> GetOrder(int id)
        {
            var order =await db.Orders.FindAsync(id);
            if(order == null)
            {
                return NotFound();
            }
            return Ok(order);
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
            if (ModelState.IsValid)
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
                order.DateAdded = DateTime.Now.Date;
                order.OrderStatus = Orderstatus.Confirmed;
                order.OrderItems = new List<OrderItems>();
                foreach (var item in cartitems)
                {
                    order.OrderItems.Add(new OrderItems() { ProductId = item.ProductId, Quantity = item.Quantity });
                    db.Carts.Remove(item);
                }
                db.Orders.Add(order);
                await db.SaveChangesAsync();
                return Ok(order);
            }
            return BadRequest();
        }
    }
}
