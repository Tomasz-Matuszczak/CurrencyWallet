using CurrencyWallet.Interfaces;
using CurrencyWallet.Models;
using CurrencyWallet.Services;
using Moq;

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
        var user = new UserModel { Name = "John", Email = "john@example.com" };

        // Act
        _userRepository.AddUser(user);

        // Assert
        var users = _userRepository.GetAllUsers();
        Assert.AreEqual(1, users.Count());
        Assert.AreEqual("John", users.First().Name);
        Assert.AreEqual("john@example.com", users.First().Email);
    }

    [TestMethod]
    public void GetAllUsers_ReturnsAllUsers()
    {
        // Arrange
        var user1 = new UserModel { Name = "John", Email = "john@example.com" };
        var user2 = new UserModel { Name = "Jane", Email = "jane@example.com" };
        _userRepository.AddUser(user1);
        _userRepository.AddUser(user2);

        // Act
        var users = _userRepository.GetAllUsers();

        // Assert
        Assert.AreEqual(2, users.Count());
        CollectionAssert.Contains(users.ToList(), new User(1, "John", "john@example.com"));
        CollectionAssert.Contains(users.ToList(), new User(2, "Jane", "jane@example.com"));
    }

    [TestMethod]
    public void GetUserById_ReturnsUserWithMatchingId()
    {
        // Arrange
        var user1 = new UserModel { Name = "John", Email = "john@example.com" };
        var user2 = new UserModel { Name = "Jane", Email = "jane@example.com" };
        _userRepository.AddUser(user1);
        _userRepository.AddUser(user2);

        // Act
        var user = _userRepository.GetUserById(2);

        // Assert
        Assert.IsNotNull(user);
        Assert.AreEqual(2, user.Id);
        Assert.AreEqual("Jane", user.Name);
        Assert.AreEqual("jane@example.com", user.Email);
    }

    [TestMethod]
    public void GetUserById_ReturnsNullForNonExistingId()
    {
        // Arrange
        var user = new UserModel { Name = "John", Email = "john@example.com" };
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
        var user1 = new UserModel { Name = "John", Email = "john@example.com" };
        var user2 = new UserModel { Name = "Jane", Email = "jane@example.com" };
        _userRepository.AddUser(user1);
        _userRepository.AddUser(user2);

        // Act
        var user = _userRepository.GetUserByName("jane");

        // Assert
        Assert.IsNotNull(user);
        Assert.AreEqual(2, user.Id);
        Assert.AreEqual("Jane", user.Name);
        Assert.AreEqual("jane@example.com", user.Email);
    }

    [TestMethod]
    public void GetUserByName_ReturnsNullForNonExistingName()
    {
        // Arrange
        var user = new UserModel { Name = "John", Email = "john@example.com" };
        _userRepository.AddUser(user);

        // Act
        var result = _userRepository.GetUserByName("Jane");

        // Assert
        Assert.IsNull(result);
    }
}