using IdentityEmailApp.DTOs.FinanceDtos;
using IdentityEmailApp.Services.Abstract;
using Newtonsoft.Json;
using System.Security.AccessControl;

namespace IdentityEmailApp.Services.Concrete
{
    public class FinanceService : IFinanceService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public FinanceService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<List<FinanceCurrencyResultDto>> GetCurrenciesAsync()
        {
            var apiKey = _configuration["CurrencyApi:ApiKey"];
            _httpClient.DefaultRequestHeaders.Clear();

            _httpClient.DefaultRequestHeaders.Add("authorization", $"apikey {apiKey}");

            var response = await _httpClient.GetAsync(
                 "https://api.collectapi.com/economy/allCurrency");

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<FinanceCurrencyAllDto>(json);

            return result.Result
                .Where(x =>
                    x.Name.StartsWith("USD") ||
                    x.Name.StartsWith("EUR") ||
                    x.Name.StartsWith("CHF") ||
                    x.Name.StartsWith("JPY") ||
                    x.Name.StartsWith("GBP"))
                .ToList();
                    }

        private async Task<FinanceStockAllDto> GetAllStocksAsync()
        {
            var apiKey = _configuration["CurrencyApi:ApiKey"];

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("authorization", $"apikey {apiKey}");

            var response = await _httpClient.GetAsync(
          "https://api.collectapi.com/economy/hisseSenedi");
            var json = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<FinanceStockAllDto>(json);

            return result;

        }
        public async Task<List<FinanceStockResultDto>> GetFeaturedStocksAsync()
        {
            var stockData = await GetAllStocksAsync();

            return stockData.Result
               .Where(x =>
            x.Code == "THYAO" ||
            x.Code == "ASELS" ||
            x.Code == "AKBNK" ||
            x.Code == "GARAN" ||
            x.Code == "TUPRS")
                 .ToList();
        }

        public async Task<FinanceStockRadarDto> GetStockRadarAsync()
        {
            var stockData = await GetAllStocksAsync();

            return new FinanceStockRadarDto

            {
                TopGainer=stockData.Result.OrderByDescending(x=>x.Rate).FirstOrDefault(),

                TopLoser = stockData.Result.OrderBy(x => x.Rate).FirstOrDefault(),

                HighestVolume = stockData.Result.OrderByDescending(x => x.Hacim).FirstOrDefault()
            };
        
        }
    }
}
