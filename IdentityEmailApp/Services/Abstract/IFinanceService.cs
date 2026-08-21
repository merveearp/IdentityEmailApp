using IdentityEmailApp.DTOs.FinanceDtos;

namespace IdentityEmailApp.Services.Abstract
{
    public interface IFinanceService
    {
        //currency
        Task<List<FinanceCurrencyResultDto>> GetCurrenciesAsync(); 


        //Hisse senetleri
        Task<FinanceStockRadarDto> GetStockRadarAsync();
        Task<List<FinanceStockResultDto>> GetFeaturedStocksAsync();
    }
}
