using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.ViewComponents
{
    public class _HomeHeadViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
