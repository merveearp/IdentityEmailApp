using IdentityEmailApp.Context;
using IdentityEmailApp.Entities;
using IdentityEmailApp.Models.MessageModels;
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
       
        //----------GELEN KUTUSU---------//
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


                where m.ReceiverEmail == user.Email && m.IsDraft == false && m.IsDeleted==false && m.IsSpam == false
          
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
                    IsStarred = m.IsStarred,
                    CategoryName = category != null ? category.CategoryName :"Kategori Yok"
                }).OrderByDescending(x=>x.SendDate).ToListAsync();

            return View(values);
        }
       
        //----------GİDEN KUTUSU---------//
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


                where m.SenderEmail == user.Email && !m.IsDraft && !m.IsDeleted && !m.IsSpam
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
                }).OrderByDescending(x=>x.SendDate).ToListAsync();

            return View(values);
        }

        //-----------KATEGORİLERE GÖRE GELEN KUTUSU---------//
        public async Task<IActionResult> GetMessageListCategoryId(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var messageCategory = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);

            if (messageCategory == null)
            {
                return NotFound();
            }

            ViewBag.CategoryName = messageCategory.CategoryName;


            var values = await (
                from m in _context.Messages
                join u in _context.Users
                    on m.SenderEmail equals u.Email into userGroup
                from sender in userGroup.DefaultIfEmpty()

                join c in _context.Categories
                on m.CategoryId equals c.CategoryId into catgeoryGroup
                from category in catgeoryGroup.DefaultIfEmpty()


                where m.ReceiverEmail == user.Email && m.CategoryId == id


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
                    CategoryName = category.CategoryName

                }).ToListAsync();



            return View(values);
        }

        //----------MESAJ DETAY---------//
        public async Task<IActionResult> MessageDetail(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var message = await _context.Messages
                .FirstOrDefaultAsync(x =>
                    x.MessageId == id &&
                    (
                        x.ReceiverEmail == user.Email ||
                        x.SenderEmail == user.Email
                    ));

            if (message == null)
            {
                return NotFound();
            }

            var conversationId = message.ConversationId ?? message.MessageId;

            var conversationMessages = await _context.Messages
                .Include(x => x.Category)
                .Where(x =>
                    x.ConversationId == conversationId &&
                    (
                        x.ReceiverEmail == user.Email ||
                        x.SenderEmail == user.Email
                    ) &&
                    !x.IsDeleted &&
                    !x.IsDraft)
                .OrderBy(x => x.SendDate)
                .ToListAsync();

            var unreadMessages = conversationMessages
                .Where(x =>
                    x.ReceiverEmail == user.Email &&
                    !x.IsRead)
                .ToList();

            if (unreadMessages.Any())
            {
                foreach (var item in unreadMessages)
                {
                    item.IsRead = true;
                }

                await _context.SaveChangesAsync();
            }

            ViewBag.CurrentUserEmail = user.Email;
            ViewBag.ConversationId = conversationId;

            return View(conversationMessages);
        }

        //----------MESAJ GÖNDER---------//
        [HttpGet]
        public async Task<IActionResult> ComposeMessage()
        {
            await LoadCategoriesAsync();

            return View(new Message());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ComposeMessage(Message message, string actionType)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var isDraftAction = actionType == "draft";

            if (!isDraftAction && !ModelState.IsValid)
            {
                await LoadCategoriesAsync();
                return View(message);
            }

            if (message.MessageId > 0)
            {
                var existingDraft = await _context.Messages
                    .FirstOrDefaultAsync(x =>
                        x.MessageId == message.MessageId &&
                        x.SenderEmail == user.Email &&
                        x.IsDraft &&
                        !x.IsDeleted);

                if (existingDraft == null)
                {
                    return NotFound();
                }

                existingDraft.ReceiverEmail = message.ReceiverEmail;
                existingDraft.CategoryId = message.CategoryId;
                existingDraft.Subject = message.Subject;
                existingDraft.MessageDetail = message.MessageDetail;
                existingDraft.IsDraft = isDraftAction;

                if (!isDraftAction)
                {
                    existingDraft.SendDate = DateTime.Now;
                    existingDraft.IsRead = false;

                    if (existingDraft.ConversationId == null)
                    {
                        existingDraft.ConversationId = existingDraft.MessageId;
                    }
                }

                await _context.SaveChangesAsync();

                return isDraftAction
                    ? RedirectToAction(nameof(DraftedMessages))
                    : RedirectToAction(nameof(SendBox));
            }

            // Yeni mesaj oluşturuluyorsa
            message.SenderEmail = user.Email!;
            message.SendDate = DateTime.Now;
            message.IsRead = false;
            message.IsDraft = isDraftAction;

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            if (!isDraftAction)
            {
                message.ConversationId = message.MessageId;
                await _context.SaveChangesAsync();
            }

            return isDraftAction
                ? RedirectToAction(nameof(DraftedMessages))
                : RedirectToAction(nameof(SendBox));
        }

        //----------STARRED MESSAGE---------//
        public async Task<IActionResult> StarredMessages()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var messages = await _context.Messages
                 .Where(x =>
                     x.IsStarred &&
                     !x.IsDeleted &&
                     !x.IsDraft &&
                     (
                         x.ReceiverEmail == user.Email ||
                         x.SenderEmail == user.Email
                     ))
                 .OrderByDescending(x => x.SendDate)
                 .ToListAsync();

            return View(messages);
        }

        //----------STARRED ---------//

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStar(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var message = await _context.Messages
                .FirstOrDefaultAsync(x=>
                      x.MessageId == id &&
                      (x.ReceiverEmail == user.Email ||
                             x.SenderEmail == user.Email));

            if (message == null)
                return NotFound();

            message.IsStarred = !message.IsStarred;

            await _context.SaveChangesAsync();

            return Ok();

        }





        //----------DRAFT MESSAGE---------//
        public async Task<IActionResult> DraftedMessages()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var messages = await _context.Messages
                 .Where(x =>
                     
                     !x.IsSpam &&
                     !x.IsDeleted &&

                     x.IsDraft &&
                     (
                         x.SenderEmail == user.Email
                     ))
                 .OrderByDescending(x => x.SendDate)
                 .ToListAsync();

            return View(messages);
        }

        [HttpGet]
        public async Task<IActionResult> EditDraft(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var draft = await _context.Messages
                .FirstOrDefaultAsync(x =>
                    x.MessageId == id &&
                    x.SenderEmail == user.Email &&
                    x.IsDraft &&
                    !x.IsDeleted);

            if (draft == null)
            {
                return NotFound();
            }

            await LoadCategoriesAsync();

            return View("ComposeMessage", draft);
        }


        //---------- SPAM MESSAGES ----------//
        public async Task<IActionResult> SpamMessages()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var messages = await _context.Messages
                .Where(x =>
                    x.IsSpam &&
                    !x.IsDeleted &&
                    !x.IsDraft &&
                    x.ReceiverEmail == user.Email
                )
                .OrderByDescending(x => x.SendDate)
                .ToListAsync();

            return View(messages);
        }

        //----------SPAMMED ---------//

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> ToggleSpam(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var message = await _context.Messages
                .FirstOrDefaultAsync(x =>
                      x.MessageId == id &&
                      (x.ReceiverEmail == user.Email));

            if (message == null)
                return NotFound();

            message.IsSpam = !message.IsSpam;

            await _context.SaveChangesAsync();

            return Ok();


        }

        //---------- DELETED MESSAGES ----------//
        public async Task<IActionResult> DeletedMessages()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            ViewBag.UserMail = user.Email;

            var messages = await _context.Messages
                .Where(x =>
                    x.IsDeleted &&
                    !x.IsDraft &&
                    (
                        x.ReceiverEmail == user.Email ||
                        x.SenderEmail == user.Email
                    ))
                .OrderByDescending(x => x.SendDate)
                .ToListAsync();
            return View(messages);
        }

        //----------DELETED ---------//
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleDeleted(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var message = await _context.Messages
                .FirstOrDefaultAsync(x =>
                    x.MessageId == id &&
                    (
                        x.ReceiverEmail == user.Email ||
                        x.SenderEmail == user.Email
                    ));

            if (message == null)
            {
                return NotFound();
            }

            message.IsDeleted = !message.IsDeleted;

            await _context.SaveChangesAsync();

            return Ok();

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsRead(int id)
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
            
            message.IsRead = !message.IsRead;
            await _context.SaveChangesAsync();

            return RedirectToAction("Inbox");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var message = await _context.Messages
                .FirstOrDefaultAsync(x =>
                    x.MessageId == id &&
                    x.IsDeleted &&
                    (
                        x.ReceiverEmail == user.Email ||
                        x.SenderEmail == user.Email
                    ));

            if (message == null)
            {
                return NotFound();
            }

            _context.Messages.Remove(message);

            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true
            });
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReplyMessage(MessageReplyViewModel model)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] =
                    "Yanıt mesajı boş bırakılamaz.";

                return RedirectToAction(
                    nameof(MessageDetail),
                    new { id = model.ReplyMessageId });
            }

            var message = await _context.Messages
                .FirstOrDefaultAsync(x =>
                    x.MessageId == model.ReplyMessageId &&
                    (
                        x.ReceiverEmail == currentUser.Email ||
                        x.SenderEmail == currentUser.Email
                    ));

            if (message == null)
            {
                return NotFound();
            }

            var receiverEmail =
                message.SenderEmail == currentUser.Email
                    ? message.ReceiverEmail
                    : message.SenderEmail;

            var subject =
                message.Subject.StartsWith(
                    "Re:",
                    StringComparison.OrdinalIgnoreCase)
                        ? message.Subject
                        : $"Re: {message.Subject}";

            var conversationId =
                message.ConversationId ?? message.MessageId;

            var replyMessage = new Message
            {
                SenderEmail = currentUser.Email!,
                ReceiverEmail = receiverEmail,
                Subject = subject,
                MessageDetail = model.MessageDetail,
                SendDate = DateTime.Now,
                IsRead = false,
                IsDraft = false,
                CategoryId = message.CategoryId,
                ConversationId = conversationId
            };

            await _context.Messages.AddAsync(replyMessage);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Yanıtınız başarıyla gönderildi.";

            return RedirectToAction(
                nameof(MessageDetail),
                new { id = replyMessage.MessageId });
        }
    }
}
