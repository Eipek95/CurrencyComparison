using Business.Abstract;
using Business.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyComparisonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CurrencyController : CustomBaseController
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
            var result = await _currencyService.GetCurrencyRatesAsync(currency.ToUpper(), date);

            if (result.Error != null)
            {
                await _logService.AddLogAsync(new CreateLogDtoModel
                {
                    TransactionDate = DateTime.Now,
                    UserIP = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Currency = currency,
                    ErrorFunction = nameof(GetCurrencyComparison),
                    ErrorDescription = result.Error.Errors.FirstOrDefault()
                });
            }
            return ActionResultInstance(result);
        }
    }
}
