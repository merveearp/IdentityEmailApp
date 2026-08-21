using IdentityEmailApp.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.ViewComponents.FinanceViewComponents
{
    public class _FinanceStockRadarComponent:ViewComponent
    {
        private readonly IFinanceService _financeService;

        public _FinanceStockRadarComponent(IFinanceService financeService)
        {
            _financeService = financeService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _financeService.GetStockRadarAsync();


            return View(result);
        }
    }
}
