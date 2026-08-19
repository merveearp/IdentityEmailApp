using IdentityEmailApp.Entities;
using IdentityEmailApp.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.Controllers
{
    public class AISupportController : Controller
    {
        private readonly IAISupportService _aISupportService;
        private readonly UserManager<AppUser> _userManager;

        public AISupportController(IAISupportService aISupportService, UserManager<AppUser> userManager)
        {
            _aISupportService = aISupportService;
            _userManager = userManager;
        }

        public IActionResult Layout()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            ViewBag.UserName = user.Name;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(string question)
        {
            if (string.IsNullOrWhiteSpace(question))
            {
                return Json(new
                {
                    success = false,
                    message = "Lütfen bir mesaj yazın."
                });
            }

            var answer = await _aISupportService
                .GetSupportResponseAsync(question);

            return Json(new
            {
                success = true,
                answer
            });
        }

    }
}
