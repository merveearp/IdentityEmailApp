using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.ViewComponents.LayoutViewComponents
{
    public class _LayoutScriptsComponent :ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
