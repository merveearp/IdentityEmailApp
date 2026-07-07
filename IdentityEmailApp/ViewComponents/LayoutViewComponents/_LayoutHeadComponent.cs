using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.ViewComponents.LayoutViewComponents
{
    public class _LayoutHeadComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
