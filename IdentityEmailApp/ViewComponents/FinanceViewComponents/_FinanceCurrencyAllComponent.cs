using IdentityEmailApp.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.ViewComponents.FinanceViewComponents
{
    public class _FinanceCurrencyAllComponent:ViewComponent
    {
        private readonly IFinanceService _financeService;

        public _FinanceCurrencyAllComponent(IFinanceService financeService)
        {
            _financeService = financeService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _financeService.GetCurrenciesAsync();
            return View(values);
        }
    }
}
