using IdentityEmailApp.DTOs.NewsDtos;
using IdentityEmailApp.Services.Abstract;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace IdentityEmailApp.Services.Concrete
{
    public class NewsService : INewsService
    {
        private readonly IConfiguration _configuration;
        private readonly string rapidapi_host_latest = "google-news13.p.rapidapi.com";

        private readonly IMemoryCache _memoryCache;

        public NewsService(IMemoryCache cache, IConfiguration configuration)
        {
            _memoryCache = cache;
            _configuration = configuration;
        }

        public async Task<List<ResultLatestOfNewDto.Item>> GetCurrentNewsAsync()
        {
            var rapidapi_key = _configuration["GoogleNewsApi:ApiKey"];
            const string cacheKey = "CurrentNews";

            if (_memoryCache.TryGetValue(cacheKey, out List<ResultLatestOfNewDto.Item>? cachedNews))
            {
                return cachedNews!;
            }

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

                var news = result?.items ?? new List<ResultLatestOfNewDto.Item>();

               
                _memoryCache.Set(cacheKey, news, TimeSpan.FromMinutes(30));

                return news;
            }
        }

        public async Task<List<ResultLatestOfNewDto.Item>> GetLocalNewsAsync()
        {
            var rapidapi_key = _configuration["GoogleNewsApi:ApiKey"];
            const string cacheKey = "LocalNews";

            if (_memoryCache.TryGetValue(cacheKey, out List<ResultLatestOfNewDto.Item>? cachedNews))
            {
                return cachedNews!;
            }

            using var client = new HttpClient();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://google-news13.p.rapidapi.com/entertainment?lr=tr-TR"),
                Headers =
        {
            { "x-rapidapi-key", rapidapi_key },
            { "x-rapidapi-host", rapidapi_host_latest },
        },
            };

            using var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return new List<ResultLatestOfNewDto.Item>();
            }

            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResultLatestOfNewDto>(body);

            var news = result?.items ?? new List<ResultLatestOfNewDto.Item>();

            _memoryCache.Set(cacheKey, news, TimeSpan.FromMinutes(30));

            return news;
        }

        public async Task<List<ResultLatestOfNewDto.Subnew>> GetNewsAsync()
        {
            var rapidapi_key = _configuration["GoogleNewsApi:ApiKey"];
            const string cacheKey = "entertainment-subnews";

            if (_memoryCache.TryGetValue(
                cacheKey,
                out List<ResultLatestOfNewDto.Subnew>? cachedNews))
            {
                return cachedNews!;
            }

            using var client = new HttpClient();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(
                    "https://google-news13.p.rapidapi.com/entertainment?lr=tr-TR"),
                Headers =
        {
            { "x-rapidapi-key", rapidapi_key },
            { "x-rapidapi-host", rapidapi_host_latest }
        }
            };

            using var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return new List<ResultLatestOfNewDto.Subnew>();
            }

            var body = await response.Content.ReadAsStringAsync();

            var result =
                JsonConvert.DeserializeObject<ResultLatestOfNewDto>(body);

            var randomItem = result?.items?
                .Where(x =>
                    x.hasSubnews &&
                    x.subnews != null &&
                    x.subnews.Any())
                .OrderBy(x => Guid.NewGuid())
                .FirstOrDefault();

            var news = randomItem?.subnews
                       ?? new List<ResultLatestOfNewDto.Subnew>();

            _memoryCache.Set(
                cacheKey,
                news,
                TimeSpan.FromMinutes(30));

            return news;
        }

        public async Task<List<ResultLatestOfNewDto.Item>> GetCategoryByNewsAsync(string category)
        {
            var rapidapi_key = _configuration["GoogleNewsApi:ApiKey"];
            var searchCategory = string.IsNullOrWhiteSpace(category)
                ? "latest"
                : category.ToLower();

            var cacheKey = $"CategoryNews_{searchCategory}";

            if (_memoryCache.TryGetValue(
                cacheKey,
                out List<ResultLatestOfNewDto.Item>? cachedNews))
            {
                return cachedNews!;
            }

            using var client = new HttpClient();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(
                    $"https://google-news13.p.rapidapi.com/{searchCategory}?lr=tr-TR"),
                Headers =
        {
            { "x-rapidapi-key", rapidapi_key },
            { "x-rapidapi-host", rapidapi_host_latest },
        },
            };

            using var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return new List<ResultLatestOfNewDto.Item>();
            }

            var body = await response.Content.ReadAsStringAsync();

            var result =
                JsonConvert.DeserializeObject<ResultLatestOfNewDto>(body);

            var news = result?.items
                       ?? new List<ResultLatestOfNewDto.Item>();

            _memoryCache.Set(
                cacheKey,
                news,
                TimeSpan.FromMinutes(30));

            return news;
        }

    }
}

