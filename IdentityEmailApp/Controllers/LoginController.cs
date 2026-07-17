using IdentityEmailApp.Context;
using IdentityEmailApp.Entities;
using IdentityEmailApp.Models.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace IdentityEmailApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly EmailContext _emailContext;


        public LoginController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, EmailContext emailContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailContext = emailContext;
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

            AppUser? user;

            if (model.UsernameOrEmail.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(model.UsernameOrEmail);
            }
            else
            {
                user = await _userManager.FindByNameAsync( model.UsernameOrEmail);
            }

            if (user == null)
            {
                ModelState.AddModelError( string.Empty, "Kullanıcı adı veya şifre hatalı.");

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
                user.UserName!,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: true
            );

            if (result.Succeeded)
            {
                if(!user.IsProfileSetupShown)
                {
                    return RedirectToAction("CompleteProfile", "Profile");
                }
                return RedirectToAction("Inbox", "Message");
               
            }

           
            ModelState.AddModelError( string.Empty, "Kullanıcı adı veya şifre hatalı." );

            return View(model);
        }
    }
}
