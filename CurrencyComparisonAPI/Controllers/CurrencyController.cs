using Business.Abstract;
using Business.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CurrencyComparisonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

                if (!DateTime.TryParseExact(date, "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    return BadRequest(new { Message = "Geçersiz tarih formatı. Tarih ddMMyyyy formatında olmalıdır." });
                }

                var rate = await _currencyService.GetCurrencyRatesAsync(currency.ToUpper(), date);

                if (rate == null)
                    return NotFound(new { Message = "Para birimi verisi bulunamadı.", Currency = currency });

                // Fark ve yüzdesel değişimi hesapla
                decimal difference = rate.Difference;
                decimal percentageChange = rate.PercentageChange;


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
                    UserIP = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Currency = currency,
                    ErrorFunction = nameof(GetCurrencyComparison),
                    ErrorDescription = ex.Message
                });

                // Sunucu hatası döndür
                return StatusCode(500, new
                {
                    Message = "İsteğiniz işlenirken bir hata oluştu.",
                    Error = ex.Message
                });
            }
        }


    }
}
