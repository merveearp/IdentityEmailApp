using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
