using IdentityEmailApp.Context;
using IdentityEmailApp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityEmailApp.ViewComponents.DashboardViewComponents
{
    public class _DashboardUserTaskComponent :ViewComponent
    {
        private readonly EmailContext _emailContext;
        private readonly UserManager<AppUser> _userManager;

        public _DashboardUserTaskComponent(EmailContext emailContext, UserManager<AppUser> userManager)
        {
            _emailContext = emailContext;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if(user == null)
            {
                return View("SignIn", "Login");
            }

            var task = await _emailContext.UserTasks
                .Where(x => x.AppUserId == user.Id && x.IsDeleted ==false)
                .Include(x => x.TaskList)
                .Include(x => x.SubTasks)
                .OrderByDescending(x => x.CreatedDate)
                .FirstOrDefaultAsync();
            return View(task);
        }
    }
}
