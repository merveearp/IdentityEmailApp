using IdentityEmailApp.DTOs.NewsDtos;
using IdentityEmailApp.Services.Abstract;
using Newtonsoft.Json;

namespace IdentityEmailApp.Services.Concrete
{
    public class NewsService : INewsService
    {
        private readonly string rapidapi_key = "b64f56ed1emsh8afba1e8adc4772p11a325jsn1d2ee2523f59";
        private readonly string rapidapi_host_latest = "google-news13.p.rapidapi.com";

        public async Task<List<ResultLatestOfNewDto.Item>> GetCurrentNewsAsync()
        {
           
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://google-news13.p.rapidapi.com/latest?lr=tr-TR"),
                Headers =
    {
        { "x-rapidapi-key", rapidapi_key },
        { "x-rapidapi-host", rapidapi_host_latest },
    },
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();

                var body = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<ResultLatestOfNewDto>(body);

                return result?.items ?? new List<ResultLatestOfNewDto.Item>();
            }
        }

       
    }
}
