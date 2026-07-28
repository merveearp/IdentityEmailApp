using IdentityEmailApp.Context;
using IdentityEmailApp.DTOs.AIDtos;
using IdentityEmailApp.Services.Abstract;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace IdentityEmailApp.Services.Concrete
{
    public class AIGenerateResponse : IAIGenerateResponse
    {
        private readonly HttpClient _httpClient;
        private readonly EmailContext _context;
        private readonly string _apiKey;
        private readonly string url;

        public AIGenerateResponse(HttpClient httpClient, IConfiguration configuration, EmailContext context)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenAI:ApiKey"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            url = "https://api.openai.com/v1/chat/completions";
            _context = context;
        }

        // Changed return type from Task<OpenAIResponseDto> to Task<string>
        public async Task<string> GenerateResponseAsync(int messageId)
        {
            var message = await _context.Messages.FindAsync(messageId);

            if (message == null)
                throw new Exception("Mesaj bulunamadı.");

            var requestBody = new
            {
                model = "gpt-4.1-mini",
                messages = new[]
                {
            new
            {
                role = "system",
                content = @"Sen profesyonel bir e-posta yanıt asistanısın.
                    Kullanıcının gönderdiği metin gelen e-postadır.
                    Bu e-postaya uygun, nazik ve profesyonel bir Türkçe cevap hazırla.
                    Sadece gönderilecek e-posta metnini döndür."
            },
            new
            {
                role = "user",
                content = message.MessageDetail
            }
        },
                temperature = 0.7
            };

            var json = JsonSerializer.Serialize(requestBody);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("OpenAI API hatası: " + response.StatusCode);
            }

            var jsonString = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<OpenAIResponseDto>(jsonString);

           
            return result.Choices[0].Message.Content;
        }

       
    }
}