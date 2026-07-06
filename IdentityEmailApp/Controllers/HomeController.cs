using IdentityEmailApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IdentityEmailApp.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
       
    }
}
