using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.ViewComponents.LayoutViewComponents
{
    public class _LayoutHeaderComponent :ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
