using IdentityEmailApp.Context;
using IdentityEmailApp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityEmailApp.ViewComponents.DashboardViewComponents
{
    public class _DashboardPieComponent : ViewComponent
    {
        private readonly EmailContext _emailContext;
        private readonly UserManager<AppUser> _userManager;

        public _DashboardPieComponent(
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
                ViewBag.DraftMessage = 0;
                ViewBag.SpamMessage = 0;
                ViewBag.StarMessage = 0;
                ViewBag.UnreadMessage = 0;
                ViewBag.DeletedMessage = 0;

                return View();
            }

            ViewBag.DraftMessage =
                await _emailContext.Messages.CountAsync(x =>
                    x.SenderEmail == user.Email &&
                    x.IsDraft &&
                    !x.IsDeleted);

            ViewBag.SpamMessage =
                await _emailContext.Messages.CountAsync(x =>
                    x.ReceiverEmail == user.Email &&
                    x.IsSpam &&
                    !x.IsDeleted);

            ViewBag.StarMessage =
                await _emailContext.Messages.CountAsync(x =>
                    x.ReceiverEmail == user.Email &&
                    x.IsStarred &&
                    !x.IsDeleted);

            ViewBag.UnreadMessage =
                await _emailContext.Messages.CountAsync(x =>
                    x.ReceiverEmail == user.Email &&
                    !x.IsRead &&
                    !x.IsDeleted &&
                    !x.IsSpam &&
                    !x.IsDraft);

            ViewBag.DeletedMessage =
                await _emailContext.Messages.CountAsync(x =>
                    x.ReceiverEmail == user.Email &&
                    x.IsDeleted);

            return View();
        }
    }
}