using IdentityEmailApp.Context;
using IdentityEmailApp.Entities;
using IdentityEmailApp.Enums;
using IdentityEmailApp.Models.UserModels;
using IdentityEmailApp.Services.Abstract;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Security.Cryptography;

namespace IdentityEmailApp.Controllers
{
    public class ActivationController : Controller
    {
        private readonly EmailContext _emailContext;
        private readonly ISystemEventService _systemEventService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public ActivationController(
            EmailContext emailContext,
            ISystemEventService systemEventService,
            UserManager<AppUser> userManager,
            IConfiguration configuration)
        {
            _emailContext = emailContext;
            _systemEventService = systemEventService;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult UserActivation()
        {
            var email = TempData["EmailMove"]?.ToString();

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Signup", "Register");
            }

            TempData.Keep("EmailMove");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserActivation(UserActivationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData.Keep("EmailMove");
                return View(model);
            }

            var email = TempData["EmailMove"]?.ToString();

            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Aktivasyon işlemi için kullanıcı bilgisi bulunamadı."
                );

                return View(model);
            }

            var user = await _emailContext.Users
                .FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Kullanıcı bulunamadı."
                );

                return View(model);
            }

            if (model.ActivationCode != user.ActivationCode)
            {
                ModelState.AddModelError(
                    nameof(model.ActivationCode),
                    "Girdiğiniz aktivasyon kodu hatalı."
                );

                TempData.Keep("EmailMove");

                return View(model);
            }

            user.EmailConfirmed = true;
            user.ActivationCode = 0;

            await _emailContext.SaveChangesAsync();

            var isMember = await _userManager.IsInRoleAsync(user, "Member");

            if (!isMember)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "Member");

                if (!roleResult.Succeeded)
                {
                    foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    TempData.Keep("EmailMove");
                    return View(model);
                }
            }


            TempData["SuccessMessage"] =
                "Hesabınız başarıyla doğrulandı. Giriş yapabilirsiniz.";

            await _systemEventService.CreateAsync(user, NotificationType.EmailVerified);


            return RedirectToAction("SignIn", "Login");
        }


        private async Task SendActivationEmailAsync( AppUser user, int activationCode)
        {
            var appPassword = _configuration["EmailSettings:AppPassword"];

            if (string.IsNullOrWhiteSpace(appPassword))
            {
                throw new InvalidOperationException(
                    "Gmail uygulama şifresi bulunamadı."
                );
            }

            var mimeMessage = new MimeMessage();

            mimeMessage.From.Add(
                new MailboxAddress(
                    "Notika Email App",
                    "merveearp@gmail.com"
                )
            );

            mimeMessage.To.Add(
                new MailboxAddress(
                    $"{user.Name} {user.Surname}",
                    user.Email
                )
            );

            // Gmail'in aynı konuşmada gruplamaması için saat ekledik.
            mimeMessage.Subject =
                $"Notika Yeni Aktivasyon Kodu - {DateTime.Now:HH:mm:ss}";
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
<!DOCTYPE html>
<html lang='tr'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Notika Yeni Aktivasyon Kodu</title>
</head>

<body style='margin:0;
             padding:0;
             background-color:#f4f7f6;
             font-family:Arial, Helvetica, sans-serif;'>

    <table role='presentation'
           width='100%'
           cellspacing='0'
           cellpadding='0'
           border='0'
           style='background-color:#f4f7f6;
                  padding:40px 15px;'>

        <tr>
            <td align='center'>

                <table role='presentation'
                       width='100%'
                       cellspacing='0'
                       cellpadding='0'
                       border='0'
                       style='max-width:600px;
                              background-color:#ffffff;
                              border-radius:14px;
                              overflow:hidden;
                              box-shadow:0 8px 30px rgba(0,0,0,0.08);'>

                    <!-- Üst başlık -->
                    <tr>
                        <td style='background-color:#00c292;
                                   padding:32px 25px;
                                   text-align:center;'>

                            <div style='font-size:34px;
                                        margin-bottom:10px;'>
                                ✉️
                            </div>

                            <h1 style='margin:0;
                                       color:#ffffff;
                                       font-size:26px;
                                       font-weight:700;'>
                                Notika Email App
                            </h1>

                            <p style='margin:8px 0 0;
                                      color:#eafff8;
                                      font-size:14px;'>
                                Yeni Hesap Doğrulama Kodu
                            </p>

                        </td>
                    </tr>

                    <!-- İçerik -->
                    <tr>
                        <td style='padding:40px 35px;'>

                            <h2 style='margin:0 0 18px;
                                       color:#333333;
                                       font-size:22px;'>
                                Merhaba {user.Name},
                            </h2>

                            <p style='margin:0 0 22px;
                                      color:#666666;
                                      font-size:15px;
                                      line-height:1.7;'>
                                Talebiniz üzerine Notika Email App hesabınız
                                için yeni bir aktivasyon kodu oluşturuldu.
                                Hesabınızı aktif hale getirmek için aşağıdaki
                                kodu aktivasyon ekranına giriniz.
                            </p>

                            <!-- Kod alanı -->
                            <div style='background-color:#f1fffa;
                                        border:1px solid #b8f1df;
                                        border-radius:12px;
                                        padding:28px 20px;
                                        text-align:center;
                                        margin:28px 0;'>

                                <p style='margin:0 0 10px;
                                          color:#55706a;
                                          font-size:13px;
                                          text-transform:uppercase;
                                          letter-spacing:1.5px;
                                          font-weight:700;'>
                                    Yeni Aktivasyon Kodunuz
                                </p>

                                <div style='font-size:36px;
                                            font-weight:800;
                                            letter-spacing:10px;
                                            color:#00a67d;'>
                                    {activationCode}
                                </div>

                            </div>

                            <p style='margin:0 0 18px;
                                      color:#666666;
                                      font-size:14px;
                                      line-height:1.6;'>
                                Önceki aktivasyon kodunuz artık geçerli değildir.
                                Güvenliğiniz için bu kodu hiç kimseyle paylaşmayınız.
                            </p>

                            <!-- Uyarı alanı -->
                            <div style='background-color:#fff8e7;
                                        border-left:4px solid #f5b942;
                                        padding:14px 16px;
                                        border-radius:6px;
                                        margin-top:24px;'>

                                <p style='margin:0;
                                          color:#765b16;
                                          font-size:13px;
                                          line-height:1.5;'>
                                    Bu işlemi siz başlatmadıysanız
                                    e-postayı dikkate almayabilirsiniz.
                                </p>

                            </div>

                        </td>
                    </tr>

                    <!-- Alt alan -->
                    <tr>
                        <td style='background-color:#f8faf9;
                                   padding:22px 30px;
                                   text-align:center;
                                   border-top:1px solid #eeeeee;'>

                            <p style='margin:0 0 6px;
                                      color:#888888;
                                      font-size:12px;'>
                                Bu e-posta otomatik olarak gönderilmiştir.
                            </p>

                            <p style='margin:0;
                                      color:#00a67d;
                                      font-size:12px;
                                      font-weight:700;'>
                                © {DateTime.Now.Year} Notika Email App
                            </p>

                        </td>
                    </tr>

                </table>

            </td>
        </tr>

    </table>

</body>
</html>",

                TextBody =
                    $"Merhaba {user.Name},\n\n" +
                    $"Yeni aktivasyon kodunuz: {activationCode}\n\n" +
                    "Önceki aktivasyon kodunuz artık geçerli değildir.\n" +
                    "Bu kodu hiç kimseyle paylaşmayınız."
            };

            mimeMessage.Body = bodyBuilder.ToMessageBody();

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
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendActivationCode()
        {
            var email = TempData["EmailMove"]?.ToString();

            if (string.IsNullOrWhiteSpace(email))
            {
                TempData["ErrorMessage"] =
                    "Aktivasyon bilgisi bulunamadı. Lütfen tekrar kayıt olun.";

                return RedirectToAction("Signup", "Register");
            }

            
            TempData.Keep("EmailMove");

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
                return RedirectToAction(nameof(UserActivation));
            }

            if (user.EmailConfirmed)
            {
                TempData.Remove("EmailMove");

                TempData["SuccessMessage"] =
                    "E-posta adresiniz zaten doğrulanmış. Giriş yapabilirsiniz.";

                return RedirectToAction("SignIn", "Login");
            }

            int newActivationCode =
                RandomNumberGenerator.GetInt32(100000, 1000000);

            user.ActivationCode = newActivationCode;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                TempData["ErrorMessage"] =
                    "Yeni aktivasyon kodu oluşturulamadı.";

                TempData.Keep("EmailMove");

                return RedirectToAction(nameof(UserActivation));
            }

            try
            {
                await SendActivationEmailAsync(user, newActivationCode);

               
                TempData["EmailMove"] = email;

                TempData["SuccessMessage"] =
                    "Yeni aktivasyon kodu e-posta adresinize gönderildi.";
            }
            catch (Exception)
            {
                TempData["EmailMove"] = email;

                TempData["ErrorMessage"] =
                    "Aktivasyon e-postası gönderilemedi. Lütfen tekrar deneyiniz.";
            }

            return RedirectToAction(nameof(UserActivation));
        }

    }
}
