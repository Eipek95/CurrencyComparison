namespace Business.Dtos
{
    public class CurrencyComparisonResultDto
    {
        public string Currency { get; set; }
        public decimal CurrentRate { get; set; }
        public decimal OldRate { get; set; }
        public decimal Difference { get; set; }
        public decimal PercentageChange { get; set; }
    }
}
