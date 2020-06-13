using MedicalStoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;

namespace MedicalStoreWebApi.Controllers
{
    [Authorize(Roles ="Customer,Admin")]
    [RoutePrefix("api/OrdersItems")]
    public class OrderItemsController : ApiController
    {
        private MedicalStoreDbContext context;
        public OrderItemsController()
        {
            context = new MedicalStoreDbContext();
        }

        [Route("OrdersItems")]
        public IHttpActionResult GetOrdersItems()
        {
            var orderItems = context.OrderItems.ToList();
            if (orderItems is null)
            {
                return NotFound();
            }
            return Ok(orderItems);
        }

        [Route("OrderItems")]
        public IHttpActionResult GetOrderItems(int orderId)
        {
            var orderItems = context.OrderItems.Where(ww => ww.OrderId == orderId).ToList();
            if (orderItems is null)
            {
                return NotFound();
            }
            return Ok(orderItems);
        }

        [Route("OrderItem")]
        public async Task<IHttpActionResult> PostOrderItem(OrderItems orderItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Cart");
            }
            context.OrderItems.Add(orderItem);
            await context.SaveChangesAsync();
            return Created("Order item added successfully", orderItem);
        }

        [Route("OrderItem")]
        public async Task<IHttpActionResult> PutOrderItem(OrderItems orderItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid Order item");
            }
            context.Entry(orderItem).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok();
        }

        [Route("OrderItem")]
        public async Task<IHttpActionResult> DeleteOrderItem(int orderId, int productId)
        {
            var orderItem = context.OrderItems.SingleOrDefault(ww => ww.OrderId == orderId && ww.ProductId == productId);
            context.OrderItems.Remove(orderItem);
            await context.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("OrdertItems")]
        public async Task<IHttpActionResult> DeleteOrderItems(int orderId)
        {
            var orderItems = context.OrderItems.Where(ww => ww.OrderId == orderId).ToList();
            foreach (var item in orderItems)
            {
                context.OrderItems.Remove(item);
            }
            await context.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);

        }
    }
}
