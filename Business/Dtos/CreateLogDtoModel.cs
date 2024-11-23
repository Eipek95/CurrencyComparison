namespace Business.Dtos
{
    public class CreateLogDtoModel
    {

        public DateTime TransactionDate { get; set; }
        public string UserIP { get; set; }
        public string Currency { get; set; }
        public double PercentageChange { get; set; }
        public string ErrorFunction { get; set; }
        public string ErrorDescription { get; set; }
    }
}
