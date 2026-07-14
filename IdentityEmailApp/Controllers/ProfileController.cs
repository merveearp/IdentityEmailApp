using IdentityEmailApp.Entities;
using IdentityEmailApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace IdentityEmailApp.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfileController(UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Skip()
        {

            return RedirectToAction("Inbox", "Message");

        }

        [HttpGet]
        public async Task<IActionResult> CompleteProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            if(user== null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            if(!user.IsProfileSetupShown)
            {
                user.IsProfileSetupShown = true;
                await _userManager.UpdateAsync(user);
            }

            var model = new CompleteProfileViewModel
            {
                PhoneNumber = user.PhoneNumber,
                City = user.City,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                ImageUrl = user.ImageUrl,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteProfile(CompleteProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            if(model.ProfileImage != null)
            {
                var extension =Path.GetExtension(model.ProfileImage.FileName).ToLowerInvariant();
                var allowedExtensions = new[]
                {
                    ".jpg",
                    ".jpeg",
                    ".jfif",
                    ".webp",
                    ".png"
                };

                if(!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(nameof(model.ProfileImage),
               "Yalnızca JPG, JPEG, PNG veya WEBP dosyası yükleyebilirsiniz.");

                    model.ImageUrl = user.ImageUrl;
                    return View(model);
                }

                if (model.ProfileImage.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError(
                        nameof(model.ProfileImage),
                        "Profil fotoğrafı en fazla 5 MB olabilir.");

                    model.ImageUrl = user.ImageUrl;
                    return View(model);
                }

                var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "profile");
                Directory.CreateDirectory(folderPath);

                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(folderPath, fileName);

                await using var stream = new FileStream(filePath, FileMode.Create);
                await model.ProfileImage.CopyToAsync(stream);
                user.ImageUrl = $"images/profile/{fileName}";

            }

            user.PhoneNumber = model.PhoneNumber;
            user.City = model.City;
            user.BirthDate = model.BirthDate;
            user.Gender = model.Gender;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] =
                    "Profil bilgileriniz başarıyla güncellendi.";

                return RedirectToAction("Inbox", "Message");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            model.ImageUrl = user.ImageUrl;

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var values = await _userManager.FindByEmailAsync(User.Identity.Name);
            EditUserViewModel model = new EditUserViewModel();
            model.Name = values.Name;
            model.Surname = values.Surname;
            model.UserName = values.UserName;
            model.PhoneNumber = values.PhoneNumber;
            model.City = values.City;

            return View(model);
        }

    }
}
