namespace CurrencyWallet.Interfaces
{
    public interface IWalletServices
    {
        void AddMoneyToWallet(int userId, string currencyCode, decimal amount);
        void WithdrawMoneyFromWallet(int userId, string currencyCode, decimal amount);
        public void ExchangeCurrency(int userId, string fromCurrency, string toCurrency, decimal amount);
    }
}
