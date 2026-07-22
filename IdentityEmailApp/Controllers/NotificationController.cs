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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsUnRead(int id)
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

            notification.IsRead = false;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkSelectedAsRead(List<int> ids)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            if(ids==null || !ids.Any())
            {
                TempData["ErrorMessage"] = "Lütfen en az bir bildirim seçiniz.";
                return RedirectToAction("Index");
            }

            var notifications = await _context.Notifications.Where(x => ids.Contains(x.NotificationId) && x.AppUserId == user.Id).ToListAsync();

            if(!notifications.Any())
            {
                TempData["ErrorMessage"] = "Seçilen bildirimler bulunamadı.";
                return RedirectToAction("Index");
            }

            foreach(var notification in notifications)
            {
              
                notification.IsRead= true;
            }
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                $"{notifications.Count} bildirim okundu olarak işaretlendi.";

            return RedirectToAction("Index");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkSelectedAsUnRead(List<int> ids)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            if (ids == null || !ids.Any())
            {
                TempData["ErrorMessage"] = "Lütfen en az bir bildirim seçiniz.";
                return RedirectToAction("Index");
            }

            var notifications = await _context.Notifications.Where(x => ids.Contains(x.NotificationId) && x.AppUserId == user.Id).ToListAsync();

            if (!notifications.Any())
            {
                TempData["ErrorMessage"] = "Seçilen bildirimler bulunamadı.";
                return RedirectToAction("Index");
            }

            foreach (var notification in notifications)
            {

                notification.IsRead = false;
            }
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                $"{notifications.Count} bildirim okunmadı olarak işaretlendi.";

            return RedirectToAction("Index");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSelected(List<int> ids)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            if (ids == null || !ids.Any())
            {
                TempData["ErrorMessage"] = "Lütfen en az bir bildirim seçiniz.";
                return RedirectToAction("Index");
            }

            var notifications = await _context.Notifications
                .Where(x => ids.Contains(x.NotificationId) &&
                            x.AppUserId == user.Id)
                .ToListAsync();

            if (!notifications.Any())
            {
                TempData["ErrorMessage"] = "Seçilen bildirimler bulunamadı.";
                return RedirectToAction("Index");
            }

            _context.Notifications.RemoveRange(notifications);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                $"{notifications.Count} bildirim başarıyla silindi.";

            return RedirectToAction("Index");
        }
    } 
}