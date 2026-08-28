using IdentityEmailApp.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.ViewComponents.NewViewComponents
{
    public class _NewMoreNewsComponent : ViewComponent
    {
        private readonly INewsService _newsService;

        public _NewMoreNewsComponent(INewsService newsService)
        {
            _newsService = newsService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var news = await _newsService.GetNewsAsync();

            var values = news?
                .Take(4)
                .ToList() ?? new();

            return View(values);
        }
    }
}