using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.ViewComponents.AISupportViewComponents
{
    public class _AISupportFooterComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
