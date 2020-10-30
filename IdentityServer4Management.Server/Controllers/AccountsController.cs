using IdentityServer4Management.Infrastructure.Persistence;
using IdentityServer4Management.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer4Management.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("UserRegister")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestViewModel registerRequestView)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userInformation = new ApplicationUser { UserName = registerRequestView.EMail, Name = registerRequestView.Name, Email = registerRequestView.EMail };
            var result = await _userManager.CreateAsync(userInformation, registerRequestView.Password).ConfigureAwait(false);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddClaimAsync(userInformation, new Claim("userName", userInformation.UserName));
            await _userManager.AddClaimAsync(userInformation, new Claim("name", userInformation.Name));
            await _userManager.AddClaimAsync(userInformation, new Claim("email", userInformation.Email));
            await _userManager.AddClaimAsync(userInformation, new Claim("role", "user"));
            return Ok();
        }
    }
}
