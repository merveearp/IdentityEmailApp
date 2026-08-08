using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.ViewComponents.TaskViewComponents
{
    public class _TaskHeadComponent :ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
