using IdentityEmailApp.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.ViewComponents.NewViewComponents
{
    public class _NewLocalComponent :ViewComponent
    {
        private readonly INewsService _newsService;

        public _NewLocalComponent(INewsService newsService)
        {
            _newsService = newsService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _newsService.GetLocalNewsAsync();
            return View(values);
        }
    }
}
