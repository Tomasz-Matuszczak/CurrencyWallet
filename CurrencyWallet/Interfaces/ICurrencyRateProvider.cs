using CurrencyWallet.Models;

namespace CurrencyWallet.Interfaces
{
    public interface ICurrencyRateProvider
    {
        Task<IEnumerable<CurrencyRate>> GetCurrencyRatesAsync();
    }
}
