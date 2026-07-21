using IdentityEmailApp.Entities;
using IdentityEmailApp.Models.RoleModels;
using IdentityEmailApp.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityEmailApp.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISystemEventService _systemEventService;

        public RoleController(ISystemEventService systemEventService)
        {
            _systemEventService = systemEventService;
        }

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }


        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            IdentityRole role = new IdentityRole()
            {
                Name = model.RoleName
            };

            await _roleManager.CreateAsync(role);
            return RedirectToAction("RoleList");

        }
        public async Task<IActionResult> RoleList()
        {
            var values = await _roleManager.Roles.ToListAsync();
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateRole(string id)
        {
            var value = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == id);
            UpdateRoleViewModel model = new UpdateRoleViewModel()
            {
                RoleId = value.Id,
                RoleName = value.Name
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole(UpdateRoleViewModel model)
        {
            var value = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == model.RoleId);
            value.Name = model.RoleName;
            await _roleManager.UpdateAsync(value);
            return RedirectToAction("RoleList");

        }

        public async Task<IActionResult> DeleteRole(string id)
        {
            var value = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == id);
            await _roleManager.DeleteAsync(value);
            return RedirectToAction("RoleList");
        }

        public async Task<IActionResult> UserList()
        {
            var values = await _userManager.Users.ToListAsync();
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> AssignRole(string id)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            TempData["userId"] = user.Id;

            ViewBag.UserFullName = $"{user.Name} {user.Surname}";
            ViewBag.UserName = user.UserName;
            ViewBag.UserEmail = user.Email;
            ViewBag.UserImageUrl = user.ImageUrl;

            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new List<RoleAssignViewModel>();

            foreach (var item in roles)
            {
                var roleAssign = new RoleAssignViewModel
                {
                    RoleId = item.Id,
                    RoleName = item.Name,
                    RoleExist = userRoles.Contains(item.Name)
                };

                model.Add(roleAssign);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(List<RoleAssignViewModel> model)
        {

            var userId = TempData["userId"].ToString();
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            foreach(var item in model)
            {
                if(item.RoleExist)
                {
                    await _userManager.AddToRoleAsync(user, item.RoleName);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, item.RoleName);
                }

            }

            await _systemEventService.CreateAsync(user, Enums.NotificationType.RoleAssigned);
            return RedirectToAction("UserList");
        }
    }
}
