using IdentityEmailApp.Enums;
using IdentityEmailApp.Services.Abstract;
using Microsoft.CodeAnalysis.Elfie.Model.Structures;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace IdentityEmailApp.Services.Concrete
{
    public class AISupportService : IAISupportService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        

        public AISupportService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }


        public async Task<string> GetSupportResponseAsync(string question)
         {
        var apiKey = _configuration["OpenAI:ApiKey"];

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", apiKey);

        var requestBody = new
        {
            model = "gpt-4o-mini",

            messages = new[]
            {
            new
            {
                role = "system",
                content = """
                    Sen, Notika uygulaması içerisinde bulunan genel amaçlı
                    bir yapay zekâ asistanısın.

                    Kullanıcının sorularını açık, anlaşılır ve düzenli bir
                    şekilde yanıtla. Metin yazma, özetleme, fikir üretme,
                    planlama, çeviri, yazılım ve günlük konular dahil olmak
                    üzere farklı alanlarda yardımcı ol.

                    Kullanıcı hangi dilde yazıyorsa aynı dilde cevap ver.
                    Emin olmadığın bilgileri kesinmiş gibi sunma.
                    """
            },
            new
            {
                role = "user",
                content = question
            }
        }
        };

        var json = JsonConvert.SerializeObject(requestBody);

        var content = new StringContent(
            json,
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PostAsync(
            "https://api.openai.com/v1/chat/completions",
            content);

        var responseText =
            await response.Content.ReadAsStringAsync();

        var result = JObject.Parse(responseText);

        return result["choices"]?[0]?["message"]?["content"]?
            .ToString() ?? "Yanıt oluşturulamadı.";
    }
}
}
