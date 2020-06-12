using MedicalStoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MedicalStoreWebApi.Controllers
{
  [Authorize(Roles ="Customer")]
    public class OrderController : ApiController
    {
        private MedicalStoreDbContext db;
            public OrderController()
        {
            db = new MedicalStoreDbContext();
        }
        [Authorize(Roles ="Admin,Customer")]
       //[AllowAnonymous]
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
       //[AllowAnonymous]
        public async Task<IHttpActionResult> GetOrder(int id)
        {
            var order =await db.Orders.FindAsync(id);
            if(order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        public async Task<IHttpActionResult> PostOrder( Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                await db.SaveChangesAsync();
                return Ok(order);
            }
            return BadRequest();
        }

        [Authorize(Roles = "Admin,Customer")]
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
    }
}
