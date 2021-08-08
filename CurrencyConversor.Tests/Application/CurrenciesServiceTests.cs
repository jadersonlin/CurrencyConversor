using CurrencyConversor.Application.Dtos;
using CurrencyConversor.Application.Impl;
using CurrencyConversor.Domain.Models;
using CurrencyConversor.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CurrencyConversor.Tests.Application
{
    public class CurrenciesServiceTests
    {
        private readonly Mock<ICurrencyRepository> currencyRepositoryMock;

        public CurrenciesServiceTests()
        {
            currencyRepositoryMock = new Mock<ICurrencyRepository>();
        }

        [Fact]
        public async Task Can_get_available_currencies()
        {
            var currencies = new List<Currency>
            {
                new Currency("BRL", "Real"),
                new Currency("USD", "Dollar"),
                new Currency("JPY", "Yene"),
                new Currency("EUR", "Euro")
            };

            //Arrange
            currencyRepositoryMock.Setup(_ => _.GetAll()).ReturnsAsync(currencies);

            var currenciesService = new CurrenciesService(currencyRepositoryMock.Object);

            var result = await currenciesService.GetAvailableCurrencies();

            Assert.IsAssignableFrom<GetAllCurrenciesResult>(result);
            Assert.IsAssignableFrom<IList<CurrencyDto>>(result.Currencies);
            Assert.Equal(4, result.Currencies.Count);
        }

        [Fact]
        public async Task Try_get_available_currencies_error()
        {
            //Arrange
            currencyRepositoryMock.Setup(_ => _.GetAll()).ThrowsAsync(new TimeoutException());

            //Act and Assert
            var currenciesService = new CurrenciesService(currencyRepositoryMock.Object);
            await Assert.ThrowsAsync<TimeoutException>(() => currenciesService.GetAvailableCurrencies());
        }
    }
}
