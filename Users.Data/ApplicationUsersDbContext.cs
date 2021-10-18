using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using Users.Data.Models;

namespace Users.Data
{
    public class ApplicationUsersDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationUsersDbContext(DbContextOptions<ApplicationUsersDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
               .HasIndex(user => user.Subject)
                 .IsUnique();
        }
    }
}
