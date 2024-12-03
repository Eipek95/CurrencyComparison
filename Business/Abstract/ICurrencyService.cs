using Business.Dtos;
using SharedLibrary.DTOs;

namespace Business.Abstract
{
    public interface ICurrencyService
    {
        Task<Response<CurrencyComparisonResultDto>> GetCurrencyRatesAsync(string currencyCode, string date);
    }
}
