using IdentityEmailApp.Context;
using IdentityEmailApp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityEmailApp.ViewComponents.DashboardViewComponents
{
    public class _DashbordWeeklyChartComponent : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly EmailContext _context;

        public _DashbordWeeklyChartComponent(
            UserManager<AppUser> userManager,
            EmailContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager
                .GetUserAsync(HttpContext.User);

            if (user?.Email == null)
            {
                ViewBag.ThisWeek = new int[7];
                ViewBag.LastWeek = new int[7];

                return View();
            }

            var today = DateTime.Today;

            // İçinde bulunduğumuz haftanın pazartesi günü
            var daysSinceMonday =
                ((int)today.DayOfWeek + 6) % 7;

            var thisWeekStart =
                today.AddDays(-daysSinceMonday);

            var nextWeekStart =
                thisWeekStart.AddDays(7);

            var lastWeekStart =
                thisWeekStart.AddDays(-7);

            var messageDates = await _context.Messages
                .Where(x =>
                    (x.SenderEmail == user.Email ||
                     x.ReceiverEmail == user.Email) &&
                    !x.IsDeleted &&
                    x.SendDate >= lastWeekStart &&
                    x.SendDate < nextWeekStart)
                .Select(x => x.SendDate)
                .ToListAsync();

            var thisWeekData = new int[7];
            var lastWeekData = new int[7];

            for (var dayIndex = 0; dayIndex < 7; dayIndex++)
            {
                var thisWeekDay =
                    thisWeekStart.AddDays(dayIndex);

                var lastWeekDay =
                    lastWeekStart.AddDays(dayIndex);

                thisWeekData[dayIndex] = messageDates.Count(x =>
                    x.Date == thisWeekDay.Date);

                lastWeekData[dayIndex] = messageDates.Count(x =>
                    x.Date == lastWeekDay.Date);
            }

            ViewBag.DayNames = new[]
            {
                "Pzt",
                "Sal",
                "Çar",
                "Per",
                "Cum",
                "Cmt",
                "Paz"
            };

            ViewBag.ThisWeek = thisWeekData;
            ViewBag.LastWeek = lastWeekData;

            return View();
        }
    }
}