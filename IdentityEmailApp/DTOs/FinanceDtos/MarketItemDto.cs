namespace IdentityEmailApp.Dtos.FinanceDtos
{
    public class MarketItemDto
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal ChangeRate { get; set; }
        public string Category { get; set; }
        public string Currency { get; set; }
    }
}