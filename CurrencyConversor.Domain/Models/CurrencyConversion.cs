using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CurrencyConversor.Domain.Abstraction;
using CurrencyConversor.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace CurrencyConversor.Domain.Models
{
    public class CurrencyConversion
    {
        private readonly IExternalCurrenciesService currenciesService;
        private readonly ILogger<CurrencyConversion> logger;

        public CurrencyConversion(IExternalCurrenciesService currenciesService, ILogger<CurrencyConversion> logger)
        {
            this.currenciesService = currenciesService;
            this.logger = logger;
        }

        public Currency FromCurrency { get; private set; }

        public Currency ToCurrency { get; private set; }

        public decimal FromValue { get; private set; }

        public ConversionStatus ConversionStatus { get; private set; } = ConversionStatus.NotSet;

        public decimal? ConversionValue { get; private set; }

        public decimal? ConversionRate { get; private set; }

        public long? ConversionTimestamp { get; private set; }

        public void PrepareForConversion(Currency fromCurrency, Currency toCurrency, decimal fromValue)
        {
            FromCurrency = fromCurrency;
            ToCurrency = toCurrency;
            FromValue = fromValue;

            SetConversionStatus(ConversionStatus.PreparedForConversion);
        }

        private void SetConversionStatus(ConversionStatus newStatus)
        {
            ConversionStatus = newStatus;
        }

        public async Task<ConversionTransaction> ExecuteConversion()
        {
            try
            {
                VerifyPreConversionStatus();

                var conversionData = await currenciesService.GetConversionRate(FromCurrency.Code, ToCurrency.Code);
                ConversionRate = conversionData.ConversionRate;
                ConversionTimestamp = conversionData.ConversionTimestamp;
                
                CalculateConversionValue();

                SetConversionStatus(ConversionStatus.ConversionDone);

                return SucessTransaction.CreateNew(this);
            }
            catch (Exception ex)
            {
                LogConversionError(ex);
                SetConversionStatus(ConversionStatus.ErrorInConversion);

                return FailureTransaction.CreateNew(this, ex.Message);
            }
        }

        private void VerifyPreConversionStatus()
        {
            if (ConversionStatus != ConversionStatus.PreparedForConversion)
                throw new InvalidOperationException($"Data is not prepared for conversion. Status: {ConversionStatus}.");
        }

        private void LogConversionError(Exception ex)
        {
            logger.LogError($"Could not convert currencies using FromValue: {FromValue}, "
                            + $"FromCurrency: {FromCurrency.Code} and ToCurrency: {ToCurrency.Code}", ex);
        }

        private void CalculateConversionValue()
        {
            ConversionValue = ConversionRate * FromValue;
        }
    }
}
