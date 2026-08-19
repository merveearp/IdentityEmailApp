using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.ViewComponents.FinanceViewComponents
{
    public class _FinanceHeaderComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
