using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CurrencyConversor.API.Controllers;
using CurrencyConversor.Application.Dtos;
using CurrencyConversor.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace CurrencyConversor.Tests.API
{
    public class TransactionsControllerTests
    {
        private readonly Mock<IConversionTransactionService> conversionTransactionServiceMock;
        private readonly ILogger<TransactionsController> logger;

        public TransactionsControllerTests()
        {
            conversionTransactionServiceMock = new Mock<IConversionTransactionService>();
            ILoggerFactory nullLoggerFactory = new NullLoggerFactory();
            logger = nullLoggerFactory.CreateLogger<TransactionsController>();
        }

        [Fact]
        public async Task Can_gen_success_transactions()
        {
            //Arrange

            var expectedResult = new GetAllSuccessTransactionsResult
            {
                Transactions = new List<SuccessTransactionDto>
                {
                    new SuccessTransactionDto
                    {
                        Id = Guid.NewGuid().ToString(),
                        FromValue = (decimal)4.02,
                        FromCurrency = "BRL",
                        ToCurrency = "JPY",
                        ConversionRate = (decimal)10.32,
                        ConversionTimestamp = DateTime.UtcNow,
                        UserId = Guid.NewGuid().ToString()
                    },
                    new SuccessTransactionDto
                    {
                        Id = Guid.NewGuid().ToString(),
                        FromValue = (decimal)2.02,
                        FromCurrency = "BRL",
                        ToCurrency = "USD",
                        ConversionRate = (decimal)0.32,
                        ConversionTimestamp = DateTime.UtcNow,
                        UserId = Guid.NewGuid().ToString()
                    },
                }
            };

            conversionTransactionServiceMock.Setup(_ => _.GetAllSuccessfulTransactions()).ReturnsAsync(expectedResult);

            //Act
            var controller = new TransactionsController(conversionTransactionServiceMock.Object, logger);
            var result = await controller.GetSuccessTransactions();

            Assert.IsAssignableFrom<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;

            Assert.IsAssignableFrom<GetAllSuccessTransactionsResult>(okResult.Value);
            var transactionsResult = (GetAllSuccessTransactionsResult)okResult.Value;
            Assert.IsAssignableFrom<IList<SuccessTransactionDto>>(transactionsResult.Transactions);
        }

        [Fact]
        public async Task Can_gen_failure_transactions()
        {
            //Arrange

            var expectedResult = new GetAllFailureTransactionsResult()
            {
                Transactions = new List<FailureTransactionDto>
                {
                    new FailureTransactionDto
                    {
                        Id = Guid.NewGuid().ToString(),
                        FromValue = (decimal)4.02,
                        FromCurrency = "BRL",
                        ToCurrency = "JPY",
                        ConversionTimestamp = DateTime.UtcNow,
                        UserId = Guid.NewGuid().ToString(),
                        ErrorMessage = "Timeout exception"
                    },
                    new FailureTransactionDto
                    {
                        Id = Guid.NewGuid().ToString(),
                        FromValue = (decimal)2.02,
                        FromCurrency = "BRL",
                        ToCurrency = "USD",
                        ConversionTimestamp = DateTime.UtcNow,
                        ErrorMessage = "Timeout exception"
                    }
                }
            };

            conversionTransactionServiceMock.Setup(_ => _.GetAllFailedTransactions()).ReturnsAsync(expectedResult);

            //Act
            var controller = new TransactionsController(conversionTransactionServiceMock.Object, logger);
            var result = await controller.GetFailureTransactions();

            Assert.IsAssignableFrom<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;

            Assert.IsAssignableFrom<GetAllFailureTransactionsResult>(okResult.Value);
            var transactionsResult = (GetAllFailureTransactionsResult)okResult.Value;
            Assert.IsAssignableFrom<IList<FailureTransactionDto>>(transactionsResult.Transactions);
        }
    }
}
