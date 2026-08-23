using IdentityEmailApp.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.ViewComponents.FinanceViewComponents
{
    public class _FinanceGoldComponent:ViewComponent
    {
        private readonly IFinanceService _financeService;

        public _FinanceGoldComponent(IFinanceService financeService)
        {
            _financeService = financeService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _financeService.GetGoldPricesAsync();
            return View(values);
        }
    }
}
