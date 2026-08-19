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
                IsProfileSetupShown = false,
                
                
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
                HtmlBody = $@"
<!DOCTYPE html>
<html lang='tr'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Notika Aktivasyon Kodu</title>
</head>

<body style='margin:0; padding:0; background-color:#f4f7f6; font-family:Arial, Helvetica, sans-serif;'>

    <table role='presentation'
           width='100%'
           cellspacing='0'
           cellpadding='0'
           border='0'
           style='background-color:#f4f7f6; padding:40px 15px;'>

        <tr>
            <td align='center'>

                <table role='presentation'
                       width='100%'
                       cellspacing='0'
                       cellpadding='0'
                       border='0'
                       style='max-width:600px; background-color:#ffffff; border-radius:14px; overflow:hidden; box-shadow:0 8px 30px rgba(0,0,0,0.08);'>

                    <tr>
                        <td style='background-color:#00c292; padding:32px 25px; text-align:center;'>

                            <div style='font-size:34px; margin-bottom:10px;'>
                                ✉️
                            </div>

                            <h1 style='margin:0; color:#ffffff; font-size:26px; font-weight:700;'>
                                Notika Email App
                            </h1>

                            <p style='margin:8px 0 0; color:#eafff8; font-size:14px;'>
                                Hesap Doğrulama İşlemi
                            </p>

                        </td>
                    </tr>

                    <tr>
                        <td style='padding:40px 35px;'>

                            <h2 style='margin:0 0 18px; color:#333333; font-size:22px;'>
                                Merhaba {model.Name},
                            </h2>

                            <p style='margin:0 0 22px; color:#666666; font-size:15px; line-height:1.7;'>
                                Notika Email App hesabınızı oluşturduğunuz için teşekkür ederiz.
                                Hesabınızı aktif hale getirmek için aşağıdaki doğrulama kodunu
                                aktivasyon ekranına girmeniz yeterlidir.
                            </p>

                            <div style='background-color:#f1fffa; border:1px solid #b8f1df; border-radius:12px; padding:28px 20px; text-align:center; margin:28px 0;'>

                                <p style='margin:0 0 10px; color:#55706a; font-size:13px; text-transform:uppercase; letter-spacing:1.5px; font-weight:700;'>
                                    Aktivasyon Kodunuz
                                </p>

                                <div style='font-size:36px; font-weight:800; letter-spacing:10px; color:#00a67d;'>
                                    {code}
                                </div>

                            </div>

                            <p style='margin:0 0 18px; color:#666666; font-size:14px; line-height:1.6;'>
                                Bu kod yalnızca hesabınızı doğrulamak amacıyla gönderilmiştir.
                                Güvenliğiniz için kodu hiç kimseyle paylaşmayınız.
                            </p>

                            <div style='background-color:#fff8e7; border-left:4px solid #f5b942; padding:14px 16px; border-radius:6px; margin-top:24px;'>

                                <p style='margin:0; color:#765b16; font-size:13px; line-height:1.5;'>
                                    Bu işlemi siz başlatmadıysanız e-postayı dikkate almayabilirsiniz.
                                </p>

                            </div>

                        </td>
                    </tr>

                    <tr>
                        <td style='background-color:#f8faf9; padding:22px 30px; text-align:center; border-top:1px solid #eeeeee;'>

                            <p style='margin:0 0 6px; color:#888888; font-size:12px;'>
                                Bu e-posta otomatik olarak gönderilmiştir.
                            </p>

                            <p style='margin:0; color:#00a67d; font-size:12px; font-weight:700;'>
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
        $"Merhaba {model.Name},\n\n" +
        $"Notika Email App hesabınızı doğrulamak için aktivasyon kodunuz: {code}\n\n" +
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
            return RedirectToAction( "UserActivation", "Activation");
        }
    }
}
