using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.ViewComponents.NewViewComponents
{
    public class _NewHeadComponent :ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
