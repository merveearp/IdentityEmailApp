using IdentityEmailApp.Context;
using IdentityEmailApp.DTOs.AIDtos;
using IdentityEmailApp.Services.Abstract;
using Microsoft.EntityFrameworkCore;
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
        private readonly string _url;

        public AIGenerateResponse(
            HttpClient httpClient,
            IConfiguration configuration,
            EmailContext context)
        {
            _httpClient = httpClient;
            _context = context;

            _apiKey = configuration["OpenAI:ApiKey"]
                ?? throw new Exception("OpenAI API anahtarı bulunamadı.");

            _url = "https://api.openai.com/v1/chat/completions";

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<SpamAnalysisDto> AnalyzeSpamAsync(int messageId)
        {
            var message = await _context.Messages
                .FirstOrDefaultAsync(x => x.MessageId == messageId);

            if (message == null)
            {
                throw new Exception("Mesaj bulunamadı.");
            }

            var prompt = $@"Aşağıdaki e-postayı spam olma ihtimaline göre analiz et.

            Gönderen: {message.SenderEmail}
            Konu: {message.Subject}
            Mesaj içeriği: {message.MessageDetail}

            Sonucu yalnızca aşağıdaki JSON formatında döndür:

            {{
                ""isSpam"": true,
                ""spamScore"": 85
            }}

            spamScore değerini 0 ile 100 arasında belirle.
            70 ve üzerindeki sonuçlarda isSpam değerini true yap.
            JSON dışında açıklama veya başka bir metin yazma.";

            var requestBody = new
            {
                model = "gpt-4.1-mini",

                messages = new[]
                {
            new
            {
                role = "system",
                content = """
                    Sen profesyonel bir e-posta spam analiz asistanısın.
                    E-postayı spam belirtilerine göre analiz et.
                    Yalnızca istenen JSON sonucunu döndür.
                    """
            },
            new
            {
                role = "user",
                content = prompt
            }
        },

                response_format = new
                {
                    type = "json_object"
                },

                temperature = 0.1
            };

            var json = JsonSerializer.Serialize(requestBody);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(_url, content);

            var jsonString = await response.Content.ReadAsStringAsync();

            var openAIResult =
                JsonSerializer.Deserialize<OpenAIResponseDto>(jsonString);

            var result =
                JsonSerializer.Deserialize<SpamAnalysisDto>(
                    openAIResult.choices[0].message.content
                );

            return result;
        }

        public async Task<string> GenerateResponseAsync(int messageId)
        {
            var message = await _context.Messages.FindAsync(messageId);

            if (message == null)
            {
                throw new Exception("Mesaj bulunamadı.");
            }

            if (string.IsNullOrWhiteSpace(message.MessageDetail))
            {
                throw new Exception("Mesaj içeriği boş.");
            }

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

            using var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            using var response = await _httpClient.PostAsync(_url, content);

            var jsonString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"OpenAI API hatası: {response.StatusCode} - {jsonString}");
            }

            var result =
                JsonSerializer.Deserialize<OpenAIResponseDto>(jsonString);

            if (result?.choices == null ||
                result.choices.Count == 0 ||
                result.choices[0].message == null ||
                string.IsNullOrWhiteSpace(result.choices[0].message.content))
            {
                throw new Exception("OpenAI geçerli bir yanıt oluşturamadı.");
            }

            return result.choices[0].message.content;
        }
    }
}