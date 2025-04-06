using CurrencyWallet.Interfaces;
using CurrencyWallet.Models;
using CurrencyWallet.Services;
using Moq;

namespace CurrencyWallet.Services.Tests
{
    [TestClass]
    public class UserRepositoryTests
    {
        private Mock<ICurrencyRateProvider> _currencyRateProviderMock;
        private UserRepository _userRepository;

        [TestInitialize]
        public void Initialize()
        {
            _currencyRateProviderMock = new Mock<ICurrencyRateProvider>();
            _userRepository = new UserRepository(_currencyRateProviderMock.Object);
        }

        [TestMethod]
        public void AddUser_AddsUserToRepository()
        {
            // Arrange
            var user = new UserModel { Name = "Tom", Email = "Tom@test.com" };

            // Act
            _userRepository.AddUser(user);

            // Assert
            var users = _userRepository.GetAllUsers();
            Assert.AreEqual(1, users.Count());
            Assert.AreEqual("Tom", users.First().Name);
            Assert.AreEqual("Tom@test.com", users.First().Email);
        }

        [TestMethod]
        public void GetAllUsers_ReturnsAllUsers()
        {
            // Arrange
            var user1 = new UserModel { Name = "Tom", Email = "Tom@test.com" };
            var user2 = new UserModel { Name = "Ania", Email = "Ania@test.com" };
            _userRepository.AddUser(user1);
            _userRepository.AddUser(user2);

            // Act
            var users = _userRepository.GetAllUsers().ToList();

            // Assert
            Assert.AreEqual(2, users.Count());

            Assert.AreEqual(users[0].Id, 0);
            Assert.AreEqual(users[0].Name, "Tom");
            Assert.AreEqual(users[0].Email, "Tom@test.com");
            Assert.IsTrue(users[0].Wallet.ContainsKey("PLN"));
            Assert.AreEqual(users[1].Id, 1);
            Assert.AreEqual(users[1].Name, "Ania");
            Assert.AreEqual(users[1].Email, "Ania@test.com");
            Assert.IsTrue(users[1].Wallet.ContainsKey("PLN"));
        }

        [TestMethod]
        public void GetUserById_ReturnsUserWithMatchingId()
        {
            // Arrange
            var user1 = new UserModel { Name = "Tom", Email = "Tom@test.com" };
            var user2 = new UserModel { Name = "Ania", Email = "Ania@test.com" };
            _userRepository.AddUser(user1);
            _userRepository.AddUser(user2);

            // Act
            var user = _userRepository.GetUserById(1);

            // Assert
            Assert.IsNotNull(user);
            Assert.AreEqual(1, user.Id);
            Assert.AreEqual("Ania", user.Name);
            Assert.AreEqual("Ania@test.com", user.Email);
        }

        [TestMethod]
        public void GetUserById_ReturnsNullForNonExistingId()
        {
            // Arrange
            var user = new UserModel { Name = "Tom", Email = "Tom@test.com" };
            _userRepository.AddUser(user);

            // Act
            var result = _userRepository.GetUserById(2);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetUserByName_ReturnsUserWithMatchingName()
        {
            // Arrange
            var user1 = new UserModel { Name = "Tom", Email = "Tom@test.com" };
            var user2 = new UserModel { Name = "Ania", Email = "Ania@test.com" };
            _userRepository.AddUser(user1);
            _userRepository.AddUser(user2);

            // Act
            var user = _userRepository.GetUserByName("Ania");

            // Assert
            Assert.IsNotNull(user);
            Assert.AreEqual(1, user.Id);
            Assert.AreEqual("Ania", user.Name);
            Assert.AreEqual("Ania@test.com", user.Email);
        }

        [TestMethod]
        public void GetUserByName_ReturnsNullForNonExistingName()
        {
            // Arrange
            var user = new UserModel { Name = "Tom", Email = "Tom@test.com" };
            _userRepository.AddUser(user);

            // Act
            var result = _userRepository.GetUserByName("Ania");

            // Assert
            Assert.IsNull(result);
        }
    }
}