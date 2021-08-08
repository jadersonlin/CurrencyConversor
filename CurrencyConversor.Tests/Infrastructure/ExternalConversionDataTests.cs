using CurrencyConversor.Domain.Models;
using Xunit;

namespace CurrencyConversor.Tests.Infrastructure
{
    public class ExternalConversionDataTests
    {

        [Fact]
        public void Can_calculate_conversion()
        {
            //Arrange and Act
            const decimal baseFromConversionRate = (decimal)5.233504;
            const decimal baseToConversionRate = (decimal)0.850138;

            var externalConversionData = new ExternalConversionData
            {
                FromCurrency = "EUR",
                ToCurrency = "BRL",
                BaseFromConversionRate = baseFromConversionRate,
                BaseToConversionRate = baseToConversionRate,
                BaseCurrency = "USD"
            };

            //Assert
            Assert.Equal("USD", externalConversionData.BaseCurrency);
            Assert.Equal(baseToConversionRate / baseFromConversionRate, externalConversionData.ConversionRate);
        }
    }
}
