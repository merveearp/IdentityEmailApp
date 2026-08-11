using IdentityEmailApp.Context;
using IdentityEmailApp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityEmailApp.ViewComponents.DashboardViewComponents
{
    public class _DashboardPie2Component : ViewComponent
    {
        private readonly EmailContext _emailContext;
        private readonly UserManager<AppUser> _userManager;

        public _DashboardPie2Component(
            EmailContext emailContext,
            UserManager<AppUser> userManager)
        {
            _emailContext = emailContext;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager
                .GetUserAsync(HttpContext.User);

            if (user == null)
            {
                SetEmptyValues();
                return View();
            }

            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var tasks = await _emailContext.UserTasks
                .AsNoTracking()
                .Where(x =>
                    x.AppUserId == user.Id &&
                    !x.IsDeleted)
                .ToListAsync();

            // Toplam görev
            ViewBag.TotalTasks = tasks.Count;

            // Tamamlanan görevler
            ViewBag.CompletedTasks =
                tasks.Count(x => x.IsCompleted);

            // Son tarihi geçmiş ve tamamlanmamış görevler
            ViewBag.OverdueTasks =
                tasks.Count(x =>
                    !x.IsCompleted &&
                    x.DueDate.HasValue &&
                    x.DueDate.Value.Date < today);

            // Bugün tamamlanması gereken görevler
            ViewBag.TodayTasks =
                tasks.Count(x =>
                    !x.IsCompleted &&
                    x.DueDate.HasValue &&
                    x.DueDate.Value.Date == today);

            // Gelecek tarihli aktif görevler
            ViewBag.UpcomingTasks =
                tasks.Count(x =>
                    !x.IsCompleted &&
                    x.DueDate.HasValue &&
                    x.DueDate.Value.Date > today);

            // Son tarihi bulunmayan aktif görevler
            ViewBag.NoDueDateTasks =
                tasks.Count(x =>
                    !x.IsCompleted &&
                    !x.DueDate.HasValue);

            // Önemli ve tamamlanmamış görevler
            ViewBag.ImportantTasks =
                tasks.Count(x =>
                    !x.IsCompleted &&
                    x.IsImportant);

            // Yarın tamamlanması gereken görevler
            ViewBag.TomorrowTasks =
                tasks.Count(x =>
                    !x.IsCompleted &&
                    x.DueDate.HasValue &&
                    x.DueDate.Value.Date == tomorrow);

            return View();
        }

        private void SetEmptyValues()
        {
            ViewBag.TotalTasks = 0;
            ViewBag.CompletedTasks = 0;
            ViewBag.OverdueTasks = 0;
            ViewBag.TodayTasks = 0;
            ViewBag.UpcomingTasks = 0;
            ViewBag.NoDueDateTasks = 0;
            ViewBag.ImportantTasks = 0;
            ViewBag.TomorrowTasks = 0;
        }
    }
}