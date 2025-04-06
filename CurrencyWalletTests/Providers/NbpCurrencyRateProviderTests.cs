using CurrencyWallet.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;

namespace CurrencyWallet.Providers.Tests
{
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
            _configurationMock.Setup(c => c["nbpTableBUrl"]).Returns("https://api.nbp.pl/api/exchangerates/tables/b");

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _currencyRateProvider = new NbpCurrencyRateProvider(httpClient, _configurationMock.Object);
        }

        [TestMethod]
        public async Task GetCurrencyRatesAsync_ReturnsRatesFromApi()
        {
            // Arrange
            var expectedRates = new List<CurrencyRate>
        {
            new() { Currency = "TestName1", Code = "CRC", Mid = 3.8m },
            new() { Currency = "TestName2", Code = "JMD", Mid = 4.5m }
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

            var expectedCurrency1 = actualRates.FirstOrDefault(c => c.Code == "CRC");
            Assert.AreEqual(expectedRates[0].Code, expectedCurrency1.Code);
            Assert.AreEqual(expectedRates[0].Currency, expectedCurrency1.Currency);
            Assert.AreEqual(expectedRates[0].Mid, expectedCurrency1.Mid);
            CollectionAssert.Contains(actualRates.ToList(), expectedRates[0]);

            var expectedCurrency2 = actualRates.FirstOrDefault(c => c.Code == "JMD");
            Assert.AreEqual(expectedRates[1].Code, expectedCurrency2.Code);
            Assert.AreEqual(expectedRates[1].Currency, expectedCurrency2.Currency);
            Assert.AreEqual(expectedRates[1].Mid, expectedCurrency2.Mid);
            CollectionAssert.Contains(actualRates.ToList(), expectedRates[1]);
        }
    }
}