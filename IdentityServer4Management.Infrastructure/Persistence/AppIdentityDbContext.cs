using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer4Management.Infrastructure.Persistence
{
    public class AppIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(new IdentityRole { Name ="VMUTLU"});
            base.OnModelCreating(builder);
        }
    }
}
