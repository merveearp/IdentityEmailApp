using IdentityEmailApp.Dtos.FinanceDtos;
using IdentityEmailApp.DTOs.FinanceDtos;
using IdentityEmailApp.Services.Abstract;
using Newtonsoft.Json;
using System.Globalization;
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
                JsonConvert.DeserializeObject<FinanceStockAllDto>(json );

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
            var apiKey = _configuration["RapidApi:ApiKey"];

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,

                RequestUri = new Uri(
                    "https://harem-altin-live-gold-price-data.p.rapidapi.com/" +
                    "harem_altin/prices/23b4c2fb31a242d1eebc0df9b9b65e5e"),

                Headers =
        {
            {
                "x-rapidapi-key",
                apiKey
            },
            {
                "x-rapidapi-host",
                "harem-altin-live-gold-price-data.p.rapidapi.com"
            }
        }
            };

            var response =await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<FinanceGoldAllDto>(body);

            if (result == null)
            {
                return new List<FinanceGoldResultDto>();
            }

            return result.Data
                .Where(x =>
                    x.Key == "GRAM ALTIN" ||
                    x.Key == "YENİ ÇEYREK" ||
                    x.Key == "YENİ YARIM" ||
                    x.Key == "YENİ TAM" ||
                    x.Key == "YENİ ATA")
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


            return new FinanceStockOverviewDto
            {
                FeaturedStocks = featuredStocks

            };
        }

        public async Task<List<FinanceCreditRateResultDto>> GetCreditRatesAsync(int price = 1000, int month = 12, string query = "ihtiyac")
        {
            var apiKey =  _configuration["CurrencyApi:ApiKey"];

            _httpClient.DefaultRequestHeaders.Clear();

            _httpClient.DefaultRequestHeaders.Add(
                "authorization",
                $"apikey {apiKey}");

            if (price <= 0)
            {
                price = 1000;
            }

            if (month <= 0)
            {
                month = 12;
            }

            if (string.IsNullOrWhiteSpace(query))
            {
                query = "ihtiyac";
            }

            var url =
               $"https://api.collectapi.com/credit/creditBid" +
               $"?data.price={price}" +
               $"&data.month={month}" +
               $"&data.query={query}";

            var response = await _httpClient.GetAsync(url);

            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Kredi API hatası: {json}");
            }

            var result =
                JsonConvert.DeserializeObject<
                    FinanceCreditRateAllDto>(json);

            return result?.Result
                .Take(5)
                .ToList()
                ?? new List<FinanceCreditRateResultDto>();
        }


            public async Task<FinanceExchangeResultDto> ExchangeCurrencyAsync(decimal amount, string fromCurrency,string toCurrency)
        {
            var apiKey = _configuration["CurrencyApi:ApiKey"];

            var formattedAmount = amount.ToString(
                CultureInfo.InvariantCulture);

            var url =
                $"https://api.collectapi.com/economy/exchange" +
                $"?int={formattedAmount}" +
                $"&to={toCurrency}" +
                $"&base={fromCurrency}";

            _httpClient.DefaultRequestHeaders.Clear();

            _httpClient.DefaultRequestHeaders.Add(
                "authorization",
                $"apikey {apiKey}");

            var response = await _httpClient.GetAsync(url);

            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Dönüştürücü API hatası: {json}");
            }

            var exchangeResponse = JsonConvert.DeserializeObject<FinanceExchangeDto>(json);

            return exchangeResponse?.Result?.FirstOrDefault() ?? new FinanceExchangeResultDto();
        }
    }
}
