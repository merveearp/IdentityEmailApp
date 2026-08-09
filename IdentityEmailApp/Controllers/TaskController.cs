using IdentityEmailApp.Context;
using IdentityEmailApp.DTOs.TaskDtos;
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
             .Where(x =>
                 x.AppUserId == user.Id &&
                 !x.IsDeleted)
             .OrderByDescending(x => x.CreatedDate)
             .ToListAsync();


            return View(tasks);
        }
        public async Task<IActionResult> StarredIndex()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var tasks = await _context.UserTasks
             .Include(x => x.TaskList)
             .Include(x => x.SubTasks)
             .Where(x =>
                 x.AppUserId == user.Id &&
                 !x.IsDeleted &&
                 x.IsImportant)
             .OrderByDescending(x => x.CreatedDate)
             .ToListAsync();


            return View(tasks);
        }

        [HttpGet]
        public async Task<IActionResult> TaskDetail(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var userTask = await _context.UserTasks
                .AsNoTracking()
                .Include(x => x.TaskList)
                .Include(x => x.SubTasks)
                .FirstOrDefaultAsync(x =>
                    x.UserTaskId == id &&
                    x.AppUserId == user.Id &&
                    !x.IsDeleted);

            if (userTask == null)
            {
                return NotFound();
            }

            return View(userTask);
        }

        [HttpGet]
        public async Task<IActionResult> EditTask(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var userTask = await _context.UserTasks
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                x.UserTaskId == id &&
                x.AppUserId == user.Id &&
                !x.IsDeleted);

            if (userTask == null)
            {
                return NotFound();
            }
            await LoadTaskListAsync(user.Id);

            var model = new EditTaskDto
            {
                UserTaskId = userTask.UserTaskId,
                Title = userTask.Title,
                Description = userTask.Description,
                DueDate = userTask.DueDate,
                ReminderDate = userTask.ReminderDate,
                IsImportant = userTask.IsImportant,
                TaskListId = userTask.TaskListId
            };


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditTask(EditTaskDto model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var userTask = await _context.UserTasks
              .FirstOrDefaultAsync(x =>
                  x.UserTaskId == model.UserTaskId &&
                  x.AppUserId == user.Id &&
                  !x.IsDeleted);

            if (userTask == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await LoadTaskListAsync(user.Id);
                return View(model);
            }

            userTask.Title = model.Title.Trim();
            userTask.Description = model.Description?.Trim();
            userTask.DueDate = model.DueDate;
            userTask.ReminderDate = model.ReminderDate;
            userTask.IsImportant = model.IsImportant;
            userTask.TaskListId = model.TaskListId;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Görev başarıyla güncellendi.";

            return RedirectToAction(
                nameof(TaskDetail),
                new { id = userTask.UserTaskId }
            );
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleSubTaskStatus(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var subTask = await _context.SubTasks
                .Include(x => x.UserTask)
                .FirstOrDefaultAsync(x =>
                    x.SubTaskId == id &&
                    x.UserTask.AppUserId == user.Id &&
                    !x.UserTask.IsDeleted);

            if (subTask == null)
            {
                return NotFound();
            }

            subTask.IsCompleted = !subTask.IsCompleted;

            subTask.CompletedDate = subTask.IsCompleted
                ? DateTime.Now
                : null;

            await _context.SaveChangesAsync();

            return RedirectToAction(
                nameof(TaskDetail),
                new { id = subTask.UserTaskId }
            );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleTaskStatus(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var userTask = await _context.UserTasks
                .FirstOrDefaultAsync(x =>
                    x.UserTaskId == id &&
                    x.AppUserId == user.Id &&
                    !x.IsDeleted);

            if (userTask == null)
            {
                return NotFound();
            }

            userTask.IsCompleted = !userTask.IsCompleted;

            userTask.CompletedDate = userTask.IsCompleted
                ? DateTime.Now
                : null;

            await _context.SaveChangesAsync();

            return RedirectToAction(
                nameof(TaskDetail),
                new { id = userTask.UserTaskId }
            );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSubTask(CreateSubTaskDto model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var userTask = await _context.UserTasks
                .FirstOrDefaultAsync(x =>
                    x.UserTaskId == model.UserTaskId &&
                    x.AppUserId == user.Id &&
                    !x.IsDeleted);

            if (userTask == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Lütfen ayrıntı başlığını yazınız.";

                return RedirectToAction(
                    nameof(TaskDetail),
                    new { id = model.UserTaskId }
                );
            }

            var subTask = new SubTask
            {
                Title = model.Title.Trim(),
                CreatedDate = DateTime.Now,
                IsCompleted = false,
                CompletedDate = null,
                UserTaskId = userTask.UserTaskId
            };

            _context.SubTasks.Add(subTask);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Ayrıntı başarıyla eklendi.";

            return RedirectToAction(
                nameof(TaskDetail),
                new { id = userTask.UserTaskId }
            );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var userTask = await _context.UserTasks
                .FirstOrDefaultAsync(x =>
                    x.UserTaskId == id &&
                    x.AppUserId == user.Id &&
                    !x.IsDeleted);

            if (userTask == null)
            {
                return NotFound();
            }

            userTask.IsDeleted = true;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Görev silindi.";

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> DeletedIndex()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var tasks = await _context.UserTasks
             .Include(x => x.TaskList)
             .Include(x => x.SubTasks)
             .Where(x =>
                 x.AppUserId == user.Id &&
                 x.IsDeleted)
             .OrderByDescending(x => x.CreatedDate)
             .ToListAsync();


            return View(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> RestoreTask(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var userTask = await _context.UserTasks
                .FirstOrDefaultAsync(x =>
                x.UserTaskId == id &&
                x.AppUserId == user.Id &&
                x.IsDeleted);

            if(userTask==null)
            {
                return NotFound("Silinecek Görev bulunamadı!");
            }

            userTask.IsDeleted = false;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Görev başarıyla geri yüklendi.";

            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> PermanentDeleteTask(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var userTask = await _context.UserTasks
                .Include(x => x.SubTasks)
                .FirstOrDefaultAsync(x =>
                    x.UserTaskId == id &&
                    x.AppUserId == user.Id &&
                    x.IsDeleted);

            if (userTask == null)
            {
                return NotFound();
            }

            if(userTask.SubTasks != null && userTask.SubTasks.Any())
            {
                _context.SubTasks.RemoveRange(userTask.SubTasks);
            }

            _context.UserTasks.Remove(userTask);
            TempData["SuccessMessage"] = "Görev kalıcı olarak silindi.";

            return RedirectToAction(nameof(DeletedIndex));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSubTask(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var subTask = await _context.SubTasks
                .Include(x => x.UserTask)
                .FirstOrDefaultAsync(x =>
                    x.SubTaskId == id &&
                    x.UserTask.AppUserId == user.Id &&
                    !x.UserTask.IsDeleted);

            if (subTask == null)
            {
                return NotFound();
            }

            var userTaskId = subTask.UserTaskId;

            _context.SubTasks.Remove(subTask);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Ayrıntı başarıyla silindi.";

            return RedirectToAction(
                nameof(TaskDetail),
                new { id = userTaskId }
            );
        }

    }

     
}
