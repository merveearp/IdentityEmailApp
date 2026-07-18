using IdentityEmailApp.Entities;
using IdentityEmailApp.Models.PasswordModels;
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

        public PasswordController(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> ForgetPassword()
        {
            

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var passwordResetTokenLink = Url.Action("ResetPassword", "PasswordChange", new
            {
                userId=user.Id,
                token=passwordResetToken

            },HttpContext.Request.Scheme);

            MimeMessage mimeMessage = new MimeMessage();
            MailboxAddress mailboxAddress = new MailboxAddress("Notika Email App", "merveearp@gmail.com");

            mimeMessage.From.Add(mailboxAddress);
            MailboxAddress mailboxAddressTo = new MailboxAddress("User", model.Email);
            mimeMessage.To.Add(mailboxAddressTo);

            var bodyBuilder =new BodyBuilder();
            bodyBuilder.TextBody = passwordResetTokenLink;
            mimeMessage.Body = bodyBuilder.ToMessageBody();

            mimeMessage.Subject = "Şifre Değişiklik Talebi ";

            var appPassword =
                   _configuration["EmailSettings:AppPassword"];
           
            SmtpClient client = new SmtpClient();

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

            return RedirectToAction("PasswordResetLinkSent");
        }

        [HttpGet]
        public IActionResult PasswordResetLinkSent()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string userId,string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = TempData["userId"];
            var token = TempData["token"];
     
            if(userId==null || token ==null)
            {
                ViewBag.v = "Hata oluştu";
            }
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var result = await _userManager.ResetPasswordAsync(user, token.ToString(), model.Password);
            

            if(result.Succeeded)
            {
                return RedirectToAction("SignIn", "Login");
            }

            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> ResendResetPasswordMail(string email)
        //{
        //    var user = await _userManager.FindByEmailAsync(email);

        //    if(user != null)
        //    {
        //        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //        var link = Url.Action("ResetPassword", "Password",
        //            new { email = user.Email, token },
        //            Request.Scheme);
                
        //        await     
                    
        //    }
        //}

    }
}
