using MedicalStoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MedicalStoreWebApi.Controllers
{
    public class ContactUsController : ApiController
    {
        private MedicalStoreDbContext context;
        public ContactUsController()
        {
            context = new MedicalStoreDbContext();
        }
        [Authorize(Roles ="Admin")]
        public IHttpActionResult GetContactUs()
        {
            var contactUs = context.ContactUs.ToList();
            if(contactUs is null)
            {
                return NotFound();
            }
            return Ok(contactUs);
        }
        [AllowAnonymous]
        public IHttpActionResult PostContactUs(ContactUs contactUs)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            context.ContactUs.Add(contactUs);
            context.SaveChanges();
            return Created("Contact Message Added Successfully", contactUs);
        }
    }
}
