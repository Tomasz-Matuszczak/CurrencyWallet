using CurrencyWallet.Interfaces;
using CurrencyWallet.Models;

namespace CurrencyWallet.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users = [];
        private readonly ICurrencyRateProvider _currencyRateProvider;
        private int _nextUser = 1;

        public UserRepository(ICurrencyRateProvider currencyRateProvider)
        {
            _currencyRateProvider = currencyRateProvider;
        }

        public void AddMoneyToWallet(int userId, string currencyCode, decimal amount)
        {
            var user = GetUserById(userId);
            if (user != null)
            {
                var currencyRates = _currencyRateProvider.GetCurrencyRatesAsync().Result;
                var availableCurrencies = currencyRates.Select(r => r.Code).ToList();
                if (availableCurrencies.Contains(currencyCode) || currencyCode == "PLN")
                {
                    //user.Wallet = [];

                    if (user.Wallet.ContainsKey(currencyCode))
                    {
                        user.Wallet[currencyCode] += amount;
                    }
                    else
                        user.Wallet[currencyCode] = amount;
                }
                else
                    throw new InvalidOperationException("Invalid currency code. The currency must exit in NBP Table B.");
            }
            else
                throw new InvalidOperationException("User not found.");
        }

        public void AddUser(UserModel user)
        {
            _users.Add(new User(_nextUser++, user.Name, user.Email));
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _users;
        }

        public User GetUserById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public User GetUserByName(string name)
        {
            return _users.FirstOrDefault(u => u.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        public void WithdrawMoneyFromWallet(int userId, string currencyCode, decimal amount)
        {
            var user = GetUserById(userId);
            if (user != null && user.Wallet != null && user.Wallet.ContainsKey(currencyCode))
            {
                if (user.Wallet[currencyCode] >= amount)
                    user.Wallet[currencyCode] -= amount;
                else
                    throw new InvalidOperationException("Not enough money in this wallet.");
            }
            else
                new InvalidOperationException("User or currency not found in this wallet.");
        }

        public void ExchangeCurrency(int userId, string fromCurrency, string toCurrency, decimal amount)
        {
            var user = GetUserById(userId);
            if (user != null && user.Wallet != null && user.Wallet.ContainsKey(fromCurrency))
            {
                if (user.Wallet[fromCurrency] >= amount)
                {
                    var plnAmount = amount;
                    if (fromCurrency != "PLN")
                        plnAmount = ConvertToPln(fromCurrency, amount);

                    var convertedAmount = plnAmount;
                    if (toCurrency != "PLN")
                        convertedAmount = ConvertFromPln(toCurrency, plnAmount);

                    user.Wallet[fromCurrency] -= amount;
                    if (user.Wallet.ContainsKey(toCurrency))
                        user.Wallet[toCurrency] += convertedAmount;
                    else
                        user.Wallet[toCurrency] = convertedAmount;
                }
                else
                {
                    throw new InvalidOperationException("Insufficient funds in the source currency wallet.");
                }
            }
            else
                throw new InvalidOperationException("User or source currency not found in the wallet.");
        }

        private decimal ConvertToPln(string currency, decimal amount)
        {
            var currencyRates = _currencyRateProvider.GetCurrencyRatesAsync().Result;
            var currencyRate = currencyRates.FirstOrDefault(r => r.Code == currency);

            if (currencyRate != null)
            {
                return Math.Round(amount * currencyRate.Mid, 2);
            }

            throw new InvalidOperationException("Currency not fund in exchange rates.");
        }

        private decimal ConvertFromPln(string currency, decimal plnAmount)
        {
            var currencyRates = _currencyRateProvider.GetCurrencyRatesAsync().Result;
            var currencyRate = currencyRates.FirstOrDefault(r => r.Code == currency);

            if (currencyRate != null)
            {
                return Math.Round(plnAmount / currencyRate.Mid, 2);
            }

            throw new InvalidOperationException("Currency not fund in exchange rates.");
        }
    }
}