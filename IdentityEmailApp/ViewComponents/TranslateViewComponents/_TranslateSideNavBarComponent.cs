using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.ViewComponents.TranslateViewComponents
{
    public class _TranslateSideNavBarComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
