using IdentityEmailApp.Context;
using IdentityEmailApp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityEmailApp.ViewComponents.NavbarHeaderViewComponents
{
    public class _NotificationListOnNavbarComponent : ViewComponent
    {
        private readonly EmailContext _context;
        private readonly UserManager<AppUser> _userManager;

        public _NotificationListOnNavbarComponent(
            EmailContext context,
            UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity!.Name!);

            if (user == null)
                return View(new List<Notification>());

            var values = await _context.Notifications
                .Where(x => x.AppUserId == user.Id && x.IsRead==false)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            ViewBag.NotificationCount = values.Count(x => !x.IsRead);

            return View(values);
        }
    }
}