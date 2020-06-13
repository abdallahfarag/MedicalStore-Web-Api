﻿using MedicalStoreWebApi.Models;
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
            context.Entry(cart).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok();
        }

        [Route("CartItem")]
        public async Task<IHttpActionResult> DeleteCartItem(string userId ,int ProductId)
        {
            var cartItem = context.Carts.SingleOrDefault(ww => ww.UserId.ToLower() == userId.ToLower() && ww.ProductId == ProductId);
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