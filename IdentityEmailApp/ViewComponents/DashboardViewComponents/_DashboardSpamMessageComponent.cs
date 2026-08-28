using IdentityEmailApp.Context;
using IdentityEmailApp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityEmailApp.ViewComponents.DashboardViewComponents
{
    public class _DashboardSpamMessageComponent: ViewComponent
    {
        private readonly EmailContext _emailContext;
        private readonly UserManager<AppUser> _userManager;

        public _DashboardSpamMessageComponent(EmailContext emailContext, UserManager<AppUser> userManager)
        {
            _emailContext = emailContext;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return View(new List<Message>());
            }

            var lastMessages = await _emailContext.Messages
                .AsNoTracking()
                .Where(x =>
                    x.ReceiverEmail == user.Email &&
                    !x.IsDeleted &&
                    x.IsSpam &&
                    !x.IsDraft)
                .OrderByDescending(x => x.SendDate)
                .Take(5)
                .ToListAsync();

            return View(lastMessages);
        }    
    }
}
