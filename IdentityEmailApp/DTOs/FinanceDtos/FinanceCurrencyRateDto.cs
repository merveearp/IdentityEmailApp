using Newtonsoft.Json;

namespace IdentityEmailApp.DTOs.FinanceDtos
{

    public class FinanceCurrencyRateDto
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("base")]
        public string Base { get; set; }

        [JsonProperty("quote")]
        public string Quote { get; set; }

        [JsonProperty("rate")]
        public decimal Rate { get; set; }
    }

  
    public class FinanceCurrencyConversionDto
    {
        public string FromCurrency { get; set; }

        public string ToCurrency { get; set; }

        public decimal Amount { get; set; }

        public decimal Rate { get; set; }

        public decimal ConvertedAmount { get; set; }

        public string Date { get; set; }
    }
}