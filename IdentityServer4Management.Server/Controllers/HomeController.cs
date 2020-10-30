using Microsoft.AspNetCore.Mvc;

namespace IdentityServer4Management.Server.Controllers
{
    public class HomeController : Controller
    {

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
