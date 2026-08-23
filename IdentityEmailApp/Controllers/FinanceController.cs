using IdentityEmailApp.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.Controllers
{
    public class FinanceController : Controller
    {
        private readonly IFinanceService _financeService;

        public FinanceController(IFinanceService financeService)
        {
            _financeService = financeService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetLiveStocks()
        {
            var values = await _financeService.GetLiveStocksAsync();

            return Json(values);
        }
    }
}
