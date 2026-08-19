using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.Controllers
{
    public class FinanceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
