namespace IdentityEmailApp.DTOs.FinanceDtos
{
    public class FinanceStockRadarDto
    {
        public FinanceStockResultDto TopGainer { get; set; }

        public FinanceStockResultDto TopLoser { get; set; }

        public FinanceStockResultDto HighestVolume { get; set; }
    }
}