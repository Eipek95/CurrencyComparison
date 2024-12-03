using Business.Abstract;
using Business.Dtos;
using SharedLibrary.DTOs;
using System.Globalization;
using System.Net;
using System.Xml.Linq;

namespace Business.Concrete
{
    public class CurrencyService : ICurrencyService
    {
        public async Task<Response<CurrencyComparisonResultDto>> GetCurrencyRatesAsync(string currencyCode, string date)
        {
            if (!DateTime.TryParseExact(date, "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return CreateErrorResponse("Geçersiz tarih formatı. Tarih ddMMyyyy formatında olmalıdır");
            }

            string currentCurrencyUrl = "https://www.tcmb.gov.tr/kurlar/today.xml";
            string oldCurrencyUrl = $"https://www.tcmb.gov.tr/kurlar/{date.Substring(4, 4)}{date.Substring(2, 2)}/{date}.xml";

            try
            {
                using var client = new HttpClient();


                // Bugünkü kuru al
                var currentResponse = await client.GetStringAsync(currentCurrencyUrl);
                var currentRate = GetCurrencyRateFromXml(currentResponse, currencyCode);


                // Seçilen tarihteki kuru al
                var oldResponse = await client.GetStringAsync(oldCurrencyUrl);
                var oldRate = GetCurrencyRateFromXml(oldResponse, currencyCode);

                if (currentRate.HasValue && oldRate.HasValue)
                {
                    // Fark ve yüzdesel değişim hesaplama
                    var difference = currentRate.Value - oldRate.Value;
                    var percentageChange = (difference / oldRate.Value) * 100;


                    return Response<CurrencyComparisonResultDto>.Success(new CurrencyComparisonResultDto
                    {
                        Currency = currencyCode,
                        CurrentRate = currentRate.Value,
                        OldRate = oldRate.Value,
                        Difference = difference,
                        PercentageChange = percentageChange
                    }, Convert.ToInt16(HttpStatusCode.OK));

                }
                return CreateErrorResponse("Döviz kurları alınamadı.");
            }
            catch (HttpRequestException httpEx)
            {
                return CreateErrorResponse("HTTP isteği sırasında bir hata oluştu.");
            }
            catch (Exception ex)
            {
                return CreateErrorResponse($"Kur bilgisi karşılaştırılırken bir hata oluştu: {ex.Message}");
            }
        }
        private decimal? GetCurrencyRateFromXml(string xmlContent, string currencyCode)
        {
            var xDoc = XDocument.Parse(xmlContent);

            var currency = xDoc.Descendants("Currency")
                               .FirstOrDefault(x => x.Attribute("Kod")?.Value == currencyCode);

            if (currency != null)
            {
                if (decimal.TryParse(currency.Element("ForexBuying")?.Value.Replace(".", ","), out var rate))
                {
                    return rate;
                }
            }

            return null;
        }

        private Response<CurrencyComparisonResultDto> CreateErrorResponse(string errorMessage)
        {
            var errorDto = new ErrorDto(errorMessage, true);
            return Response<CurrencyComparisonResultDto>.Fail(errorDto, Convert.ToInt16(HttpStatusCode.BadRequest));
        }
    }
}
