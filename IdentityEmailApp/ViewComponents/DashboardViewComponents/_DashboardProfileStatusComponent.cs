using IdentityEmailApp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.ViewComponents.DashboardViewComponents
{
    public class _DashboardProfileStatusComponent : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;

        public _DashboardProfileStatusComponent(
            UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager
                .GetUserAsync(HttpContext.User);

            if (user == null)
            {
                SetEmptyValues();
                return View();
            }

            var completedFieldCount = 0;
            const int totalFieldCount = 5;

            var hasName =
                !string.IsNullOrWhiteSpace(user.Name);

            var hasSurname =
                !string.IsNullOrWhiteSpace(user.Surname);

            var hasCity =
                !string.IsNullOrWhiteSpace(user.City);

            var hasPhoneNumber =
                !string.IsNullOrWhiteSpace(user.PhoneNumber);

            var hasProfilePhoto =
                !string.IsNullOrWhiteSpace(user.ImageUrl);

            if (hasName)
                completedFieldCount++;

            if (hasSurname)
                completedFieldCount++;

            if (hasCity)
                completedFieldCount++;

            if (hasPhoneNumber)
                completedFieldCount++;

            if (hasProfilePhoto)
                completedFieldCount++;

            var profilePercentage =
                completedFieldCount * 100 / totalFieldCount;

            ViewBag.FullName =
                $"{user.Name} {user.Surname}".Trim();

            ViewBag.Email = user.Email;
            ViewBag.City = user.City;
            ViewBag.PhoneNumber = user.PhoneNumber;
            ViewBag.ImageUrl = user.ImageUrl;

            ViewBag.IsEmailConfirmed = user.EmailConfirmed;

            ViewBag.HasName = hasName;
            ViewBag.HasSurname = hasSurname;
            ViewBag.HasCity = hasCity;
            ViewBag.HasPhoneNumber = hasPhoneNumber;
            ViewBag.HasProfilePhoto = hasProfilePhoto;

            ViewBag.CompletedFieldCount = completedFieldCount;
            ViewBag.TotalFieldCount = totalFieldCount;
            ViewBag.ProfilePercentage = profilePercentage;

            return View();
        }

        private void SetEmptyValues()
        {
            ViewBag.FullName = "Kullanıcı";
            ViewBag.Email = null;
            ViewBag.City = null;
            ViewBag.PhoneNumber = null;
            ViewBag.ImageUrl = null;

            ViewBag.IsEmailConfirmed = false;

            ViewBag.HasName = false;
            ViewBag.HasSurname = false;
            ViewBag.HasCity = false;
            ViewBag.HasPhoneNumber = false;
            ViewBag.HasProfilePhoto = false;

            ViewBag.CompletedFieldCount = 0;
            ViewBag.TotalFieldCount = 5;
            ViewBag.ProfilePercentage = 0;
        }
    }
}