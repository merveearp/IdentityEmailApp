using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.ViewComponents.NewViewComponents
{
    public class _NewNavigationLinkComponent :ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
