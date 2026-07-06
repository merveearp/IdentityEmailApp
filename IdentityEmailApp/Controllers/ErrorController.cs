using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
