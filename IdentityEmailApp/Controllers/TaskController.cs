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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var tasks = await _context.UserTasks
                .AsNoTracking()
                .Include(x => x.TaskList)
                .Include(x => x.SubTasks)
                .Where(x =>
                    x.AppUserId == user.Id &&
                    !x.IsDeleted)
                .OrderByDescending(x => x.IsImportant)
                .ThenBy(x => x.DueDate == null)
                .ThenBy(x => x.DueDate)
                .ThenByDescending(x => x.CreatedDate)
                .ToListAsync();

            return View(tasks);
        }
        public async Task<IActionResult> CompletedIndex()
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
                 x.IsCompleted)
             .OrderByDescending(x => x.CompletedDate)
             .ToListAsync();


            return View(tasks);
        }

        public async Task<IActionResult> UnCompletedIndex()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var tasks = await _context.UserTasks
                .AsNoTracking()
                .Include(x => x.TaskList)
                .Include(x => x.SubTasks)
                .Where(x =>
                    x.AppUserId == user.Id &&
                    !x.IsDeleted &&
                    !x.IsCompleted)
                .OrderBy(x => x.DueDate == null)
                .ThenBy(x => x.DueDate)
                .ThenByDescending(x => x.CreatedDate)
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
        public async Task<IActionResult> CreateTask(int? taskListId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            if (taskListId.HasValue)
            {
                var categoryExists = await _context.TaskLists
                    .AnyAsync(x =>
                        x.TaskListId == taskListId.Value &&
                        x.AppUserId == user.Id);

                if (!categoryExists)
                {
                    return NotFound();
                }
            }

            await LoadTaskListAsync(user.Id);

            var model = new CreateTaskDto
            {
                TaskListId = taskListId
            };

            return View(model);
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

            if (userTask.SubTasks != null)
            {
                foreach (var subTask in userTask.SubTasks)
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

            if (userTask == null)
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

            if (userTask.SubTasks != null && userTask.SubTasks.Any())
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

        [HttpGet]
        public async Task<IActionResult> TaskListIndex(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }


            var taskList = await _context.TaskLists
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.TaskListId == id &&
                    x.AppUserId == user.Id);

            if (taskList == null)
            {
                return NotFound();
            }


            var tasks = await _context.UserTasks
                .AsNoTracking()
                .Include(x => x.TaskList)
                .Include(x => x.SubTasks)
                .Where(x =>
                    x.AppUserId == user.Id &&
                    x.TaskListId == id &&
                    !x.IsCompleted &&
                    !x.IsDeleted)
                .OrderByDescending(x => x.IsImportant)
                .ThenBy(x => x.DueDate == null)
                .ThenBy(x => x.DueDate)
                .ThenByDescending(x => x.CreatedDate)
                .ToListAsync();

            ViewBag.TaskListId = taskList.TaskListId;
            ViewBag.TaskListName = taskList.Name;

            return View(tasks);


        }

        //<--Kategori alanı-->
        [HttpGet]
        public async Task<IActionResult> TaskList()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }


            var taskList = await _context.TaskLists
                .AsNoTracking()
                .Where(x =>
                    x.AppUserId == user.Id)
                .OrderBy(x => x.Name)
                .ToListAsync();

            if (taskList == null)
            {
                return NotFound();
            }

            return View(taskList);


        }

        [HttpGet]
        public async Task<IActionResult> CreateTaskList()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            return View(new CreateTaskListDto());

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTaskList(CreateTaskListDto model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var categoryName = model.Name;
            var categoryExist = await _context.TaskLists
                .AnyAsync(x => x.AppUserId == user.Id &&
                x.Name == categoryName);

            if (categoryExist == true)
            {
                ModelState.AddModelError(nameof(model.Name), "Bu isimde bir kategoriniz zaten bulunuyor.");

                return View(model);
            }
            var taskList = new TaskList
            {
                Name = categoryName,
                CreatedDate = DateTime.Now,
                AppUserId = user.Id
            };

            _context.TaskLists.Add(taskList);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Kategori başarıyla oluşturuldu.";

            return RedirectToAction(nameof(TaskList));
        }


        [HttpGet]
        public async Task<IActionResult> EditTaskList(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var taskList = await _context.TaskLists
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                x.TaskListId == id &&
                x.AppUserId == user.Id);

            if (taskList == null)
            {
                return NotFound();
            }

            var model = new EditTaskListDto
            {
                TaskListId = taskList.TaskListId,
                Name = taskList.Name
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditTaskList(EditTaskListDto model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            var taskList = await _context.TaskLists.FirstOrDefaultAsync(x =>
           x.TaskListId == model.TaskListId &&
           x.AppUserId == user.Id);

            if (taskList == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var categoryName = model.Name.Trim();

            var categoryExist = await _context.TaskLists
                .AnyAsync(x => x.AppUserId == user.Id &&
                 x.Name == categoryName);

            if (categoryExist == true)
            {
                ModelState.AddModelError(nameof(model.Name),
            "Bu isimde başka bir kategoriniz zaten bulunuyor.");
                return View(model);
            }

            taskList.Name = model.Name;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Kategori başarıyla güncellendi.";

            return RedirectToAction(nameof(TaskList));

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTaskList(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var taskList = await _context.TaskLists
                .FirstOrDefaultAsync(x =>
                    x.TaskListId == id &&
                    x.AppUserId == user.Id);

            if (taskList == null)
            {
                return NotFound();
            }

            _context.TaskLists.Remove(taskList);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Kategori ve bağlı görevler kalıcı olarak silindi.";

            return RedirectToAction(nameof(TaskList));
        }

    }

}



