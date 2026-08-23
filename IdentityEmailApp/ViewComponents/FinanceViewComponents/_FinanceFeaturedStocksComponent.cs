using IdentityEmailApp.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.ViewComponents.FinanceViewComponents
{
    public class _FinanceFeaturedStocksComponent:ViewComponent
    {
        private readonly IFinanceService _financeService;

        public _FinanceFeaturedStocksComponent(IFinanceService financeService)
        {
            _financeService = financeService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _financeService.GetStockOverviewAsync();

            return View(result.FeaturedStocks);
        }
    }
}
