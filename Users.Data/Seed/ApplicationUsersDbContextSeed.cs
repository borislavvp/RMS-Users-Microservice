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
            var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
            context.Database.Migrate();
            await SeedAsync(context,userManager, roleManager);
        }

        private static async Task SeedAsync(
            ApplicationUsersDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            if (!context.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole(RoleConstants.ChorbaDeckAdmin));
                await roleManager.CreateAsync(new IdentityRole(RoleConstants.ChorbaDeckDriver));
                await roleManager.CreateAsync(new IdentityRole(RoleConstants.ChorbaDeckCustomer));

                if (!context.Users.Any())
                {
                    /**    Admin User   **/ 
                    var adminUser = new ApplicationUser
                    {
                        Email = "chorbadeckadmin@gmail.com",
                        Address = "Chorba Deck Address",
                        FirstName = "Chorba",
                        LastName = "Deck",
                        PhoneNumber = "089891232332",
                        Role = await roleManager.FindByNameAsync(RoleConstants.ChorbaDeckAdmin),
                        UserName = "chorbadeckadmin@gmail.com",
                        Subject = System.Guid.NewGuid().ToString()
                    };
                    var adminPassword = "@dm1nP@ssword1";
                    await userManager.CreateAsync(adminUser, adminPassword);
                    await userManager.AddToRoleAsync(adminUser, (RoleConstants.ChorbaDeckAdmin));

                    /**    Driver 1   **/
                    var driver1 = new ApplicationUser
                    {
                        Email = "chorbadeckdriver1@gmail.com",
                        Address = "Chorba Deck Driver 1 Address",
                        FirstName = "Chorba Driver",
                        LastName = "Deck",
                        PhoneNumber = "089891232332",
                        Role = await roleManager.FindByNameAsync(RoleConstants.ChorbaDeckDriver),
                        UserName = "chorbadeckdriver1@gmail.com",
                        Subject = System.Guid.NewGuid().ToString()
                    };
                    var driver1Password = "Dr1v3r1P@ssword1";
                    await userManager.CreateAsync(driver1, driver1Password);
                    await userManager.AddToRoleAsync(driver1, (RoleConstants.ChorbaDeckDriver));

                    /**    Driver 2   **/
                    var driver2 = new ApplicationUser
                    {
                        Email = "chorbadeckdriver2@gmail.com",
                        Address = "Chorba Deck Driver2 Address",
                        FirstName = "Chorba Driver",
                        LastName = "Deck",
                        PhoneNumber = "089891232332",
                        Role = await roleManager.FindByNameAsync(RoleConstants.ChorbaDeckDriver),
                        UserName = "chorbadeckdriver2@gmail.com",
                        Subject = System.Guid.NewGuid().ToString()
                    };
                    var driver2Password = "Dr1v3r2P@ssword1";
                    await userManager.CreateAsync(driver2, driver2Password);
                    await userManager.AddToRoleAsync(driver2, (RoleConstants.ChorbaDeckDriver));

                    /**    Driver 3   **/
                    var driver3 = new ApplicationUser
                    {
                        Email = "chorbadeckdriver3@gmail.com",
                        Address = "Chorba Deck Driver 3 Address",
                        FirstName = "Chorba Driver",
                        LastName = "Deck",
                        PhoneNumber = "089891232332",
                        Role = await roleManager.FindByNameAsync(RoleConstants.ChorbaDeckDriver),
                        UserName = "chorbadeckdriver3@gmail.com",
                        Subject = System.Guid.NewGuid().ToString()
                    };
                    var driver3Password = "Dr1v3r3P@ssword1";
                    await userManager.CreateAsync(driver3, driver3Password);
                    await userManager.AddToRoleAsync(driver3, (RoleConstants.ChorbaDeckDriver));
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
