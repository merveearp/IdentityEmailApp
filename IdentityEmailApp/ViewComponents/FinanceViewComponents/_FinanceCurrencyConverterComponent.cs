using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.ViewComponents.FinanceViewComponents
{
    public class _FinanceCurrencyConverterComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
