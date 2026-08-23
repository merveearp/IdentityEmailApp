using Newtonsoft.Json;

namespace IdentityEmailApp.DTOs.FinanceDtos
{
    public class FinanceLiveStockDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("result")]
        public List<FinanceLiveStockResultDto> Result { get; set; }
            = new();
    }
    public class FinanceLiveStockResultDto
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("pricestr")]
        public string PriceStr { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("rate")]
        public decimal Rate { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }
    }
}
