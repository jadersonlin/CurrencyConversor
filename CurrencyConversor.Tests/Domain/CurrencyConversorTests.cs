﻿using CurrencyConversor.Domain.Interfaces;
using CurrencyConversor.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using CurrencyConversor.Domain.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace CurrencyConversor.Tests.Domain
{
    public class CurrencyConversorTests
    {
        private readonly Mock<IExternalCurrenciesService> externalCurrenciesServiceMock;
        private readonly ILogger<CurrencyConversionService> logger;

        public CurrencyConversorTests()
        {
            ILoggerFactory nullLoggerFactory = new NullLoggerFactory();
            logger = nullLoggerFactory.CreateLogger<CurrencyConversionService>();
            externalCurrenciesServiceMock = new Mock<IExternalCurrenciesService>();
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
            var userId = Guid.NewGuid().ToString();

            var conversionData = new ExternalConversionData
            {
                FromCurrency = fromCurrency,
                ToCurrency = toCurrency,
                ConversionTimestamp = new DateTime(2021, 08, 03).ToFileTime(),
                ConversionRate = conversionRate
            };

            externalCurrenciesServiceMock.Setup(_ => _.GetConversionRate(fromCurrency, toCurrency)).ReturnsAsync(conversionData);

            var dollar = new Currency(fromCurrency, "US Dollars");
            var reais = new Currency(toCurrency, "Brazilian Reais");

            //Act
            var currencyConversionService = new CurrencyConversionService(externalCurrenciesServiceMock.Object, logger);
            var result = await currencyConversionService.Convert(dollar, reais, fromValue, userId);

            //Assert
            Assert.IsAssignableFrom<SuccessTransaction>(result);

            var successTransaction = (SuccessTransaction)result;

            Assert.Equal(fromValue * conversionRate, successTransaction.ConversionValue);
            Assert.Equal(conversionRate, successTransaction.ConversionRate);
            Assert.Equal(ConversionStatus.ConversionDone, successTransaction.ConversionStatus);
            Assert.NotNull(successTransaction.Id);
            Assert.NotNull(successTransaction.ConversionRate);
            Assert.NotNull(successTransaction.ConversionValue);
            Assert.NotNull(successTransaction.ConversionTimestamp);
            Assert.NotNull(successTransaction.FromCurrency);
            Assert.NotNull(successTransaction.ToCurrency);
            Assert.NotNull(successTransaction.UserId);
        }

        [Fact]
        public async Task Try_convert_with_error_on_external_service()
        {
            //Arrange
            const decimal fromValue = (decimal)12353.23;
            const string fromCurrency = "USD";
            const string toCurrency = "BRL";
            var userId = Guid.NewGuid().ToString();

            externalCurrenciesServiceMock.Setup(_ => _.GetConversionRate(fromCurrency, toCurrency)).ThrowsAsync(new TimeoutException());

            var dollar = new Currency(fromCurrency, "US Dollars");
            var reais = new Currency(toCurrency, "Brazilian Reais");

            //Act
            var currencyConversionService = new CurrencyConversionService(externalCurrenciesServiceMock.Object, logger);
            var result = await currencyConversionService.Convert(dollar, reais, fromValue, userId);

            //Assert
            Assert.IsAssignableFrom<FailureTransaction>(result);
            var failureTransaction = (FailureTransaction)result;
            Assert.NotNull(failureTransaction.Id);
            Assert.NotNull(failureTransaction.ErrorMessage);
        }
    }
}
