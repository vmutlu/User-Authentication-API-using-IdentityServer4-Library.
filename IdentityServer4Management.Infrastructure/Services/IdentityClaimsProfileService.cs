using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4Management.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer4Management.Infrastructure.Services
{
    //Token içerisinde yer alacak bilgileri istedğim gibi yapılandıracagım.
    public class IdentityClaimsProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        private readonly UserManager<ApplicationUser> _userManager;
        public IdentityClaimsProfileService(IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory, UserManager<ApplicationUser> userManager)
        {
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub).ConfigureAwait(false);
            var principal = await _userClaimsPrincipalFactory.CreateAsync(user).ConfigureAwait(false);

            //token içerisinde hangi bilgileri istiyorum ekleyim
            var claims = principal.Claims.ToList();
            claims = claims.Where(i => context.RequestedClaimTypes.Contains(i.Type)).ToList();
            claims.Add(new Claim(JwtClaimTypes.GivenName, user.Name)); //token oluştugunda kullanıcın adını ver
            claims.Add(new Claim(JwtClaimTypes.Id, user.Id.ToString()));
            claims.Add(new Claim("UserEmailAddress", user.Email));
            claims.Add(new Claim(JwtClaimTypes.Role, "user"));
            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub).ConfigureAwait(false);

            context.IsActive = user != null;
        }
    }
}
