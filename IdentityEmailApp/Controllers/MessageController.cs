using IdentityEmailApp.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityEmailApp.Controllers
{
    public class MessageController : Controller
    {
        private readonly EmailContext _context;

        public MessageController(EmailContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Inbox()
        {
            var values = await _context.Messages.Where(x => x.ReceiverEmail == "merve@notika.com").ToListAsync();
            return View(values);
        }
        public async Task<IActionResult> SendBox()
        {
            var values = await _context.Messages.Where(x => x.SenderEmail == "merve@notika.com").ToListAsync();
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
