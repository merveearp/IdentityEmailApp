namespace IdentityEmailApp.DTOs.FinanceDtos
{
    public class FinanceStockOverviewDto
    {
        public List<FinanceStockResultDto>  FeaturedStocks { get; set; } 
        public FinanceStockRadarDto Radar { get; set; } 
    }
}