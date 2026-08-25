using Newtonsoft.Json;

namespace IdentityEmailApp.DTOs.FinanceDtos
{
    public class FinanceCreditRateAllDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("result")]
        public List<FinanceCreditRateResultDto> Result { get; set; }
            = new();
    }

    public class FinanceCreditRateResultDto
    {
        [JsonProperty("bank-code")]
        public string BankCode { get; set; }

        [JsonProperty("status")]
        public string CreditName { get; set; }

        [JsonProperty("oran")]
        public string InterestRate { get; set; }

        [JsonProperty("tl")]
        public string TotalPayment { get; set; }

        [JsonProperty("ay")]
        public string MonthlyPayment { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}