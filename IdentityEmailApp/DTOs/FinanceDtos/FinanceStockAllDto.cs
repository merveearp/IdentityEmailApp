using Newtonsoft.Json;

namespace IdentityEmailApp.DTOs.FinanceDtos
{
    public class FinanceStockAllDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("result")]
        public List<FinanceStockResultDto> Result { get; set; }
            = new();
    }

    public class FinanceStockResultDto
    {
        [JsonProperty("rate")]
        public decimal Rate { get; set; }

        [JsonProperty("lastprice")]
        public decimal LastPrice { get; set; }

        [JsonProperty("lastpricestr")]
        public string LastPriceStr { get; set; }

        [JsonProperty("hacim")]
        public decimal Hacim { get; set; }  
        
        [JsonProperty("hacimstr")]
        public string HacimStr { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }
    }
}