using IdentityEmailApp.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.ViewComponents.FinanceViewComponents
{
    public class _FinanceExhangeComponent:ViewComponent
    {
        private readonly IFinanceService _financeService;

        public _FinanceExhangeComponent(IFinanceService financeService)
        {
            _financeService = financeService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            decimal amount = 1;
            string fromCurrency = "EUR";
            string toCurrency = "TRY";

            var result =
               await _financeService.ExchangeCurrencyAsync(amount,fromCurrency, toCurrency);

            ViewBag.Amount = amount;
            ViewBag.FromCurrency = fromCurrency;
            ViewBag.ToCurrency = toCurrency;

            return View(result);
        }
    }
}
