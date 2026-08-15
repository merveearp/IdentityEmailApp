using IdentityEmailApp.Services.Abstract;
using System.Text.Json;

namespace IdentityEmailApp.Services.Concrete
{
    public class TranslateService : ITranslateService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        private const string RapidApiHost =
            "google-translate113.p.rapidapi.com";

        public TranslateService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<string> TranslateAsync(string text, string targetLanguage = "tr", string sourceLanguage = "auto")
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException(
                    "Çevrilecek metin boş olamaz.",
                    nameof(text)
                );
            }
            var apiKey = _configuration["RapidApi:ApiKey"];

            if(string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException("RapidAPI anahtarı bulunamadı");
            }

            using var request = new HttpRequestMessage(HttpMethod.Post, "https://google-translate113.p.rapidapi.com/api/v1/translator/text");
            request.Headers.Add(
               "x-rapidapi-key",
               apiKey
           );

            request.Headers.Add(
                "x-rapidapi-host",
                RapidApiHost
            );
            request.Content = new FormUrlEncodedContent(
               new Dictionary<string, string>
               {
                    { "from", sourceLanguage },
                    { "to", targetLanguage },
                    { "text", text }
               }
           );

            using var response =
                await _httpClient.SendAsync(request);

            var responseContent =
                await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Çeviri işlemi başarısız oldu. " +
                    $"Durum kodu: {(int)response.StatusCode}"
                );
            }

            using var jsonDocument =
                JsonDocument.Parse(responseContent);

            if (!jsonDocument.RootElement.TryGetProperty(
                    "trans",
                    out var translatedElement))
            {
                throw new InvalidOperationException(
                    "API yanıtında çevrilmiş metin bulunamadı."
                );
            }

            return translatedElement.GetString() ?? string.Empty;
        }
    }
}
   
