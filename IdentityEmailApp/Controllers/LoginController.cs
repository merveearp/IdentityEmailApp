using IdentityEmailApp.Entities;
using IdentityEmailApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DiaSymReader;

namespace IdentityEmailApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public LoginController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LoginUserViewModel model)
        {
           
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            AppUser user = null;

            if(model.UsernameOrEmail.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(model.UsernameOrEmail);
            }
            else
            {
                user =await _userManager.FindByNameAsync(model.UsernameOrEmail);
            }  
            
            if(user==null)
            {
                ModelState.AddModelError("", "Kullanıcı adı vey şifre hatalı!");
                return View(model);

            }

            var result = await _signInManager.PasswordSignInAsync
                (
                user.UserName,
                model.Password,
                model.RememberMe,true);
            
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");
            return View(model);
        }
    }
}
