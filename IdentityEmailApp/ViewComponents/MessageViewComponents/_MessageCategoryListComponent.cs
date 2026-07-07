using IdentityEmailApp.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityEmailApp.ViewComponents.MessageViewComponents
{
    public class _MessageCategoryListComponent:ViewComponent
    {
        private readonly EmailContext _context;

        public _MessageCategoryListComponent(EmailContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _context.Categories.Where(x=>x.CategoryStatus==true).ToListAsync();
            return View(values);
        }
    }
}
