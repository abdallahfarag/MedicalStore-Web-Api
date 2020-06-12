using MedicalStoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity.Migrations;
using System.Web;

namespace MedicalStoreWebApi.Controllers
{
    [Authorize(Roles ="Admin")]
    public class CategoriesController : ApiController
    {
        private MedicalStoreDbContext db;
        public CategoriesController()
        {
            db = new MedicalStoreDbContext();
        }


        // GET: api/Categories
        [AllowAnonymous]
        public IHttpActionResult GetCategories()
        {
            var categories = db.Categories.ToList();

             if(categories.Count == 0)
             {
                return NotFound();
             }

             return Ok(categories);
        
        }

        // GET: api/Categories/5
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetCategory(int id)
        {
            var category = await db.Categories.FindAsync(id);
            
            if(category is null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // POST: api/Categories
        public async Task<IHttpActionResult> PostAddCat([FromBody]Category category)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            db.Categories.Add(category);
            await db.SaveChangesAsync();
            return Created("created successfully", category);
        }

        // PUT: api/Categories/5
        public async Task<IHttpActionResult> PutEditCat(Category category)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            var catChk = await db.Categories.FindAsync(category.Id);

            if(catChk is null)
            {
                return NotFound();
            }

            db.Categories.AddOrUpdate(category);
            await db.SaveChangesAsync();

            return Created("updated successfully", category);
        }

        // DELETE: api/Categories/5
        public async Task<IHttpActionResult> DeleteCat(int id)
        {
            var category = await db.Categories.FindAsync(id);
            
            if(category is null)
            {
                NotFound();
            }

            db.Categories.Remove(category);
            await db.SaveChangesAsync();

            return Ok("Deleted successfully");
        }


        [HttpPost]
        [Route("api/Product/UploadImage")]
        public HttpResponseMessage UploadImage()
        {
            // Check if the request contains multipart/form-data. 
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {

                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var fileName = postedFile.FileName.Substring(0, postedFile.FileName.IndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {
                            var message = $"Please Upload image of type .jpg,.gif,.png.";

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = $"Please Upload a file upto 1 mb.";

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {
                            var filePath = HttpContext.Current.Server.MapPath($"~/App_Data/Images/{fileName}");
                            //var filePath = HttpContext.Current.Server.MapPath($"~/App_Data/Images/{fileName}_{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{extension}");

                            postedFile.SaveAs(filePath);
                        }
                    }

                    var message1 = $"Image Updated Successfully.";
                    return Request.CreateErrorResponse(HttpStatusCode.Created, message1); ;
                }
                var res = $"Please Upload a image.";
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            catch
            {
                var res = $"something is wrong";
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }
    }
}
