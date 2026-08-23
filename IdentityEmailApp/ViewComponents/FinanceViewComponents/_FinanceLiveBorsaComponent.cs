using IdentityEmailApp.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.ViewComponents.FinanceViewComponents
{
    public class _FinanceLiveBorsaComponent:ViewComponent
    {
        private readonly IFinanceService _financeService;

        public _FinanceLiveBorsaComponent(IFinanceService financeService)
        {
            _financeService = financeService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _financeService.GetLiveStocksAsync();
            return View(values);
        }
    }
}
