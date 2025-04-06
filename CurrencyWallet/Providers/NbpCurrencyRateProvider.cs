using CurrencyWallet.Interfaces;
using CurrencyWallet.Models;
using Newtonsoft.Json;

namespace CurrencyWallet.Providers
{
    public class NbpCurrencyRateProvider : ICurrencyRateProvider
    {
        //https://nbp.pl/statystyka-i-sprawozdawczosc/kursy/informacja-o-terminach-publikacji-kursow-walut/
        //Tabela B kursów średnich walut obcych aktualizowana jest w każdą środę, między godziną 11:45 a 12:15.
        //Jeżeli środa jest dniem wolnym od pracy, aktualizacja odbywa się poprzedniego dnia.

        private readonly string _apiUrl;
        private List<CurrencyRate> _currencyRates;
        private DateTime _lastUpdateDate;
        private readonly HttpClient _httpClient;

        public NbpCurrencyRateProvider(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiUrl = configuration["nbpTableBUrl"];
            _currencyRates = new List<CurrencyRate>();
            _lastUpdateDate = DateTime.MinValue;
        }

        public async Task<IEnumerable<CurrencyRate>> GetCurrencyRatesAsync()
        {
            if (ShouldRefreshData())
            {
                await RefreshCurrencyRatesAsync();
            }

            return _currencyRates;
        }
        private async Task RefreshCurrencyRatesAsync()
        {
            try
            {
                
                var response = await _httpClient.GetAsync(_apiUrl);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var currencyRate = JsonConvert.DeserializeObject<List<NbpResponse>>(responseBody);

                if (currencyRate != null && currencyRate.Count > 0)
                {
                    _currencyRates = currencyRate[0].Rates;
                    _lastUpdateDate = DateTime.Now.Date;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool ShouldRefreshData()
        {
            var currentDate = DateTime.Now.Date;
            var currentDayOfTheWeek = currentDate.DayOfWeek;

            if ((currentDayOfTheWeek == DayOfWeek.Wednesday && _lastUpdateDate != currentDate) || _currencyRates.Count() == 0)
            {
                var refreshTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 12, 16, 0);
                return DateTime.Now > refreshTime;

            }

            return false;
        }
    }
}
