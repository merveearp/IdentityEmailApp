using Newtonsoft.Json;

namespace IdentityEmailApp.DTOs.FinanceDtos
{
    public class FinanceGoldAllDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public List<FinanceGoldResultDto> Data { get; set; }
            = new();
    }

    public class FinanceGoldResultDto
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("buy")]
        public string Buy { get; set; }

        [JsonProperty("sell")]
        public string Sell { get; set; }

        [JsonProperty("percent")]
        public string Percent { get; set; }

        [JsonProperty("arrow")]
        public string Arrow { get; set; }

        [JsonProperty("last_update")]
        public string LastUpdate { get; set; }
    }
}