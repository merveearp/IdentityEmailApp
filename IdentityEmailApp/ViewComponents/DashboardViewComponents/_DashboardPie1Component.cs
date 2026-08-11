using IdentityEmailApp.Context;
using IdentityEmailApp.Entities;
using IdentityEmailApp.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityEmailApp.ViewComponents.DashboardViewComponents
{
    public class _DashboardPie1Component : ViewComponent
    {
        private readonly EmailContext _emailContext;
        private readonly UserManager<AppUser> _userManager;

        public _DashboardPie1Component(
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

            var notifications = await _emailContext.Notifications
                .AsNoTracking()
                .Where(x => x.AppUserId == user.Id)
                .ToListAsync();

            // Genel özetler
            ViewBag.TotalNotifications =
                notifications.Count;

            ViewBag.UnreadNotifications =
                notifications.Count(x => !x.IsRead);

            ViewBag.TodayNotifications =
                notifications.Count(x =>
                    x.CreatedDate.Date == today);

            // Hesap bildirimleri
            ViewBag.AccountNotifications =
                notifications.Count(x =>
                    x.NotificationType == NotificationType.AccountCreated ||
                    x.NotificationType == NotificationType.EmailVerified ||
                    x.NotificationType == NotificationType.PasswordChanged ||
                    x.NotificationType == NotificationType.PasswordResetRequested);

            // Profil bildirimleri
            ViewBag.ProfileNotifications =
                notifications.Count(x =>
                    x.NotificationType == NotificationType.ProfileUpdated ||
                    x.NotificationType == NotificationType.ProfilePhotoUpdated ||
                    x.NotificationType == NotificationType.ProfileCompletionReminder);

            // Mesaj bildirimleri
            ViewBag.MessageNotifications =
                notifications.Count(x =>
                    x.NotificationType == NotificationType.NewMessageReceived);

            // Güvenlik ve giriş bildirimleri
            ViewBag.SecurityNotifications =
                notifications.Count(x =>
                    x.NotificationType == NotificationType.LoginSucceeded ||
                    x.NotificationType == NotificationType.LoginFailed ||
                    x.NotificationType == NotificationType.SecurityAlert);

            // Sistem ve yönetim bildirimleri
            ViewBag.SystemNotifications =
                notifications.Count(x =>
                    x.NotificationType == NotificationType.RoleAssigned ||
                    x.NotificationType == NotificationType.NewUser ||
                    x.NotificationType == NotificationType.WelcomeTip);

            return View();
        }

        private void SetEmptyValues()
        {
            ViewBag.TotalNotifications = 0;
            ViewBag.UnreadNotifications = 0;
            ViewBag.TodayNotifications = 0;

            ViewBag.AccountNotifications = 0;
            ViewBag.ProfileNotifications = 0;
            ViewBag.MessageNotifications = 0;
            ViewBag.SecurityNotifications = 0;
            ViewBag.SystemNotifications = 0;
        }
    }
}