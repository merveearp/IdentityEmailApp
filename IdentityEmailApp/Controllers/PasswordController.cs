using IdentityEmailApp.Entities;
using IdentityEmailApp.Enums;
using IdentityEmailApp.Models.PasswordModels;
using IdentityEmailApp.Services.Abstract;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace IdentityEmailApp.Controllers
{
    public class PasswordController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ISystemEventService _systemEventService;

        public PasswordController(UserManager<AppUser> userManager, IConfiguration configuration, ISystemEventService systemEventService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _systemEventService = systemEventService;
        }

        [HttpGet]
        public async Task<IActionResult> ForgetPassword()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            // Kullanıcı bulunamazsa sistemde kayıtlı e-postaları açık etmiyoruz.
            if (user == null)
            {
                return RedirectToAction("PasswordResetLinkSent");
            }

            string passwordResetToken =
                await _userManager.GeneratePasswordResetTokenAsync(user);

            string passwordResetTokenLink = Url.Action(
                "ResetPassword",
                "Password",
                new
                {
                    userId = user.Id,
                    token = passwordResetToken
                },
                protocol: HttpContext.Request.Scheme
            )!;

            var mimeMessage = new MimeMessage();

            mimeMessage.From.Add(
                new MailboxAddress(
                    "Notika Mail",
                    "merveearp@gmail.com"
                )
            );

            mimeMessage.To.Add(
                new MailboxAddress(
                    $"{user.Name} {user.Surname}",
                    model.Email
                )
            );

            mimeMessage.Subject = "Notika Mail | Şifre Yenileme Talebi";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $"""
            <!DOCTYPE html>
            <html lang="tr">
            <head>
                <meta charset="UTF-8">
            </head>

            <body style="
                margin:0;
                padding:0;
                background-color:#f1f4f3;
                font-family:Arial, Helvetica, sans-serif;">

                <div style="
                    max-width:600px;
                    margin:40px auto;
                    background-color:#ffffff;
                    border-radius:12px;
                    overflow:hidden;
                    box-shadow:0 4px 20px rgba(0,0,0,0.08);">

                    <div style="
                        background-color:#00c292;
                        padding:28px;
                        text-align:center;">

                        <h1 style="
                            margin:0;
                            color:#004935;
                            font-size:28px;">
                            Notika Mail
                        </h1>

                    </div>

                    <div style="padding:32px;">

                        <h2 style="
                            margin-top:0;
                            color:#181c1c;
                            font-size:24px;">
                            Şifre Yenileme Talebi
                        </h2>

                        <p style="
                            color:#3c4a43;
                            font-size:16px;
                            line-height:1.6;">
                            Merhaba {user.Name},
                        </p>

                        <p style="
                            color:#3c4a43;
                            font-size:16px;
                            line-height:1.6;">
                            Notika Mail hesabınızın şifresini yenilemek için
                            bir talep aldık. Yeni şifrenizi belirlemek için
                            aşağıdaki butona tıklayabilirsiniz.
                        </p>

                        <div style="
                            text-align:center;
                            margin:32px 0;">

                            <a href="{passwordResetTokenLink}"
                               style="
                                   display:inline-block;
                                   padding:14px 28px;
                                   background-color:#00c292;
                                   color:#004935;
                                   text-decoration:none;
                                   font-size:16px;
                                   font-weight:bold;
                                   border-radius:8px;">
                                Şifremi Yenile
                            </a>

                        </div>

                        <p style="
                            color:#777777;
                            font-size:14px;
                            line-height:1.6;">
                            Buton çalışmıyorsa aşağıdaki bağlantıyı
                            tarayıcınıza kopyalayabilirsiniz:
                        </p>

                        <p style="
                            color:#006c50;
                            font-size:13px;
                            line-height:1.5;
                            word-break:break-all;">
                            {passwordResetTokenLink}
                        </p>

                        <p style="
                            color:#777777;
                            font-size:14px;
                            line-height:1.6;">
                            Bu işlemi siz talep etmediyseniz bu e-postayı
                            dikkate almayabilirsiniz. Mevcut şifreniz
                            değişmeden kalacaktır.
                        </p>

                    </div>

                    <div style="
                        padding:20px;
                        background-color:#f7faf9;
                        text-align:center;
                        color:#777777;
                        font-size:12px;">

                        © 2026 Notika Mail. Tüm hakları saklıdır.

                    </div>

                </div>

            </body>
            </html>
            """
            };

            mimeMessage.Body = bodyBuilder.ToMessageBody();

            var appPassword =
                _configuration["EmailSettings:AppPassword"];

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

            await _systemEventService.CreateAsync(user, NotificationType.PasswordResetRequested);

            return RedirectToAction("PasswordResetLinkSent");
        }

        [HttpGet]
        public IActionResult PasswordResetLinkSent()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) ||
                string.IsNullOrWhiteSpace(token))
            {
                return RedirectToAction("ForgetPassword");
            }

            var model = new ResetPasswordViewModel
            {
                UserId = userId,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(
            ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Kullanıcı bulunamadı veya bağlantı geçersizdir.");

                return View(model);
            }

            // Kullanıcı mevcut şifresiyle aynı şifreyi belirleyemez.
            var isCurrentPassword =
                await _userManager.CheckPasswordAsync(user, model.Password);

            if (isCurrentPassword)
            {
                ModelState.AddModelError(
                    nameof(model.Password),
                    "Yeni şifreniz mevcut şifrenizle aynı olamaz.");

                return View(model);
            }

            var result = await _userManager.ResetPasswordAsync(
                user,
                model.Token,
                model.Password
            );

            if (result.Succeeded)
            {
                await _userManager.ResetAccessFailedCountAsync(user);
                await _userManager.SetLockoutEndDateAsync(user, null);

                TempData["PasswordResetSuccess"] =
                    "Şifreniz başarıyla yenilendi. Yeni şifrenizle giriş yapabilirsiniz.";
                await _systemEventService.CreateAsync(user,NotificationType.PasswordChanged);

                return RedirectToAction("SignIn", "Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }


    }
}
