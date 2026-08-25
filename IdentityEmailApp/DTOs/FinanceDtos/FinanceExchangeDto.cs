using Newtonsoft.Json;

namespace IdentityEmailApp.Dtos.FinanceDtos
{
    public class FinanceExchangeDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("result")]
        public List<FinanceExchangeResultDto> Result { get; set; }
            = new List<FinanceExchangeResultDto>();
    }

    public class FinanceExchangeResultDto
    {
        [JsonProperty("base")]
        public string Base { get; set; }

        [JsonProperty("lastupdate")]
        public string LastUpdate { get; set; }

        [JsonProperty("data")]
        public List<FinanceExchangeDataDto> Data { get; set; }
            = new List<FinanceExchangeDataDto>();
    }

    public class FinanceExchangeDataDto
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("rate")]
        public string Rate { get; set; }

        [JsonProperty("calculatedstr")]
        public string CalculatedStr { get; set; }

        [JsonProperty("calculated")]
        public decimal Calculated { get; set; }
    }
}