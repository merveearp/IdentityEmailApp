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

        [HttpGet]
        public async Task<IActionResult> ConvertCurrency(decimal amount,string fromCurrency, string toCurrency)
        {
            if (amount <= 0)
            {
                return BadRequest("Miktar sıfırdan büyük olmalıdır.");
            }

            var result =
                await _financeService.ExchangeCurrencyAsync(
                    amount,
                    fromCurrency,
                    toCurrency);

            var exchangeData = result.Data.FirstOrDefault();

            if (exchangeData == null)
            {
                return BadRequest("Dönüşüm sonucu alınamadı.");
            }

            return Json(new
            {
                calculated = exchangeData.Calculated,
                rate = exchangeData.Rate
            });
        }
    }
}
