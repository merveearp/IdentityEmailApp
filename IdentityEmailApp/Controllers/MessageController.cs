using IdentityEmailApp.Context;
using IdentityEmailApp.Entities;
using IdentityEmailApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

                join c in _context.Categories
                on m.CategoryId equals c.CategoryId into catgeoryGroup
                from category in catgeoryGroup.DefaultIfEmpty()


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
                    IsRead = m.IsRead,
                    CategoryName = category != null ? category.CategoryName :"Kategori Yok"
                }).ToListAsync();

            return View(values);
        }
        public async Task<IActionResult> SendBox()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var values = await (
                from m in _context.Messages
                join u in _context.Users
                    on m.ReceiverEmail equals u.Email into userGroup
                from receiver in userGroup.DefaultIfEmpty()

                join c in _context.Categories
                on m.CategoryId equals c.CategoryId into catgeoryGroup
                from category in catgeoryGroup.DefaultIfEmpty()


                where m.SenderEmail == user.Email
                select new MessageWithReceiverInfoModel
                {
                    MessageId = m.MessageId,
                    MessageDetail = m.MessageDetail,
                    Subject = m.Subject,
                    SendDate = m.SendDate,
                    ReceiverEmail = m.ReceiverEmail,
                    ReceiverName = receiver != null ? receiver.Name : "Bilinmeyen",
                    ReceiverSurname = receiver != null ? receiver.Surname : "Kullanıcı",
                    IsRead = m.IsRead,
                    CategoryName = category != null ? category.CategoryName : "Kategori Yok"
                }).ToListAsync();

            return View(values);
        }

        public async Task<IActionResult> MessageDetail(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var message = await _context.Messages
                .Include(c=>c.Category).FirstOrDefaultAsync(x =>
                    x.MessageId == id &&
                    x.ReceiverEmail == user.Email ||
                    x.SenderEmail == user.Email);

            if (message == null)
            {
                return NotFound();
            }

            var isReceiver = message.ReceiverEmail == user.Email;
            if(isReceiver && !message.IsRead)
            {
                message.IsRead = true;
                await _context.SaveChangesAsync();
            }

            ViewBag.IsReceiver = isReceiver;

            return View(message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> MarkAsUnRead(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var message = await _context.Messages.FirstOrDefaultAsync(x => x.MessageId == id && x.ReceiverEmail == user.Email);

            if(message ==null)
            {
                return NotFound();
            }

            message.IsRead = false;
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Mesaj başarıyla gönderildi.";

            return RedirectToAction("SendBox");
        }
      

        [HttpGet]
        public async Task<IActionResult> ComposeMessage()
        {
            await LoadCategoriesAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ComposeMessage(Message message)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync();
                return View(message);
            }

            message.SenderEmail = user.Email!;
            message.SendDate = DateTime.Now;
            message.IsRead = false;

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            return RedirectToAction("SendBox");
        }

        private async Task LoadCategoriesAsync()
        {
            var categories = await _context.Categories
                .AsNoTracking()
                .ToListAsync();

            ViewBag.v = categories.Select(x => new SelectListItem
            {
                Text = x.CategoryName,
                Value = x.CategoryId.ToString()
            }).ToList();
        }
    }
}
