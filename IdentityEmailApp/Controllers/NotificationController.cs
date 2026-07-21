using IdentityEmailApp.Context;
using IdentityEmailApp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityEmailApp.Controllers
{
    public class NotificationController : Controller
    {
        private readonly EmailContext _context;
        private readonly UserManager<AppUser> _userManager;

        public NotificationController(
            EmailContext context,
            UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var notifications = await _context.Notifications
                .AsNoTracking()
                .Where(x => x.AppUserId == user.Id)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            return View(notifications);
        }

        public async Task<IActionResult> NotificationDetail(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var notification = await _context.Notifications
                .FirstOrDefaultAsync(x =>
                    x.NotificationId == id &&
                    x.AppUserId == user.Id);

            if (notification == null)
            {
                return NotFound();
            }

            if (!notification.IsRead)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }

            return View(notification);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var notification = await _context.Notifications
                .FirstOrDefaultAsync(x =>
                    x.NotificationId == id &&
                    x.AppUserId == user.Id);

            if (notification == null)
            {
                return NotFound();
            }

            _context.Notifications.Remove(notification);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Bildirim başarıyla silindi.";

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var notification = await _context.Notifications
                .FirstOrDefaultAsync(x =>
                    x.NotificationId == id &&
                    x.AppUserId == user.Id);

            if (notification == null)
            {
                return NotFound();
            }

            notification.IsRead = true;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}