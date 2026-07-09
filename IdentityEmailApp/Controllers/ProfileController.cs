using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult EditProfile()
        {
            return View();
        }

    }
}
