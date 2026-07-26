using IdentityEmailApp.Context;
using IdentityEmailApp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityEmailApp.ViewComponents.MessageViewComponents
{
    public class _MessageSidebarComponent : ViewComponent
    {
        private readonly EmailContext _context;
        private readonly UserManager<AppUser> _userManager;

        public _MessageSidebarComponent(
            EmailContext context,
            UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(
                HttpContext.User
            );

            if (user == null)
            {
                ViewBag.sendMessageCount = 0;
                return View();
            }

            ViewBag.sendMessageCount = await _context.Messages
                .CountAsync(x => x.SenderEmail == user.Email && !x.IsDraft);
            ViewBag.receivedMessageCount = await _context.Messages
                .CountAsync(x => x.ReceiverEmail == user.Email && !x.IsSpam && !x.IsDeleted );
            ViewBag.notificationCount = await _context.Notifications.Where(x => x.AppUserId==user.Id).CountAsync();
            ViewBag.starredMessageCount = await _context.Messages.Where(x => x.IsStarred && x.SenderEmail == user.Email || x.ReceiverEmail == user.Email ).CountAsync();
            ViewBag.deletedMessageCount = await _context.Messages.Where(x => x.IsStarred && x.SenderEmail == user.Email || x.ReceiverEmail == user.Email ).CountAsync();
            ViewBag.spamMessageCount = await _context.Messages.Where(x => x.IsSpam &&  x.ReceiverEmail == user.Email ).CountAsync();


            return View();
        }
    }
}