using Newtonsoft.Json;

namespace IdentityEmailApp.DTOs.FinanceDtos
{
    public class FinanceGoldAllDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("result")]
        public List<FinanceGoldResultDto> Result { get; set; }
            = new();
    }

    public class FinanceGoldResultDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("buy")]
        public string Buy { get; set; }

        [JsonProperty("sell")]
        public string Sell { get; set; }
    }
}