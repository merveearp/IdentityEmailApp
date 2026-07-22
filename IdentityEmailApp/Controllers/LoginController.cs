using IdentityEmailApp.Context;
using IdentityEmailApp.Entities;
using IdentityEmailApp.Enums;
using IdentityEmailApp.Models.UserModels;
using IdentityEmailApp.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace IdentityEmailApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly EmailContext _emailContext;
        private readonly ISystemEventService _systemEventService;


        public LoginController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, EmailContext emailContext, ISystemEventService systemEventService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailContext = emailContext;
            _systemEventService = systemEventService;
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(LoginUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usernameOrEmail = model.UsernameOrEmail.Trim();

            AppUser? user;

            if (usernameOrEmail.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(usernameOrEmail);
            }
            else
            {
                user = await _userManager.FindByNameAsync(usernameOrEmail);
            }

            if (user == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Kullanıcı adı, e-posta adresi veya şifre hatalı."
                );

                return View(model);
            }

            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Giriş yapmadan önce e-posta adresinizi doğrulamalısınız."
                );

                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: true
            );

            if (result.Succeeded)
            {
                if (!user.IsProfileSetupShown)
                {
                    await _systemEventService.CreateAsync(user, NotificationType.NewUser);
                    return RedirectToAction("CompleteProfile", "Profile");
                }

                await _systemEventService.CreateAsync(user, NotificationType.LoginSucceeded);
                
                return RedirectToAction("Inbox", "Message");
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Çok fazla başarısız giriş denemesi yapıldığı için hesabınız geçici olarak kilitlenmiştir."
                );

                return View(model);
            }

            ModelState.AddModelError(
                string.Empty,
                "Kullanıcı adı, e-posta adresi veya şifre hatalı."
            );

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("SignIn", "Login");
        }

    }
}
