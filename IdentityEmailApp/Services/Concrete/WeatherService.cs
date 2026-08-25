using System.Globalization;
using System.Net.Http.Json;
using IdentityEmailApp.DTOs.WeatherDtos;
using IdentityEmailApp.Services.Abstract;

namespace IdentityEmailApp.Services.Concrete
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        private const string RapidApiHost =
            "open-weather13.p.rapidapi.com";

        public WeatherService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<GetCityWeatherDto?> GetWeatherByLocationAsync( double latitude = 41.0082, double longitude = 28.9784)
        {
            var rapidApiKey =
                _configuration["RapidApi:ApiKey"];

            if (string.IsNullOrWhiteSpace(rapidApiKey))
            {
                throw new Exception(
                    "RapidAPI anahtarı bulunamadı.");
            }

            var latitudeText = latitude.ToString(
                CultureInfo.InvariantCulture);

            var longitudeText = longitude.ToString(
                CultureInfo.InvariantCulture);

            var url =
                "https://open-weather13.p.rapidapi.com/fivedaysforcast" +
                $"?latitude={latitudeText}" +
                $"&longitude={longitudeText}" +
                "&lang=TR";

            using var request = new HttpRequestMessage(
                HttpMethod.Get,
                url);

            request.Headers.Add(
                "x-rapidapi-key",
                rapidApiKey);

            request.Headers.Add(
                "x-rapidapi-host",
                RapidApiHost);

            using var response =
                await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage =
                    await response.Content.ReadAsStringAsync();

                throw new Exception(
                    $"Hava durumu API hatası: {errorMessage}");
            }

            return await response.Content
                .ReadFromJsonAsync<GetCityWeatherDto>();
        }
    }
}