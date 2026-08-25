using IdentityEmailApp.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.ViewComponents.FinanceViewComponents
{
    public class _FinanceCreditRatesComponent : ViewComponent
    {
        private readonly IFinanceService _financeService;

        public _FinanceCreditRatesComponent(IFinanceService financeService)
        {
            _financeService = financeService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _financeService.GetCreditRatesAsync();
            return View(values);
        }
    }
}
