using CurrencyWallet.Interfaces;
using CurrencyWallet.Models;
using CurrencyWallet.Services;
using Moq;

namespace CurrencyWallet.Services.Tests
{
    [TestClass]
    public class WalletServicesTests
    {
        private Mock<ICurrencyRateProvider> _currencyRateProviderMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private WalletServices _walletServices;

        [TestInitialize]
        public void Initialize()
        {
            _currencyRateProviderMock = new Mock<ICurrencyRateProvider>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _walletServices = new WalletServices(_currencyRateProviderMock.Object, _userRepositoryMock.Object);
        }

        [TestMethod]
        public void AddMoneyToWallet_ValidCurrency_AddsMoneyToWallet()
        {
            // Arrange
            var userId = 1;
            var currencyCode = "PLN";
            var amount = 100m;
            var user = new User(userId, "Tom", "Tom@test.com");
            _userRepositoryMock.Setup(r => r.GetUserById(userId)).Returns(user);
            _currencyRateProviderMock.Setup(p => p.GetCurrencyRatesAsync()).ReturnsAsync(new List<CurrencyRate>
        {
            new() { Currency = "TestName1", Code = "PLN", Mid = 3.8m }
        });

            // Act
            _walletServices.AddMoneyToWallet(userId, currencyCode, amount);

            // Assert
            Assert.AreEqual(100m, user.Wallet[currencyCode]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddMoneyToWallet_InvalidCurrency_ThrowsException()
        {
            // Arrange
            var userId = 1;
            var currencyCode = "JMD";
            var amount = 100m;
            var user = new User(userId, "Tom", "Tom@test.com");
            _userRepositoryMock.Setup(r => r.GetUserById(userId)).Returns(user);
            _currencyRateProviderMock.Setup(p => p.GetCurrencyRatesAsync()).ReturnsAsync(new List<CurrencyRate>
        {
            new() {Currency = "TestName1",  Code = "PLN", Mid = 3.8m }
        });

            // Act
            _walletServices.AddMoneyToWallet(userId, currencyCode, amount);
        }

        [TestMethod]
        public void WithdrawMoneyFromWallet_SufficientFunds_WithdrawsMoney()
        {
            // Arrange
            var userId = 1;
            var currencyCode = "PLN";
            var amount = 50m;
            var user = new User(userId, "Tom", "Tom@test.com");
            user.Wallet[currencyCode] = 100m;
            _userRepositoryMock.Setup(r => r.GetUserById(userId)).Returns(user);

            // Act
            _walletServices.WithdrawMoneyFromWallet(userId, currencyCode, amount);

            // Assert
            Assert.AreEqual(50m, user.Wallet[currencyCode]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WithdrawMoneyFromWallet_InsufficientFunds_ThrowsException()
        {
            // Arrange
            var userId = 1;
            var currencyCode = "PLN";
            var amount = 150m;
            var user = new User(userId, "Tom", "Tom@test.com");
            user.Wallet[currencyCode] = 100m;
            _userRepositoryMock.Setup(r => r.GetUserById(userId)).Returns(user);

            // Act
            _walletServices.WithdrawMoneyFromWallet(userId, currencyCode, amount);
        }

        [TestMethod]
        public void ExchangeCurrency_ValidCurrenciesExchangeFromPLN_ExchangesMoney()
        {
            // Arrange
            var userId = 1;
            var fromCurrency = "PLN";
            var toCurrency = "CRC";
            var amount = 100m;
            var user = new User(userId, "Tom", "Tom@test.com");
            user.Wallet[fromCurrency] = 200m;
            _userRepositoryMock.Setup(r => r.GetUserById(userId)).Returns(user);
            _currencyRateProviderMock.Setup(p => p.GetCurrencyRatesAsync()).ReturnsAsync(
            [
                new() { Currency = "TestName2", Code = "CRC", Mid = 0.007709m }
            ]);

            // Act
            _walletServices.ExchangeCurrency(userId, fromCurrency, toCurrency, amount);

            // Assert
            Assert.AreEqual(100m, user.Wallet[fromCurrency]);
            Assert.AreEqual(12971.85m, user.Wallet[toCurrency]);
        }

        [TestMethod]
        public void ExchangeCurrency_ValidCurrenciesExchangeFromJDMToCRC_ExchangesMoney()
        {
            // Arrange
            var userId = 1;
            var fromCurrency = "JMD";
            var toCurrency = "CRC";
            var amount = 100m;
            var user = new User(userId, "Tom", "Tom@test.com");
            user.Wallet[fromCurrency] = 200m;
            _userRepositoryMock.Setup(r => r.GetUserById(userId)).Returns(user);
            _currencyRateProviderMock.Setup(p => p.GetCurrencyRatesAsync()).ReturnsAsync(
            [
                new() { Currency = "TestName1", Code = "JMD", Mid = 0.0245m },
                new() { Currency = "TestName2", Code = "CRC", Mid = 0.007709m }
            ]);

            // Act
            _walletServices.ExchangeCurrency(userId, fromCurrency, toCurrency, amount);

            // Assert
            Assert.AreEqual(100m, user.Wallet[fromCurrency]);
            Assert.AreEqual(317,81m, user.Wallet[toCurrency]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ExchangeCurrency_InsufficientFunds_ThrowsException()
        {
            // Arrange
            var userId = 1;
            var fromCurrency = "PLN";
            var toCurrency = "CRC";
            var amount = 300m;
            var user = new User(userId, "Tom", "Tom@test.com");
            user.Wallet[fromCurrency] = 200m;
            _userRepositoryMock.Setup(r => r.GetUserById(userId)).Returns(user);
            _currencyRateProviderMock.Setup(p => p.GetCurrencyRatesAsync()).ReturnsAsync(new List<CurrencyRate>
            {
                new() { Currency = "TestName2", Code = "CRC", Mid = 4.5m }
            });

            // Act
            _walletServices.ExchangeCurrency(userId, fromCurrency, toCurrency, amount);
        }
    }
}