using Microsoft.AspNetCore.Identity;

namespace IdentityServer4Management.Infrastructure.Persistence
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string Name { get; set; }
    }
}
