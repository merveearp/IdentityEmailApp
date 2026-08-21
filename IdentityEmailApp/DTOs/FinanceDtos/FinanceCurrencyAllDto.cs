using Newtonsoft.Json;

namespace IdentityEmailApp.DTOs.FinanceDtos
{
    public class FinanceCurrencyAllDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("result")]
        public List<FinanceCurrencyResultDto> Result { get; set; }
            = new();
    }

    public class FinanceCurrencyResultDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("buying")]
        public decimal Buying { get; set; }

        [JsonProperty("selling")]
        public decimal Selling { get; set; }
    }
}