using IdentityEmailApp.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityEmailApp.ViewComponents.TaskViewComponents
{
    public class _TaskListComponent:ViewComponent
    {
        private readonly EmailContext _context;

        public _TaskListComponent(EmailContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var taskList = await _context.TaskLists
                .Include(x => x.UserTasks)
                .Include(x => x.AppUser)
                .Include(x => x.UserTasks).ToListAsync();

            return View(taskList);
        }
    }
}
