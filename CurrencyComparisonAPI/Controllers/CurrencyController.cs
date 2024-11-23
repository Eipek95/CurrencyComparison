using Business.Abstract;
using Business.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyComparisonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;
        private readonly ILogService _logService;

        public CurrencyController(ICurrencyService currencyService, ILogService logService)
        {
            _currencyService = currencyService;
            _logService = logService;
        }

        [HttpGet("{currency}/{date}")]
        public async Task<IActionResult> GetCurrencyComparison(string currency, string date)
        {
            try
            {
                var rate = await _currencyService.GetCurrencyRatesAsync(currency, date);

                if (rate == null)
                    return NotFound(new { Message = "Currency data not found.", Currency = currency });

                // Fark ve yüzdesel değişimi hesapla
                decimal difference = rate.Difference;
                decimal percentageChange = rate.Difference;

                // Sonuçları döndür
                return Ok(new CurrencyComparisonResultDto
                {
                    Currency = currency,
                    CurrentRate = rate.CurrentRate,
                    OldRate = rate.OldRate,
                    Difference = difference,
                    PercentageChange = percentageChange
                });
            }
            catch (Exception ex)
            {
                // Hataları kaydet
                await _logService.AddLogAsync(new CreateLogDtoModel
                {
                    TransactionDate = DateTime.Now,
                    UserIP = HttpContext.Connection.LocalIpAddress?.ToString(),
                    Currency = currency,
                    ErrorFunction = nameof(GetCurrencyComparison),
                    ErrorDescription = ex.Message
                });

                // Sunucu hatası döndür
                return StatusCode(500, new
                {
                    Message = "An error occurred while processing your request.",
                    Error = ex.Message
                });
            }
        }


    }
}
