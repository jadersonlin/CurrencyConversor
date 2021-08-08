using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using CurrencyConversor.API.Controllers;
using CurrencyConversor.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.Threading.Tasks;
using CurrencyConversor.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace CurrencyConversor.Tests.API
{
    public class CurrenciesControllerTests
    {
        private readonly Mock<IConversionTransactionService> conversionTransactionServiceMock;
        private readonly Mock<ICurrenciesService> currenciesServiceMock;
        private readonly CurrenciesController controller;

        public CurrenciesControllerTests()
        {
            conversionTransactionServiceMock = new Mock<IConversionTransactionService>();
            currenciesServiceMock = new Mock<ICurrenciesService>();
            ILoggerFactory nullLoggerFactory = new NullLoggerFactory();
            var logger = nullLoggerFactory.CreateLogger<CurrenciesController>();

            controller = new CurrenciesController(conversionTransactionServiceMock.Object,
                currenciesServiceMock.Object, logger);
        }

        [Fact]
        public async Task Can_GET_all_available_currencies()
        {
            //Arrange
            var expectedResult = new GetAllCurrenciesResult
            {
                Currencies = new List<CurrencyDto>
                {
                    new CurrencyDto
                    {
                        Code = "USD",
                        Name = "Dollar"
                    },
                    new CurrencyDto
                    {
                        Code = "BRL",
                        Name = "Real"
                    },
                    new CurrencyDto
                    {
                        Code = "JPY",
                        Name = "Yene"
                    },
                    new CurrencyDto
                    {
                        Code = "EUR",
                        Name = "Euro"
                    }
                },
                Message = "Success!"
            };

            currenciesServiceMock.Setup(_ => _.GetAvailableCurrencies()).ReturnsAsync(expectedResult);

            //Act
            var result = await controller.Get();


            //Assert
            Assert.IsAssignableFrom<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;

            Assert.IsAssignableFrom<GetAllCurrenciesResult>(okResult.Value);
            var currenciesResult = (GetAllCurrenciesResult)okResult.Value;
            Assert.IsAssignableFrom<IList<CurrencyDto>>(currenciesResult.Currencies);
            Assert.Equal(4, currenciesResult.Currencies.Count);
            Assert.All(currenciesResult.Currencies, dto =>
            {
                Assert.NotNull(dto.Code);
                Assert.NotNull(dto.Name);
            });
        }

        [Fact]
        public async Task Try_GET_all_available_currencies_not_found()
        {
            //Arrange

            currenciesServiceMock.Setup(_ => _.GetAvailableCurrencies()).ReturnsAsync((GetAllCurrenciesResult)null);

            //Act
            var result = await controller.Get();

            //Assert
            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Can_GET_conversion()
        {
            //Arrange
            const string fromCurrency = "USD";
            const string toCurrency = "BRL";
            const decimal fromValue = (decimal)65345106.01;
            var userId = Guid.NewGuid().ToString();

            var expectedResult = new GetConversionResult
            {
                Id = Guid.NewGuid().ToString(),
                FromValue = fromValue,
                FromCurrency = fromCurrency,
                ToCurrency = toCurrency,
                ConversionRate = (decimal)5.32,
                ConversionTimestamp = DateTime.UtcNow,
                UserId = userId,
                Message = "Success!"
            };

            conversionTransactionServiceMock
                .Setup(_ => _.RequestConversion(fromCurrency, toCurrency, fromValue, userId))
                .ReturnsAsync(expectedResult);

            //Act
            var result = await controller.GetConversion(fromValue, fromCurrency, toCurrency, userId);

            //Assert
            Assert.IsAssignableFrom<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;

            Assert.IsAssignableFrom<GetConversionResult>(okResult.Value);
            var conversionResult = (GetConversionResult)okResult.Value;

            Assert.Equal(expectedResult.Id, conversionResult.Id);
            Assert.Equal(expectedResult.FromValue, conversionResult.FromValue);
            Assert.Equal(expectedResult.FromCurrency, conversionResult.FromCurrency);
            Assert.Equal(expectedResult.ToCurrency, conversionResult.ToCurrency);
            Assert.Equal(expectedResult.ConversionRate, conversionResult.ConversionRate);
            Assert.Equal(expectedResult.ConversionValue, conversionResult.ConversionValue);
            Assert.Equal(expectedResult.ConversionTimestamp, conversionResult.ConversionTimestamp);
            Assert.Equal(expectedResult.UserId, conversionResult.UserId);
            Assert.Equal(expectedResult.Message, conversionResult.Message);
        }

        [Fact]
        public async Task Try_GET_conversion_not_found()
        {
            //Arrange
            const string fromCurrency = "USD";
            const string toCurrency = "BRL";
            const decimal fromValue = (decimal)65345106.01;
            var userId = Guid.NewGuid().ToString();


            conversionTransactionServiceMock
                .Setup(_ => _.RequestConversion(fromCurrency, toCurrency, fromValue, userId))
                .ReturnsAsync((GetConversionResult)null);

            //Act
            var result = await controller.GetConversion(fromValue, fromCurrency, toCurrency, userId);

            //Assert
            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Try_GET_conversion_throwing_error()
        {
            //Arrange
            const string fromCurrency = "USD";
            const string toCurrency = "BRL";
            const decimal fromValue = (decimal)65345106.01;
            var userId = Guid.NewGuid().ToString();


            conversionTransactionServiceMock
                .Setup(_ => _.RequestConversion(fromCurrency, toCurrency, fromValue, userId))
                .ThrowsAsync(new InvalidCastException());

            //Assert
            await Assert.ThrowsAsync<InvalidCastException>(() => controller.GetConversion(fromValue, fromCurrency, toCurrency, userId));
        }

        [Fact]
        public async Task Try_GET_conversion_inserting_invalid_currency()
        {
            //Arrange
            const string fromCurrency = "ASD";
            const string toCurrency = "BRL";
            const decimal fromValue = (decimal)65345106.01;
            var userId = Guid.NewGuid().ToString();

            conversionTransactionServiceMock
                .Setup(_ => _.RequestConversion(fromCurrency, toCurrency, fromValue, userId))
                .ThrowsAsync(new HttpRequestException("Currency is not valid.", null, HttpStatusCode.BadRequest));

            var result = await controller.GetConversion(fromValue, fromCurrency, toCurrency, userId);
            
            //Assert
            Assert.IsAssignableFrom<BadRequestObjectResult>(result.Result);
        }
    }
}
