using IdentityEmailApp.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.ViewComponents.NewViewComponents
{
    public class _NewLatestComponent :ViewComponent
    {
        private readonly INewsService _newsService;

        public _NewLatestComponent(INewsService newsService)
        {
            _newsService = newsService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _newsService.GetCurrentNewsAsync();
            return View(values);


        }
    }
}
