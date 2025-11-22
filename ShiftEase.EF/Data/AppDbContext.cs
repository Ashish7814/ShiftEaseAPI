
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShiftEase.EF.Models;


namespace ShiftEase.EF.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed roles
            builder.Entity<ApplicationRole>().HasData(
                new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Admin", NormalizedName = "ADMIN", Description = "Administrator role" },
                new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Manager", NormalizedName = "MANAGER", Description = "Manager role" },
                new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "User", NormalizedName = "USER", Description = "User role" }
            );
        }
    }
}
