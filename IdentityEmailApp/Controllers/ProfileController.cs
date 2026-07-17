using IdentityEmailApp.Entities;
using IdentityEmailApp.Models.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace IdentityEmailApp.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly SignInManager<AppUser> _signInManager;

        public ProfileController(UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _signInManager = signInManager;
        }

        private int CalculateProfileCompletion(AppUser user)
        {
            int completedFields = 0;
            int totalFields = 5;

            if(!string.IsNullOrEmpty(user.PhoneNumber))
            {
                completedFields++;
            }
            if (!string.IsNullOrWhiteSpace(user.City))
            {
                completedFields++;
            }

            if (user.BirthDate.HasValue)
            {
                completedFields++;
            }

            if (user.Gender.HasValue)
            {
                completedFields++;
            }

            if (!string.IsNullOrWhiteSpace(user.ImageUrl) &&
                !user.ImageUrl.Contains("default"))
            {
                completedFields++;
            }

            return completedFields * 100 / totalFields;
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
            user.IsProfileCompleted=true;
            user.IsProfileSetupShown=true;

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


        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var model = new EditUserViewModel
            {
                Name = user.Name,
                Surname = user.Surname,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                City = user.City,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                ImageUrl = user.ImageUrl
            };
            ViewBag.ProfileCompletion = CalculateProfileCompletion(user);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {

            var user = await _userManager.GetUserAsync(User);

            if(user ==null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var model = new EditUserViewModel
            {
                Name = user.Name,
                Surname = user.Surname,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                City = user.City,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                ImageUrl = user.ImageUrl
            };
            ViewBag.ProfileCompletion =CalculateProfileCompletion(user);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditUserViewModel model)
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

           
            var existingUser = await _userManager.FindByNameAsync(model.UserName!);

            if (existingUser != null && existingUser.Id != user.Id)
            {
                ModelState.AddModelError(
                    nameof(model.UserName),
                    "Bu kullanıcı adı başka bir kullanıcı tarafından kullanılıyor.");

                model.Email = user.Email;
                model.ImageUrl = user.ImageUrl;

                return View(model);
            }

           
            if (model.ProfileImage != null)
            {
                var extension = Path
                    .GetExtension(model.ProfileImage.FileName)
                    .ToLowerInvariant();

                var allowedExtensions = new[]
                {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp",
            ".jfif"
        };

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(
                        nameof(model.ProfileImage),
                        "Yalnızca JPG, JPEG, PNG , JFIF veya WEBP dosyası yükleyebilirsiniz.");

                    model.Email = user.Email;
                    model.ImageUrl = user.ImageUrl;

                    ViewBag.ProfileCompletion =CalculateProfileCompletion(user);

                    return View(model);
                }

                if (model.ProfileImage.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError(
                        nameof(model.ProfileImage),
                        "Profil fotoğrafı en fazla 5 MB olabilir.");

                    model.Email = user.Email;
                    model.ImageUrl = user.ImageUrl;

                    return View(model);
                }

                var folderPath = Path.Combine(
                    _webHostEnvironment.WebRootPath,
                    "images",
                    "profile");

                Directory.CreateDirectory(folderPath);

                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(folderPath, fileName);

                await using var stream = new FileStream(
                    filePath,
                    FileMode.Create);

                await model.ProfileImage.CopyToAsync(stream);

                user.ImageUrl = $"/images/profile/{fileName}";
            }

            user.Name = model.Name;
            user.Surname = model.Surname;
            user.UserName = model.UserName;
            user.PhoneNumber = model.PhoneNumber;
            user.City = model.City;
            user.BirthDate = model.BirthDate;
            user.Gender = model.Gender;

          
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                model.Email = user.Email;
                model.ImageUrl = user.ImageUrl;

                ViewBag.ProfileCompletion = CalculateProfileCompletion(user);

                return View(model);
            }

            await _signInManager.RefreshSignInAsync(user);

            TempData["SuccessMessage"] =
                "Profil bilgileriniz başarıyla güncellendi.";

            return RedirectToAction(nameof(EditProfile));
        }
    }
}
