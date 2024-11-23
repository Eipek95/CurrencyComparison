namespace Entities
{
    public class CurrencyModel
    {
        public string Currency { get; set; } // USD, EUR, GBP
        public decimal Rate { get; set; }
        public decimal Difference { get; set; }
        public decimal PercentageChange { get; set; }
    }
}

