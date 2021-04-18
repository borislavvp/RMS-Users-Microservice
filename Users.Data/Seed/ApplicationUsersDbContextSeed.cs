using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Storage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.Data.Configuration;
using Users.Data.Models;

namespace Users.Data.Seed
{
    public static class ApplicationUsersDbContextSeed
    {
        public static async Task SeedData(IServiceScope scope)
        {
            var context = scope.ServiceProvider.GetService<ApplicationUsersDbContext>();
            var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            context.Database.Migrate();
            await SeedAsync(context,userManager);
        }

        private static async Task SeedAsync(
            ApplicationUsersDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            if(!context.Roles.Any())
            {
                var adminRole = new IdentityRole(RoleConstants.ChorbaDeckAdmin);
                var customerRole = new IdentityRole(RoleConstants.ChorbaDeckCustomer);

                await context.Roles.AddRangeAsync(new IdentityRole[] { adminRole, customerRole });

                if (!context.Users.Any())
                {
                    var adminUser = new ApplicationUser
                    {
                        Email = "chorbadeckadmin@gmail.com",
                        Address = "Chorba Deck Address",
                        FirstName = "Chorba",
                        LastName = "Deck",
                        PhoneNumber = "089891232332",
                        Role = adminRole,
                        UserName = "chorbadeckadmin@gmail.com",
                        Subject = System.Guid.NewGuid().ToString()
                    };
                    var adminPassword = "@dm1nP@ssword1";
                    await userManager.CreateAsync(adminUser, adminPassword);
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
