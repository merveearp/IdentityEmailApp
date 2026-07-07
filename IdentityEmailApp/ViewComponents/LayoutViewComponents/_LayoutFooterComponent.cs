using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.ViewComponents.LayoutViewComponents
{
    public class _LayoutFooterComponent :ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
