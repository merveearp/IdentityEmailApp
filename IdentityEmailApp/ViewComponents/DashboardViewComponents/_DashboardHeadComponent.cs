using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.ViewComponents.DashboardViewComponents
{
    public class _DashboardHeadComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }

    }
}
