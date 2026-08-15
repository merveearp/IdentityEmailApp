using IdentityEmailApp.Context;
using IdentityEmailApp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityEmailApp.ViewComponents.DashboardViewComponents
{
    public class _DashboardNotificationComponent : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly EmailContext _emailContext;

        public _DashboardNotificationComponent(
            UserManager<AppUser> userManager,
            EmailContext emailContext)
        {
            _userManager = userManager;
            _emailContext = emailContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager
                .GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return View(new List<Notification>());
            }

            var notifications = await _emailContext.Notifications
                .AsNoTracking()
                .Where(x => x.AppUserId == user.Id)
                .OrderByDescending(x => x.CreatedDate)
                .Take(5)
                .ToListAsync();

            return View(notifications);
        }
    }
}