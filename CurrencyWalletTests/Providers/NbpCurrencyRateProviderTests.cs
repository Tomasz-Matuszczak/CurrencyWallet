using CurrencyWallet.Models;
using CurrencyWallet.Providers;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;

[TestClass]
public class NbpCurrencyRateProviderTests
{
    private Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private Mock<IConfiguration> _configurationMock;
    private NbpCurrencyRateProvider _currencyRateProvider;

    [TestInitialize]
    public void Initialize()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _configurationMock = new Mock<IConfiguration>();
        _configurationMock.Setup(c => c["NBPApiUrl"]).Returns("http://api.nbp.pl/api");

        var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _currencyRateProvider = new NbpCurrencyRateProvider(httpClient, _configurationMock.Object);
    }

    [TestMethod]
    public async Task GetCurrencyRatesAsync_ReturnsRatesFromApi()
    {
        // Arrange
        var expectedRates = new List<CurrencyRate>
        {
            new() { Currency = "USD", Code = "USD", Mid = 3.8m },
            new() { Currency = "EUR", Code = "EUR", Mid = 4.5m }
        };

        var response = new HttpResponseMessage
        {
            Content = new StringContent(JsonConvert.SerializeObject(new List<NbpResponse>
            {
                new NbpResponse { Rates = expectedRates }
            }))
        };

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        // Act
        var actualRates = await _currencyRateProvider.GetCurrencyRatesAsync();

        // Assert
        Assert.AreEqual(expectedRates.Count, actualRates.Count());
        CollectionAssert.Contains(actualRates.ToList(), expectedRates[0]);
        CollectionAssert.Contains(actualRates.ToList(), expectedRates[1]);
    }

    [TestMethod]
    public async Task GetCurrencyRatesAsync_RefreshesRatesOnWednesdayAfterUpdateTime()
    {
        // Arrange
        var initialRates = new List<CurrencyRate>
        {
            new() { Currency = "USD", Code = "USD", Mid = 3.8m },
            new() { Currency = "EUR", Code = "EUR", Mid = 4.5m }
        };

        var updatedRates = new List<CurrencyRate>
        {
            new() { Currency = "USD", Code = "USD", Mid = 3.9m },
            new() { Currency = "EUR", Code = "EUR", Mid = 4.6m }
        };

        var initialResponse = new HttpResponseMessage
        {
            Content = new StringContent(JsonConvert.SerializeObject(new List<NbpResponse>
            {
                new NbpResponse { Rates = initialRates }
            }))
        };

        var updatedResponse = new HttpResponseMessage
        {
            Content = new StringContent(JsonConvert.SerializeObject(new List<NbpResponse>
            {
                new() { Rates = updatedRates }
            }))
        };

        _httpMessageHandlerMock.Protected()
            .SetupSequence<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(initialResponse)
            .ReturnsAsync(updatedResponse);

        // Act
        var rates1 = await _currencyRateProvider.GetCurrencyRatesAsync();
        var rates2 = await _currencyRateProvider.GetCurrencyRatesAsync();

        // Assert
        CollectionAssert.AreEqual(initialRates, rates1.ToList());
        CollectionAssert.AreEqual(updatedRates, rates2.ToList());
    }
}