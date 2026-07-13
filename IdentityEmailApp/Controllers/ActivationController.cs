using IdentityEmailApp.Context;
using IdentityEmailApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityEmailApp.Controllers
{
    public class ActivationController : Controller
    {
        private readonly EmailContext _emailContext;

        public ActivationController(EmailContext emailContext)
        {
            _emailContext = emailContext;
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

            TempData["SuccessMessage"] =
                "Hesabınız başarıyla doğrulandı. Giriş yapabilirsiniz.";

            return RedirectToAction("SignIn", "Login");
        }
    }
}
