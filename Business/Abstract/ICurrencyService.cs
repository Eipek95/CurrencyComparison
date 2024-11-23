using Business.Dtos;

namespace Business.Abstract
{
    public interface ICurrencyService
    {
        Task<CurrencyComparisonResultDto> GetCurrencyRatesAsync(string currencyCode, string date);
    }
}
