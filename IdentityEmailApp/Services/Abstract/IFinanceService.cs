using IdentityEmailApp.Dtos.FinanceDtos;
using IdentityEmailApp.DTOs.FinanceDtos;

namespace IdentityEmailApp.Services.Abstract
{
    public interface IFinanceService
    {
        //currency
        Task<List<FinanceCurrencyResultDto>> GetCurrenciesAsync();

        //Hisse senetleri
        Task<FinanceStockOverviewDto> GetStockOverviewAsync();

        //live borsa
        Task<List<FinanceLiveStockResultDto>> GetLiveStocksAsync();

        //GoldPrice
        Task<List<FinanceGoldResultDto>> GetGoldPricesAsync();

        //kredi
        Task<List<FinanceCreditRateResultDto>>GetCreditRatesAsync(int price = 1000, int month = 12, string query = "ihtiyac");

        //exchange
        Task<FinanceExchangeResultDto> ExchangeCurrencyAsync( decimal amount, string fromCurrency,string toCurrency);
    }
}
