using IdentityEmailApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(LoginUserViewModel model)
        {
            return View();
        }
    }
}
