using Business.Abstract;
using Business.Dtos;
using System.Xml.Linq;

namespace Business.Concrete
{
    public class CurrencyService : ICurrencyService
    {
        public async Task<CurrencyComparisonResultDto> GetCurrencyRatesAsync(string currencyCode, string date)
        {


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

                    return new CurrencyComparisonResultDto
                    {
                        Currency = currencyCode,
                        CurrentRate = currentRate.Value,
                        OldRate = oldRate.Value,
                        Difference = difference,
                        PercentageChange = percentageChange
                    };
                }

                throw new Exception("Döviz kurları alınamadı.");
            }
            catch (HttpRequestException httpEx)
            {
                throw new Exception("HTTP isteği sırasında bir hata oluştu.", httpEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"Kur bilgisi karşılaştırılırken bir hata oluştu: {ex.Message}", ex);
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
    }
}
