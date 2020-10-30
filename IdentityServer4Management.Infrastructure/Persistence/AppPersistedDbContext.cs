using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using IdentityServer4.EntityFramework.Stores;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer4Management.Infrastructure.Persistence
{
    public class AppPersistedDbContext : PersistedGrantDbContext
    {
        public AppPersistedDbContext(DbContextOptions<PersistedGrantDbContext> options, OperationalStoreOptions storeOptions) : base(options, storeOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
