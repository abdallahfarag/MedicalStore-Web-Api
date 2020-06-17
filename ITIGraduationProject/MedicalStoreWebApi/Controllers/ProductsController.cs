using MedicalStoreWebApi.Models;
using System;
using System.Collections.Generic;
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
            //string path;
            //string base64String = null;
            foreach (var item in Products)
            {
                //if(item.Image != null)
                //{

                    string base64String = string.Empty;
                    using (var img = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(item.Image))) 
{
                        using (var memStream = new MemoryStream())
                        {
                            img.Save(memStream, img.RawFormat);
                            byte[] imageBytes = memStream.ToArray();

                            base64String = Convert.ToBase64String(imageBytes);
                            item.Image = base64String;
                        }
                    }
                //}
            }

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

            string base64String = string.Empty;
            using (var img = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(product.Image)))
            {
                using (var memStream = new MemoryStream())
                {
                    img.Save(memStream, img.RawFormat);
                    byte[] imageBytes = memStream.ToArray();

                    base64String = Convert.ToBase64String(imageBytes);
                    product.Image = base64String;
                }
            }

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

            #region Restore base64 string to image
            byte[] bytes = Convert.FromBase64String(product.Image);
            MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length);
            ms.Write(bytes, 0, bytes.Length);
            Image image = Image.FromStream(ms, true);
            image.Save(HttpContext.Current.Server.MapPath($"~/Resources/{product.Name}{product.CategoryId}{product.Price}.png"), System.Drawing.Imaging.ImageFormat.Png);
            product.Image = $"~/Resources/{product.Name}{product.CategoryId}{product.Price}.png"; 
            #endregion

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

            #region Restore base64 string to image
            byte[] bytes = Convert.FromBase64String(product.Image);
            MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length);
            ms.Write(bytes, 0, bytes.Length);
            Image image = Image.FromStream(ms, true);
            image.Save(HttpContext.Current.Server.MapPath($"~/Resources/{product.Name}{product.CategoryId}{product.Price}.png"), System.Drawing.Imaging.ImageFormat.Png);
            product.Image = $"~/Resources/{product.Name}{product.CategoryId}{product.Price}.png";
            #endregion

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


