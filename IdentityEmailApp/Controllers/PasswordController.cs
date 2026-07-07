using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.Controllers
{
    public class PasswordController : Controller
    {
        public IActionResult ForgotPassword()
        {
            return View();
        }
    }
}
