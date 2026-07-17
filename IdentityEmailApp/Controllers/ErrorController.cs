using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/404")]
        public IActionResult Page404()
        {
            return View();
        }

        [Route("Error/401")]
        public IActionResult Page401()
        {
            return View();
        }

        [Route("Error/{statusCode}")]
        public IActionResult HandlerError(int statusCode)
        {
            if(statusCode==404)
            {
                return RedirectToAction("Page404");
            }
            if (statusCode == 401)
            {
                return RedirectToAction("Page401");
            }
            return View(statusCode);
        }

       

    }
}
