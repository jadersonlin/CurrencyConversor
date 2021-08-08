using CurrencyConversor.Application.Dtos;
using CurrencyConversor.Application.Interfaces;
using CurrencyConversor.Domain.Interfaces;
using CurrencyConversor.Domain.Models;
using CurrencyConversor.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CurrencyConversor.Application.Impl
{
    public class ConversionTransactionService : IConversionTransactionService
    {
        private readonly ICurrencyConversionService currencyConversionService;
        private readonly IAuthService authService;
        private readonly ICurrencyRepository currencyRepository;
        private readonly IConversionTransactionRepository<SuccessTransaction> successTransactionRepository;
        private readonly IConversionTransactionRepository<FailureTransaction> failureTransactionRepository;
        private readonly ILogger<ConversionTransactionService> logger;

        public ConversionTransactionService(ICurrencyConversionService currencyConversionService,
                                            IAuthService authService,
                                            ICurrencyRepository currencyRepository,
                                            IConversionTransactionRepository<SuccessTransaction> successTransactionRepository,
                                            IConversionTransactionRepository<FailureTransaction> failureTransactionRepository,
                                            ILogger<ConversionTransactionService> logger)
        {
            this.currencyConversionService = currencyConversionService;
            this.authService = authService;
            this.currencyRepository = currencyRepository;
            this.successTransactionRepository = successTransactionRepository;
            this.failureTransactionRepository = failureTransactionRepository;
            this.logger = logger;
        }

        public async Task<GetAllSuccessTransactionsResult> GetAllSuccessfulTransactions()
        {
            var transactions = await successTransactionRepository.GetTransactions();

            return Mapper.MapSuccessfulTransactions(transactions);
        }

        public async Task<GetAllFailureTransactionsResult> GetAllFailedTransactions()
        {
            var transactions = await failureTransactionRepository.GetTransactions();

            return Mapper.MapFailedTransactions(transactions);
        }

        public async Task<GetConversionResult> RequestConversion(string fromCurrency, string toCurrency, decimal fromValue, string userId)
        {
            await authService.ValidateUser(userId);

            var fromCurrencyObject = await GetCurrencyObject(fromCurrency);
            var toCurrencyObject = await GetCurrencyObject(toCurrency);

            var conversion = await currencyConversionService.Convert(fromCurrencyObject, toCurrencyObject, fromValue, userId);

            if (conversion is SuccessTransaction successTransaction)
            {
                await successTransactionRepository.Insert(successTransaction);

                return Mapper.MapSuccessConversion(successTransaction);
            }

            if (conversion is FailureTransaction failureTransaction)
            {
                await failureTransactionRepository.Insert(failureTransaction);

                logger.LogError($"Error in " + nameof(RequestConversion) + ". " + failureTransaction.ErrorMessage);

                throw new HttpRequestException(failureTransaction.ErrorMessage, null, HttpStatusCode.ServiceUnavailable);
            }

            throw new HttpRequestException("Server could not respond.", null, HttpStatusCode.ServiceUnavailable);
        }

        private async Task<Currency> GetCurrencyObject(string currencyCode)
        {
            if (currencyCode == null) 
                throw new ArgumentNullException(nameof(currencyCode));
            
            var currencyObject = await currencyRepository.Get(currencyCode);
            
            if (currencyObject == null)
                throw new HttpRequestException($"Currency code '{currencyCode}' not available for conversion.",
                    null, HttpStatusCode.BadRequest);

            return currencyObject;
        }
    }
}
