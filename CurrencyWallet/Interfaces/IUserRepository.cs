using CurrencyWallet.Models;

namespace CurrencyWallet.Interfaces
{
    public interface IUserRepository
    {
        void AddUser(UserModel user);
        IEnumerable<User> GetAllUsers();
        User GetUserByName(string name);
        User GetUserById(int id);
    }
}
