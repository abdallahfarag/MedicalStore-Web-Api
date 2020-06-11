using MedicalStoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MedicalStoreWebApi.Controllers
{   
    [Authorize(Roles ="Admin")]
    public class ProductsController : ApiController
    {
        private MedicalStoreDbContext db;
        public ProductsController()
        {
            db = new MedicalStoreDbContext();
        }

        // GET: api/Products
        [AllowAnonymous]
        public IHttpActionResult GetProducts()
        {
            var Products = db.Products.ToList();

            if (Products.Count == 0)
            {
                return NotFound();
            }

            return Ok(Products);

        }

        // GET: api/Products/5
        [AllowAnonymous]
        public async Task<IHttpActionResult> Getproduct(int id)
        {
            var product = await db.Products.FindAsync(id);

            if (product is null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // POST: api/Products
        public async Task<IHttpActionResult> PostAddProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            db.Products.Add(product);
            await db.SaveChangesAsync();
            return Created("created successfully", product);
        }

        // PUT: api/Products/5
        public async Task<IHttpActionResult> PutEditProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var catChk = await db.Products.FindAsync(product.Id);

            if (catChk is null)
            {
                return NotFound();
            }

            db.Products.AddOrUpdate(product);
            await db.SaveChangesAsync();

            return Created("updated successfully", product);
        }

        // DELETE: api/Products/5
        public async Task<IHttpActionResult> DeleteProduct(int id)
        {
            var product = await db.Products.FindAsync(id);

            if (product is null)
            {
                NotFound();
            }

            db.Products.Remove(product);
            await db.SaveChangesAsync();

            return Ok("Deleted successfully");
        }
    }
}
