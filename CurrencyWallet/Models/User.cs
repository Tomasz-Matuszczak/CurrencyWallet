using Newtonsoft.Json;

namespace CurrencyWallet.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Dictionary<string, decimal> Wallet { get; set; }

        public User(int id, string name, string email)
        {
            Id = id; 
            Name = name; 
            Email = email;
            Wallet = new Dictionary<string, decimal>{ { "PLN", 0 } };
        }
    }
}
