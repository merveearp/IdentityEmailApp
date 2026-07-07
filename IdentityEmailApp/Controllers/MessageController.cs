using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.Controllers
{
    public class MessageController : Controller
    {
        public IActionResult Inbox()
        {
            return View();
        }
    }
}
