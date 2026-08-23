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

            _httpClient.DefaultRequestHeaders.Add(
                "authorization",
                $"apikey {apiKey}");

            var response = await _httpClient.GetAsync(
                "https://api.collectapi.com/economy/hisseSenedi");

            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Borsa API hatası: {json}");
            }

            var result =
                JsonConvert.DeserializeObject<FinanceStockAllDto>(
                    json
                );

            return result ?? new FinanceStockAllDto();
        }
      

        public async Task<List<FinanceLiveStockResultDto>> GetLiveStocksAsync()
        {
            var apiKey = _configuration["CurrencyApi:ApiKey"];
            _httpClient.DefaultRequestHeaders.Clear();

            _httpClient.DefaultRequestHeaders.Add("authorization", $"apikey {apiKey}");
            
            var repsonse = await _httpClient.GetAsync("https://api.collectapi.com/economy/liveBorsa");
            var json = await repsonse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<FinanceLiveStockDto>(json);

            return result?.Result
                .OrderByDescending(x => Math.Abs(x.Rate))
                .Take(5)
                .ToList()
                ?? new List<FinanceLiveStockResultDto>();
                

        }

        public async Task<List<FinanceGoldResultDto>> GetGoldPricesAsync()
        {
            var apiKey =
                _configuration["CurrencyApi:ApiKey"];

            _httpClient.DefaultRequestHeaders.Clear();

            _httpClient.DefaultRequestHeaders.Add(
                "authorization",
                $"apikey {apiKey}");


            var response = await _httpClient.GetAsync(
                "https://api.collectapi.com/economy/goldPrice");

            var json =
                await response.Content.ReadAsStringAsync();

            var result =
                JsonConvert.DeserializeObject<FinanceGoldAllDto>(
                    json);

            return result.Result
              .Where(x =>
                  !string.IsNullOrWhiteSpace(x.Buy) &&
                  !string.IsNullOrWhiteSpace(x.Sell) &&
                  x.Buy != "-" &&
                  x.Sell != "-")
              .Take(5)
              .ToList();
        }

        public async Task<FinanceStockOverviewDto> GetStockOverviewAsync()
        {
           
            var stockData = await GetAllStocksAsync();

            var featuredStocks = stockData.Result
                .Where(x =>
                    x.Code == "THYAO" ||
                    x.Code == "ASELS" ||
                    x.Code == "AKBNK" ||
                    x.Code == "GARAN" ||
                    x.Code == "TUPRS")
                .ToList();

            var radar = new FinanceStockRadarDto
            {
                TopGainer = stockData.Result
                    .OrderByDescending(x => x.Rate)
                    .FirstOrDefault(),

                TopLoser = stockData.Result
                    .OrderBy(x => x.Rate)
                    .FirstOrDefault(),

                HighestVolume = stockData.Result
                    .OrderByDescending(x => x.Hacim)
                    .FirstOrDefault()
            };

            return new FinanceStockOverviewDto
            {
                FeaturedStocks = featuredStocks,
                Radar = radar
            };
        }
    }
}
