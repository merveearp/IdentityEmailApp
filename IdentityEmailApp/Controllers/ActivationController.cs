using IdentityEmailApp.Context;
using IdentityEmailApp.Entities;
using IdentityEmailApp.Enums;
using IdentityEmailApp.Models.UserModels;
using IdentityEmailApp.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityEmailApp.Controllers
{
    public class ActivationController : Controller
    {
        private readonly EmailContext _emailContext;
        private readonly ISystemEventService _systemEventService;
        private readonly UserManager<AppUser> _userManager;

        public ActivationController(EmailContext emailContext, ISystemEventService systemEventService, UserManager<AppUser> userManager)
        {
            _emailContext = emailContext;
            _systemEventService = systemEventService;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult UserActivation()
        {
            var email = TempData["EmailMove"]?.ToString();

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Signup", "Register");
            }

            TempData.Keep("EmailMove");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserActivation(UserActivationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData.Keep("EmailMove");
                return View(model);
            }

            var email = TempData["EmailMove"]?.ToString();

            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Aktivasyon işlemi için kullanıcı bilgisi bulunamadı."
                );

                return View(model);
            }

            var user = await _emailContext.Users
                .FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Kullanıcı bulunamadı."
                );

                return View(model);
            }

            if (model.ActivationCode != user.ActivationCode)
            {
                ModelState.AddModelError(
                    nameof(model.ActivationCode),
                    "Girdiğiniz aktivasyon kodu hatalı."
                );

                TempData.Keep("EmailMove");

                return View(model);
            }

            user.EmailConfirmed = true;
            user.ActivationCode = 0;

            await _emailContext.SaveChangesAsync();

            var isMember = await _userManager.IsInRoleAsync(user, "Member");

            if (!isMember)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "Member");

                if (!roleResult.Succeeded)
                {
                    foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    TempData.Keep("EmailMove");
                    return View(model);
                }
            }


            TempData["SuccessMessage"] =
                "Hesabınız başarıyla doğrulandı. Giriş yapabilirsiniz.";

            await _systemEventService.CreateAsync(user, NotificationType.EmailVerified);


            return RedirectToAction("SignIn", "Login");
        }
    }
}
