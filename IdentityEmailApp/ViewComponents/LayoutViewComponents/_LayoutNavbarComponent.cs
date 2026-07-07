using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.ViewComponents.LayoutViewComponents
{
    public class _LayoutNavbarComponent :ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
            
    }
}
