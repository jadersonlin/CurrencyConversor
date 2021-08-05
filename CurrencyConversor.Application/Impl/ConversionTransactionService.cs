using CurrencyConversor.Application.Dtos;
using CurrencyConversor.Application.Interfaces;
using CurrencyConversor.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyConversor.Domain.Models;
using CurrencyConversor.Domain.Repositories;

namespace CurrencyConversor.Application.Impl
{
    public class ConversionTransactionService : IConversionTransactionService
    {
        private readonly ICurrencyConversionService currencyConversionService;
        private readonly IAuthService authService;
        private readonly ICurrenciesService currenciesService;
        private readonly IConversionTransactionRepository<SuccessTransaction> successTransactionRepository;
        private readonly IConversionTransactionRepository<FailureTransaction> failureTransactionRepository;
        private readonly ILogger<ConversionTransactionService> logger;

        public ConversionTransactionService(ICurrencyConversionService currencyConversionService,
                                            IAuthService authService,
                                            ICurrenciesService currenciesService,
                                            IConversionTransactionRepository<SuccessTransaction> successTransactionRepository,
                                            IConversionTransactionRepository<FailureTransaction> failureTransactionRepository,
                                            ILogger<ConversionTransactionService> logger)
        {
            this.currencyConversionService = currencyConversionService;
            this.authService = authService;
            this.currenciesService = currenciesService;
            this.successTransactionRepository = successTransactionRepository;
            this.failureTransactionRepository = failureTransactionRepository;
            this.logger = logger;
        }

        public async Task<GetAllSuccessTransactionsResult> GetAllSuccessfulTransactions()
        {
            var transactions = await successTransactionRepository.GetTransactions();

            return MapSuccessfulTransactions(transactions);
        }

        private GetAllSuccessTransactionsResult MapSuccessfulTransactions(IList<SuccessTransaction> transactions)
        {
            return new GetAllSuccessTransactionsResult
            {
                Transactions = transactions.Select(t => new SuccessTransactionDto
                {
                    Id = t.Id,
                    FromValue = t.FromValue.Value,
                    FromCurrency = t.FromCurrency,
                    ToCurrency = t.ToCurrency,
                    ConversionTimestamp = t.ConversionTimestamp.Value,
                    UserId = t.UserId,
                    ConversionRate = t.ConversionRate
                }).ToList()
            };
        }

        public async Task<GetAllFailureTransactionsResult> GetAllFailedTransactions()
        {
            var transactions = await failureTransactionRepository.GetTransactions();

            return MapFailedTransactions(transactions);
        }

        private GetAllFailureTransactionsResult MapFailedTransactions(IList<FailureTransaction> transactions)
        {

            return new GetAllFailureTransactionsResult
            {
                Transactions = transactions.Select(t => new FailureTransactionDto
                {
                    Id = t.Id,
                    FromValue = t.FromValue,
                    FromCurrency = t.FromCurrency,
                    ToCurrency = t.ToCurrency,
                    ConversionTimestamp = t.ConversionTimestamp,
                    ErrorMessage = t.ErrorMessage,
                    UserId = t.UserId
                }).ToList()
            };
        }

        public Task<GetConversionResult> RequestConversion(string fromCurrency, string toCurrency, decimal fromValueParam, string userIdParam)
        {
            throw new NotImplementedException();
        }
    }
}
