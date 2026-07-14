using IdentityEmailApp.Context;
using IdentityEmailApp.Entities;
using IdentityEmailApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityEmailApp.Controllers
{
    public class MessageController : Controller
    {
        private readonly EmailContext _context;
        private readonly UserManager<AppUser> _userManager;


        public MessageController(EmailContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Inbox()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var values = await (
                from m in _context.Messages
                join u in _context.Users
                    on m.SenderEmail equals u.Email into userGroup
                from sender in userGroup.DefaultIfEmpty()
                where m.ReceiverEmail == user.Email
                select new MessageWithSenderInfoModel
                {
                    MessageId = m.MessageId,
                    MessageDetail = m.MessageDetail,
                    Subject = m.Subject,
                    SendDate = m.SendDate,
                    SenderEmail = m.SenderEmail,
                    SenderName = sender != null ? sender.Name : "Bilinmeyen",
                    SenderSurname = sender != null ? sender.Surname : "Kullanıcı",
                    IsRead = m.IsRead
                }).ToListAsync();

            return View(values);
        }
        public async Task<IActionResult> SendBox()
        {
            var user = await _userManager.GetUserAsync(User);
            var values = await _context.Messages.Where(x => x.SenderEmail == user.Email ).ToListAsync();
            return View(values);
        }

        public async Task<IActionResult> MessageDetail(int id)
        {
            var value = await _context.Messages.Where(x => x.MessageId == 1).FirstOrDefaultAsync();
            return View(value);
        }

        [HttpGet]
        public IActionResult ComposeMessage()
        {
            return View();
        }
    }
}
