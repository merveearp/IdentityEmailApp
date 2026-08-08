using IdentityEmailApp.Context;
using IdentityEmailApp.DTOs.TaskDtos;
using IdentityEmailApp.DTOs.TaskDtos.IdentityEmailApp.DTOs.TaskDtos;
using IdentityEmailApp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        private async Task LoadTaskListAsync(string userId)
        {
            ViewBag.TaskLists = await _context.TaskLists
                .Where(x => x.AppUser.Id == userId)
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.TaskListId.ToString()
                }).ToListAsync();

        }

        public IActionResult Layout()
        {
            return View();
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

        public async Task<IActionResult> TaskDetail(int taskId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var task = await _context.UserTasks
               .Include(x => x.TaskList)
               .Include(x => x.SubTasks)
               .FirstOrDefaultAsync(
                x => x.UserTaskId == taskId && x.AppUserId == user.Id
                );

            if (task == null)
                return NotFound();

            return View(task);

        }

        [HttpGet]
        public async Task<IActionResult> CreateTask()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            await LoadTaskListAsync(user.Id);

            return View(new CreateTaskDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTask(CreateTaskDto userTask)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            if (!ModelState.IsValid)
            {
                await LoadTaskListAsync(user.Id);
                return View(userTask);
            }

            var newTask = new UserTask
            {
                Title = userTask.Title,
                Description = userTask.Description,

                CreatedDate = DateTime.Now,
                DueDate = userTask.DueDate,
                ReminderDate = userTask.ReminderDate,
                CompletedDate = null,

                IsCompleted = false,
                IsImportant = userTask.IsImportant,
                IsDeleted = false,

                AppUserId = user.Id,
                TaskListId = userTask.TaskListId
            };

            if(userTask.SubTasks !=null)
            {
                foreach(var subTask in userTask.SubTasks)
                {
                    if (string.IsNullOrWhiteSpace(subTask.Title))
                    {
                        continue;
                    }

                    newTask.SubTasks.Add(new SubTask
                    {
                        Title = subTask.Title,
                        IsCompleted = false,
                        CreatedDate = DateTime.Now,


                    });
                }
            }

            _context.UserTasks.Add(newTask);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Görev başarıyla oluşturuldu.";

            return RedirectToAction(nameof(Index));
        }
    }

     
}
