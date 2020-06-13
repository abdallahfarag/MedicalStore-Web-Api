using System;
using System.Collections.Generic;
using System.Linq;
using MedicalStoreWebApi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(MedicalStoreWebApi.Startup))]

namespace MedicalStoreWebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRoles();
        }

        public void CreateRoles()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(MedicalStoreDbContext.Create()));
            IdentityRole role;
            if (!roleManager.RoleExists("Customer"))
            {
                role = new IdentityRole { Name = "Customer" };
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Admin"))
            {
                role = new IdentityRole { Name = "Admin" };
                roleManager.Create(role);
            }
        }
    }
}
