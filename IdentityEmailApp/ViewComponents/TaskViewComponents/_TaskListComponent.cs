using IdentityEmailApp.Context;
using IdentityEmailApp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityEmailApp.ViewComponents.TaskViewComponents
{
    public class _TaskListComponent:ViewComponent
    {
        private readonly EmailContext _context;
        private readonly UserManager<AppUser> _userManager;


        public _TaskListComponent(EmailContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var taskLists = await _context.TaskLists
                .AsNoTracking()
                .Where(X => X.AppUserId == user.Id)
                .OrderBy(x => x.Name)
                .ToListAsync();

            return View(taskLists);
        }
    }
}
