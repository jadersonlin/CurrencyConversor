using System;
using System.Threading.Tasks;
using CurrencyConversor.Domain.Abstractions;
using CurrencyConversor.Domain.Interfaces;
using CurrencyConversor.Domain.Models;
using Microsoft.Extensions.Logging;

namespace CurrencyConversor.Domain.Services
{
    public class CurrencyConversionService : ICurrencyConversionService
    {
        private readonly IExternalCurrenciesService currenciesService;
        private readonly ILogger<CurrencyConversionService> logger;

        public CurrencyConversionService(IExternalCurrenciesService currenciesService, 
                                         ILogger<CurrencyConversionService> logger)
        {
            this.currenciesService = currenciesService;
            this.logger = logger;
        }

        private Currency fromCurrency;
        private Currency toCurrency;
        private decimal fromValue;
        private decimal? conversionValue;
        private decimal? conversionRate;
        private long? conversionTimestamp;
        private string userId;

        public ConversionStatus ConversionStatus { get; private set; } = ConversionStatus.NotSet;

        public async Task<ConversionTransaction> Convert(Currency fromCurrencyParam, Currency toCurrencyParam, decimal fromValueParam, string userIdParam)
        {
            PrepareDataForConversion(fromCurrencyParam, toCurrencyParam, fromValueParam, userIdParam);
            return await ExecuteConversion();
        }

        private void PrepareDataForConversion(Currency fromCurrencyParam, Currency toCurrencyParam, decimal fromValueParam, string userIdParam)
        {
            fromCurrency = fromCurrencyParam;
            toCurrency = toCurrencyParam;
            fromValue = fromValueParam;
            userId = userIdParam;

            SetConversionStatus(ConversionStatus.PreparedForConversion);
        }

        private void SetConversionStatus(ConversionStatus newStatus)
        {
            ConversionStatus = newStatus;
        }

        private async Task<ConversionTransaction> ExecuteConversion()
        {
            try
            {
                VerifyPreConversionStatus();

                var conversionData = await currenciesService.GetConversionRate(fromCurrency.Code, toCurrency.Code);
                conversionRate = conversionData.ConversionRate;
                conversionTimestamp = conversionData.ConversionTimestamp;
                
                CalculateConversionValue();

                return GetSuccessTransaction();
            }
            catch (Exception ex)
            {
                LogConversionError(ex);

                return GetFailureTransaction(ex);
            }
        }


        private void VerifyPreConversionStatus()
        {
            if (ConversionStatus != ConversionStatus.PreparedForConversion)
                throw new InvalidOperationException($"Data is not prepared for conversion. Status: {ConversionStatus}.");
        }

        private void CalculateConversionValue()
        {
            conversionValue = conversionRate * fromValue;
        }

        private void LogConversionError(Exception ex)
        {
            logger.LogError("Could not convert currencies.", ex);
        }

        private ConversionTransaction GetFailureTransaction(Exception ex)
        {
            SetConversionStatus(ConversionStatus.ErrorInConversion);

            return new FailureTransaction
            {
                Id = Guid.NewGuid().ToString(),
                ErrorMessage = ex.Message,
                FromCurrency = fromCurrency?.Code,
                ToCurrency = toCurrency?.Code,
                FromValue = fromValue,
                ConversionTimestamp = conversionTimestamp ?? DateTime.UtcNow.ToFileTimeUtc(),
                UserId = userId
            };
        }

        private ConversionTransaction GetSuccessTransaction()
        {
            SetConversionStatus(ConversionStatus.ConversionDone);

            return new SuccessTransaction
            {
                Id = Guid.NewGuid().ToString(),
                FromValue = fromValue,
                ConversionValue = conversionValue.Value,
                ConversionRate = conversionRate.Value,
                ToCurrency = toCurrency.Code,
                FromCurrency = fromCurrency.Code,
                ConversionTimestamp = conversionTimestamp,
                UserId = userId
            };
        }
    }
}
