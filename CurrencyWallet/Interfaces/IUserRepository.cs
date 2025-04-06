using CurrencyWallet.Models;

namespace CurrencyWallet.Interfaces
{
    public interface IUserRepository
    {
        void AddUser(UserModel user);
        IEnumerable<User> GetAllUsers();
        User GetUserByName(string name);
        User GetUserById(int id);


        void AddMoneyToWallet(int userId, string currencyCode, decimal amount);
        void WithdrawMoneyFromWallet(int userId, string currencyCode, decimal amount);
        public void ExchangeCurrency(int userId, string fromCurrency, string toCurrency, decimal amount);
    }
}
