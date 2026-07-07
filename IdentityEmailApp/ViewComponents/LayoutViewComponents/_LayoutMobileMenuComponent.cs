using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.ViewComponents.LayoutViewComponents
{
    public class _LayoutMobileMenuComponent :ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
