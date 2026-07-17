using IdentityEmailApp.Entities;
using IdentityEmailApp.Models.UserModels;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Security.Cryptography;


namespace IdentityEmailApp.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public RegisterController( UserManager<AppUser> userManager,IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 100000 ile 999999 arasında güvenli, 6 haneli kod üretir.
            int code = RandomNumberGenerator.GetInt32(100000, 1000000);

            AppUser appUser = new AppUser()
            {
                Name = model.Name,
                Surname = model.Surname,
                UserName = model.Username,
                Email = model.Email,
                ImageUrl = "/images/default.jfif",
                ActivationCode = code,
                IsProfileCompleted = false,
                IsProfileSetupShown = false
            };

            var result = await _userManager.CreateAsync(appUser, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            var mimeMessage = new MimeMessage();

            mimeMessage.From.Add(
                new MailboxAddress("Notika Email App", "merveearp@gmail.com")
            );

            mimeMessage.To.Add(
                new MailboxAddress($"{model.Name} {model.Surname}", model.Email)
            );

            mimeMessage.Subject = "Notika Identity Aktivasyon Kodu";

            var bodyBuilder = new BodyBuilder
            {
                TextBody =
                    $"Merhaba {model.Name},\n\n" +
                    $"Hesabınızı doğrulamak için aktivasyon kodunuz: {code}\n\n" +
                    "Bu kodu kimseyle paylaşmayınız."
            };

            mimeMessage.Body = bodyBuilder.ToMessageBody();

            try
            {
                var appPassword =
                    _configuration["EmailSettings:AppPassword"];

                if (string.IsNullOrWhiteSpace(appPassword))
                {
                    await _userManager.DeleteAsync(appUser);

                    ModelState.AddModelError(
                        string.Empty,
                        "Gmail uygulama şifresi yapılandırmada bulunamadı."
                    );

                    return View(model);
                }

                using var client = new SmtpClient();

                await client.ConnectAsync(
                    "smtp.gmail.com",
                    587,
                    MailKit.Security.SecureSocketOptions.StartTls
                );

                await client.AuthenticateAsync(
                    "merveearp@gmail.com",
                    appPassword
                );

                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);

                TempData["EmailMove"] = model.Email;
            }
            catch (Exception ex)
            {
                await _userManager.DeleteAsync(appUser);

                ModelState.AddModelError(
                    string.Empty,
                    $"Aktivasyon e-postası gönderilemedi: {ex.Message}"
                );

                return View(model);
            }
            return RedirectToAction( "UserActivation", "Activation"  );
        }
    }
}
