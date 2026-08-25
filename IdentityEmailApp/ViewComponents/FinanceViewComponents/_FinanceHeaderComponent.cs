using IdentityEmailApp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.ViewComponents.FinanceViewComponents
{
    public class _FinanceHeaderComponent:ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;

        public _FinanceHeaderComponent(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user != null)
            {
                ViewBag.FullName = $"{user.Name} {user.Surname}";
                ViewBag.ImageUrl = user.ImageUrl;
            }

            return View();
        }
    }
}
