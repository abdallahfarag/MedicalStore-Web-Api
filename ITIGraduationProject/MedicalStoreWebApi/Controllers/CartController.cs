using MedicalStoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Management;
using System.Net;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace MedicalStoreWebApi.Controllers
{
    [Authorize(Roles ="Customer")]
    [RoutePrefix("api/Cart")]
    public class CartController : ApiController
    {
        private MedicalStoreDbContext context;

        public CartController()
        {
            context = new MedicalStoreDbContext();
        }

        [Route("CartItems")]
        public IHttpActionResult GetCartItems()
        {
            var cartItems = context.Carts.ToList();
            if(cartItems is null)
            {
                return NotFound();
            }
            return Ok(cartItems);
        }

        [Route("CartUserItem")]
        public IHttpActionResult GetCartUserItems(string userId)
        {
            var userItems = context.Carts.Where(ww => ww.UserId.ToLower() == userId.ToLower()).ToList();
            if(userItems is null)
            {
                return NotFound();
            }
            return Ok(userItems);
        }

        [Route("CartItem")]
        public async Task<IHttpActionResult> PostCartItem(Cart cart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Cart");
            }
            Product productAdded = context.Products.Find(cart.ProductId);
            productAdded.QuantityInStock -= cart.Quantity;
            context.Entry(productAdded).State = EntityState.Modified;
            
            context.Carts.Add(cart);
            await context.SaveChangesAsync();
            return Created("Cart item added successfully",cart);
        }

        [Route("CartItem")]
        public async Task<IHttpActionResult> PutCartItem(Cart cart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid cart item");
            }
            var cartLastItem = context.Carts.SingleOrDefault(i => i.UserId.ToLower() == cart.UserId.ToLower() && i.ProductId == cart.ProductId);
            Product product = context.Products.Find(cart.ProductId);
            if(cart.Quantity > cartLastItem.Quantity)
            {
                var differnce = cart.Quantity - cartLastItem.Quantity;
                product.QuantityInStock -= differnce;
                context.Entry(product).State = EntityState.Modified;

                //await context.SaveChangesAsync();
                //context.Products.Find(cart.ProductId).QuantityInStock -= differnce;
            }
            if (cart.Quantity < cartLastItem.Quantity)
            {
                var differnce = cartLastItem.Quantity - cart.Quantity ;
                product.QuantityInStock += differnce;
                context.Entry(product).State = EntityState.Modified;
                //await context.SaveChangesAsync();
                //context.Products.Find(cart.ProductId).QuantityInStock += differnce;
            }
            cartLastItem.Quantity = cart.Quantity;
            context.Entry(cartLastItem).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok();
        }

        [Route("CartItem")]
        public async Task<IHttpActionResult> DeleteCartItem(string userId ,int ProductId)
        {
            var cartItem = context.Carts.SingleOrDefault(ww => ww.UserId.ToLower() == userId.ToLower() && ww.ProductId == ProductId);
            Product product = context.Products.Find(ProductId);
            product.QuantityInStock += cartItem.Quantity;
            context.Entry(product).State = EntityState.Modified;
            //context.Products.Find(ProductId).QuantityInStock += cartItem.Quantity;
            context.Carts.Remove(cartItem);
            await context.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("CartItems")]
        public async Task<IHttpActionResult> DeleteCartUserItems(string userId)
        {
            var userCartItems = context.Carts.Where(ww => ww.UserId.ToLower() == userId.ToLower()).ToList();
            foreach (var item in userCartItems)
            {
                context.Carts.Remove(item);
            }
            await context.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);

        }


    }
}