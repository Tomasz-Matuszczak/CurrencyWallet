using CurrencyWallet.Interfaces;
using CurrencyWallet.Models;
using CurrencyWallet.Services;
using Moq;

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
        var currencyCode = "USD";
        var amount = 100m;
        var user = new User(userId, "John", "john@example.com");
        _userRepositoryMock.Setup(r => r.GetUserById(userId)).Returns(user);
        _currencyRateProviderMock.Setup(p => p.GetCurrencyRatesAsync()).ReturnsAsync(new List<CurrencyRate>
        {
            new() { Currency = "TestName", Code = "USD", Mid = 3.8m }
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
        var currencyCode = "XYZ";
        var amount = 100m;
        var user = new User(userId, "John", "john@example.com");
        _userRepositoryMock.Setup(r => r.GetUserById(userId)).Returns(user);
        _currencyRateProviderMock.Setup(p => p.GetCurrencyRatesAsync()).ReturnsAsync(new List<CurrencyRate>
        {
            new() {Currency = "TestName",  Code = "USD", Mid = 3.8m }
        });

        // Act
        _walletServices.AddMoneyToWallet(userId, currencyCode, amount);
    }

    [TestMethod]
    public void WithdrawMoneyFromWallet_SufficientFunds_WithdrawsMoney()
    {
        // Arrange
        var userId = 1;
        var currencyCode = "USD";
        var amount = 50m;
        var user = new User(userId, "John", "john@example.com");
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
        var currencyCode = "USD";
        var amount = 150m;
        var user = new User(userId, "John", "john@example.com");
        user.Wallet[currencyCode] = 100m;
        _userRepositoryMock.Setup(r => r.GetUserById(userId)).Returns(user);

        // Act
        _walletServices.WithdrawMoneyFromWallet(userId, currencyCode, amount);
    }

    [TestMethod]
    public void ExchangeCurrency_ValidCurrencies_ExchangesMoney()
    {
        // Arrange
        var userId = 1;
        var fromCurrency = "USD";
        var toCurrency = "EUR";
        var amount = 100m;
        var user = new User(userId, "John", "john@example.com");
        user.Wallet[fromCurrency] = 200m;
        _userRepositoryMock.Setup(r => r.GetUserById(userId)).Returns(user);
        _currencyRateProviderMock.Setup(p => p.GetCurrencyRatesAsync()).ReturnsAsync(new List<CurrencyRate>
        {
            new() { Currency = "TestName",  Code = "USD", Mid = 3.8m },
            new() { Currency = "TestName", Code = "EUR", Mid = 4.5m }
        });

        // Act
        _walletServices.ExchangeCurrency(userId, fromCurrency, toCurrency, amount);

        // Assert
        Assert.AreEqual(100m, user.Wallet[fromCurrency]);
        Assert.AreEqual(84.21m, user.Wallet[toCurrency]);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ExchangeCurrency_InsufficientFunds_ThrowsException()
    {
        // Arrange
        var userId = 1;
        var fromCurrency = "USD";
        var toCurrency = "EUR";
        var amount = 300m;
        var user = new User(userId, "John", "john@example.com");
        user.Wallet[fromCurrency] = 200m;
        _userRepositoryMock.Setup(r => r.GetUserById(userId)).Returns(user);
        _currencyRateProviderMock.Setup(p => p.GetCurrencyRatesAsync()).ReturnsAsync(new List<CurrencyRate>
        {
            new() { Currency = "TestName",  Code = "USD", Mid = 3.8m },
            new() { Currency = "TestName", Code = "EUR", Mid = 4.5m }
        });

        // Act
        _walletServices.ExchangeCurrency(userId, fromCurrency, toCurrency, amount);
    }
}