using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyConversor.Application.Dtos;
using CurrencyConversor.Application.Impl;
using CurrencyConversor.Application.Interfaces;
using CurrencyConversor.Domain.Interfaces;
using CurrencyConversor.Domain.Models;
using CurrencyConversor.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace CurrencyConversor.Tests.Application
{
    public class ConversionTransactionServiceTests
    {
        private readonly Mock<ICurrencyConversionService> currencyConversionServiceMock;
        private readonly Mock<IAuthService> authServiceMock;
        private readonly Mock<ICurrenciesService> currenciesServiceMock;
        private readonly Mock<IConversionTransactionRepository<SuccessTransaction>> successTransactionRepositoryMock;
        private readonly Mock<IConversionTransactionRepository<FailureTransaction>> failureTransactionRepositoryMock;
        private readonly ILogger<ConversionTransactionService> logger;

        public ConversionTransactionServiceTests()
        {
            ILoggerFactory nullLoggerFactory = new NullLoggerFactory();
            logger = nullLoggerFactory.CreateLogger<ConversionTransactionService>();
            currencyConversionServiceMock = new Mock<ICurrencyConversionService>();
            authServiceMock = new Mock<IAuthService>();
            currenciesServiceMock = new Mock<ICurrenciesService>();
            successTransactionRepositoryMock = new Mock<IConversionTransactionRepository<SuccessTransaction>>();
            failureTransactionRepositoryMock = new Mock<IConversionTransactionRepository<FailureTransaction>>();
        }

        [Fact]
        public async Task Can_get_failed_transactions()
        {
            //Arrenge

            var failureTransactions = new List<FailureTransaction>
            {
                new FailureTransaction
                {
                    Id = Guid.NewGuid().ToString(),
                    ConversionStatus = ConversionStatus.ErrorInConversion,
                    FromValue = 1,
                    FromCurrency = "BRL",
                    ToCurrency = "USD",
                    ConversionTimestamp = new DateTime(2021,08,03, 11, 11, 0).ToFileTimeUtc(),
                    ErrorMessage = "Timeout error"
                },
                new FailureTransaction
                {
                    Id = Guid.NewGuid().ToString(),
                    ConversionStatus = ConversionStatus.ErrorInConversion,
                    FromValue = 2,
                    FromCurrency = "BRL",
                    ToCurrency = "USD",
                    ConversionTimestamp = new DateTime(2021, 08, 03, 11, 12, 0).ToFileTimeUtc(),
                    ErrorMessage =  "Timeout error",
                    UserId = Guid.NewGuid().ToString()
                },
            };

            failureTransactionRepositoryMock.Setup(_ => _.GetTransactions()).ReturnsAsync(failureTransactions);

            var conversionTransactionService = new ConversionTransactionService(currencyConversionServiceMock.Object,
                authServiceMock.Object, currenciesServiceMock.Object, successTransactionRepositoryMock.Object,
                failureTransactionRepositoryMock.Object, logger);

            var result = await conversionTransactionService.GetAllFailedTransactions();

            Assert.IsAssignableFrom<GetAllFailureTransactionsResult>(result);
            Assert.IsAssignableFrom<List<FailureTransactionDto>>(result.Transactions);
            Assert.Equal(2, result.Transactions.Count);
            Assert.DoesNotContain(result.Transactions, t => string.IsNullOrWhiteSpace(t.ErrorMessage));
        }

        [Fact]
        public async Task Can_get_successful_transactions()
        {
            //Arrenge

            var successTransactions = new List<SuccessTransaction>
            {
                new SuccessTransaction
                {
                    Id = Guid.NewGuid().ToString(),
                    ConversionStatus = ConversionStatus.ConversionDone,
                    FromValue = 1,
                    FromCurrency = "BRL",
                    ToCurrency = "USD",
                    ConversionTimestamp = new DateTime(2021,08,03, 11, 11, 0).ToFileTimeUtc(),
                    ConversionRate = (decimal)0.3430
                },
                new SuccessTransaction
                {
                    Id = Guid.NewGuid().ToString(),
                    ConversionStatus = ConversionStatus.ConversionDone,
                    FromValue = 2,
                    FromCurrency = "BRL",
                    ToCurrency = "JPY",
                    ConversionTimestamp = new DateTime(2021, 08, 03, 11, 12, 0).ToFileTimeUtc(),
                    UserId = Guid.NewGuid().ToString(),
                    ConversionRate = (decimal)0.10319
                },
            };

            successTransactionRepositoryMock.Setup(_ => _.GetTransactions()).ReturnsAsync(successTransactions);

            var conversionTransactionService = new ConversionTransactionService(currencyConversionServiceMock.Object,
                authServiceMock.Object, currenciesServiceMock.Object, successTransactionRepositoryMock.Object,
                failureTransactionRepositoryMock.Object, logger);

            var result = await conversionTransactionService.GetAllSuccessfulTransactions();

            Assert.IsAssignableFrom<GetAllSuccessTransactionsResult>(result);
            Assert.IsAssignableFrom<List<SuccessTransactionDto>>(result.Transactions);
            Assert.Equal(2, result.Transactions.Count);
        }
    }
}
