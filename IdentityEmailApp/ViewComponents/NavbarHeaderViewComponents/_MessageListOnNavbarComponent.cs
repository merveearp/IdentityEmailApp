using IdentityEmailApp.Context;
using IdentityEmailApp.Entities;
using IdentityEmailApp.Models.MessageModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityEmailApp.ViewComponents.NavbarHeaderViewComponents
{
    public class _MessageListOnNavbarComponent : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly EmailContext _context;

        public _MessageListOnNavbarComponent(
            UserManager<AppUser> userManager,
            EmailContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return View(new List<Message>());
            }

          

            var unreadMessageCount = await _context.Messages
                .CountAsync(x =>
                    x.ReceiverEmail == user.Email &&
                    !x.IsRead);

            ViewBag.UnreadMessageCount = unreadMessageCount;

            var messages = await (
             from message in _context.Messages
             join sender in _context.Users
                 on message.SenderEmail equals sender.Email into senderGroup
             from sender in senderGroup.DefaultIfEmpty()
             where message.ReceiverEmail == user.Email
                   && !message.IsRead
             orderby message.SendDate descending
             select new NavbarMessageViewModel
             {
                 MessageId = message.MessageId,

                 SenderName = sender != null
                     ? sender.Name
                     : message.SenderEmail,

                 SenderSurname = sender != null
                     ? sender.Surname
                     : string.Empty,

                 SenderEmail = message.SenderEmail,

                 SenderImageUrl = sender != null
                     ? sender.ImageUrl
                     : "/images/profile/image.png",

                 Subject = message.Subject,
                 MessageDetail = message.MessageDetail,
                 SendDate = message.SendDate
             })
             .Take(5)
             .ToListAsync();

            return View(messages);
        }
    }
}