using IdentityEmailApp.Context;
using IdentityEmailApp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityEmailApp.Controllers
{
    public class TaskController : Controller
    {
        private readonly EmailContext _context;
        private readonly UserManager<AppUser> _userManager;

        public TaskController(EmailContext context, UserManager<AppUser> userManager)
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

            var tasks = await _context.UserTasks
                .Include(x => x.TaskList)
                .Include(x => x.SubTasks)
                .Where(x => x.AppUserId == user.Id)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
                
                
            return View(tasks);
        }
      
    }
}
