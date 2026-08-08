using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.ViewComponents.TaskViewComponents
{
    public class _TaskHeaderComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
