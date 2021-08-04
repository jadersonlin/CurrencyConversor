using CurrencyConversor.Domain.Interfaces;
using CurrencyConversor.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace CurrencyConversor.Tests.Domain
{
    public class CurrencyConversorTests
    {
        public Mock<IExternalCurrenciesService> externalCurrenciesServiceMock;
        public ILogger<CurrencyConversion> logger;

        public CurrencyConversorTests()
        {
            ILoggerFactory nullLoggerFactory = new NullLoggerFactory();
            logger = nullLoggerFactory.CreateLogger<CurrencyConversion>();
            externalCurrenciesServiceMock = new Mock<IExternalCurrenciesService>();
        }


        [Fact]
        public void Can_prepare_data_for_conversion()
        {
            //Arrange
            const decimal fromValue = (decimal)12353.23;
            const string fromCurrency = "USD";
            const string toCurrency = "BRL";

            //Act
            var currencyConversion = new CurrencyConversion(externalCurrenciesServiceMock.Object, logger);
            currencyConversion.PrepareForConversion(new Currency(fromCurrency, "US Dollars"),
                new Currency(toCurrency, "Brazilian Reais"), fromValue);

            //Assert
            Assert.Equal(ConversionStatus.PreparedForConversion, currencyConversion.ConversionStatus); 
            Assert.Equal(fromValue, currencyConversion.FromValue);
            Assert.Equal(fromCurrency, currencyConversion.FromCurrency.Code);
            Assert.Equal(toCurrency, currencyConversion.ToCurrency.Code);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(12353.23)]
        public async Task Can_convert_currency(decimal fromValue)
        {
            //Arrange
            const string fromCurrency = "USD";
            const string toCurrency = "BRL";
            const decimal conversionRate = (decimal)5.213196;

            var conversionData = new ExternalConversionData
            {
                FromCurrency = fromCurrency,
                ToCurrency = toCurrency,
                ConversionTimestamp = new DateTime(2021, 08, 03).ToFileTime(),
                ConversionRate = conversionRate
            };

            externalCurrenciesServiceMock.Setup(_ => _.GetConversionRate(fromCurrency, toCurrency)).ReturnsAsync(conversionData);

            //Act
            var currencyConversion = new CurrencyConversion(externalCurrenciesServiceMock.Object, logger);
            currencyConversion.PrepareForConversion(new Currency(fromCurrency, "US Dollars"),
                new Currency(toCurrency, "Brazilian Reais"), fromValue);

            await currencyConversion.ExecuteConversion();

            //Assert
            Assert.Equal(fromValue * conversionRate, currencyConversion.ConversionValue);
            Assert.Equal(conversionRate, currencyConversion.ConversionRate);
            Assert.Equal(ConversionStatus.ConversionDone, currencyConversion.ConversionStatus);
        }
    }
}
