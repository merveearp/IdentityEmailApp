namespace IdentityEmailApp.Dtos.FinanceDtos
{
    public class MarketChartDto
    {
        public string Symbol { get; set; }
        public List<string> Labels { get; set; } = new();
        public List<decimal> Prices { get; set; } = new();
    }
}