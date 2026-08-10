using IdentityEmailApp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.ViewComponents.DashboardViewComponents
{
    public class _DashboardHeaderComponent:ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;

        public _DashboardHeaderComponent(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
           
            if (user == null)
            {
                ViewBag.FullName = "Kullanıcı";
                ViewBag.City = null;

                return View();
            }

            ViewBag.FullName = user.Name+ " " +user.Surname; ;
            ViewBag.City = user.City ;
            ViewBag.Email = user.Email ;
            return View();
        }
    }
}
