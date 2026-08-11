using IdentityEmailApp.Context;
using IdentityEmailApp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityEmailApp.ViewComponents.DashboardViewComponents
{
    public class _DashboardStatisticsComponent :ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly EmailContext _context;

        public _DashboardStatisticsComponent(UserManager<AppUser> userManager, EmailContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                ViewBag.TodayMessageCount = 0;
                ViewBag.UnreadMessageCount = 0;
                ViewBag.UnreadNotificationCount = 0;
                ViewBag.ActiveTaskCount = 0;

                return View();
            }
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var monthStart = new DateTime
                (today.Year,
                today.Month,
                1);

            var nextMonth = monthStart.AddMonths(1);

            ViewBag.TodayMessageCount = await _context.Messages.CountAsync(x =>
                x.ReceiverEmail == user.Email &&
                !x.IsDeleted &&
                !x.IsSpam &&
                !x.IsDraft &&
                x.SendDate >= today &&
                x.SendDate < tomorrow);

            ViewBag.UnreadMessageCount =
              await _context.Messages.CountAsync(x =>
                  x.ReceiverEmail == user.Email &&
                  !x.IsRead &&
                  !x.IsDeleted &&
                  !x.IsSpam &&
                  !x.IsDraft);

            ViewBag.UnreadNotificationCount =
               await _context.Notifications.CountAsync(x =>
                   x.AppUserId == user.Id &&
                   !x.IsRead);

            ViewBag.ActiveTaskCount =
                await _context.UserTasks.CountAsync(x =>
                    x.AppUserId == user.Id &&
                    !x.IsDeleted &&
                    !x.IsCompleted);


            return View();
        }
    }
}
