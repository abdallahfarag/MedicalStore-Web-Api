using MedicalStoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace MedicalStoreWebApi.Controllers
{   
    //[Authorize(Roles ="Admin")]
    public class ProductsController : ApiController
    {
        private MedicalStoreDbContext db;
        public ProductsController()
        {
            db = new MedicalStoreDbContext();
        }

        // GET: api/Products
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetProducts()
        {
            var Products = await db.Products.ToListAsync();

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
        public async Task<IHttpActionResult> PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                db.Products.Add(product);
                await db.SaveChangesAsync();
                return Created("created successfully", product);
            } catch
            {
                return BadRequest();
            }
           
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


        [HttpPost]
        [Route("api/Products/UploadImage/{ProductName}")]
        public async Task<IHttpActionResult> UploadImage(string ProductName)
        {
            // Check if the request contains multipart/form-data. 
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            try
            {
                var httpRequest = HttpContext.Current.Request;
                HttpPostedFile postedFile = httpRequest.Files[0];

                var guid = Guid.NewGuid().ToString();
                var filePath = HttpContext.Current.Server.MapPath($"~/Resources/Images/{guid}.jpeg");

                postedFile.SaveAs(filePath);

                var product = db.Products.SingleOrDefault(p => p.Name == ProductName);
                product.Image = $"{guid}.jpeg";
                await db.SaveChangesAsync();

                return Created("created successfully", ProductName);
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpPost]
        [Route("api/Products/UniqueName/{ProductName}")]
        public IHttpActionResult UniqueName(string ProductName)
        {
            var isExist = db.Products.Any(p => p.Name == ProductName);

            if (isExist)
            {
                return BadRequest();
            }

            return Ok();

        }
    }
}


