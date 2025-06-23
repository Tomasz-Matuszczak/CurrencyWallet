using CurrencyWallet.Interfaces;
using CurrencyWallet.Models;

namespace CurrencyWallet.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users = [];
        private readonly ICurrencyRateProvider _currencyRateProvider;
        private int _nextUser = 0;

        public UserRepository(ICurrencyRateProvider currencyRateProvider)
        {
            _currencyRateProvider = currencyRateProvider;
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
    }
}