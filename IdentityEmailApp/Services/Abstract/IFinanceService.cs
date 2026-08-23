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
    }
}
