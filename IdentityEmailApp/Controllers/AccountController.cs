using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
