using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace MedicalStoreWebApi.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public bool IsDeleted { get; set; }
        public string Address { get; set; }
        public string  Gender { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

    }

    public class MedicalStoreDbContext : IdentityDbContext<ApplicationUser>
    {
        public MedicalStoreDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public static MedicalStoreDbContext Create()
        {
            return new MedicalStoreDbContext();
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<OrderItems> OrderItems { get; set; }
        public virtual DbSet<ContactUs> ContactUs { get; set; }



    }
}