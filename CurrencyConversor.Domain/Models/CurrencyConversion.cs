using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
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

            SetConversionStatus(ConversionStatus.NotSet);
        }

        public Currency FromCurrency { get; private set; }

        public Currency ToCurrency { get; private set; }

        public decimal FromValue { get; private set; }

        public ConversionStatus ConversionStatus { get; private set; }

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

        public async Task ExecuteConversion()
        {
            try
            {
                VerifyPreConversionStatus();

                var conversionData = await currenciesService.GetConversionRate(FromCurrency.Code, ToCurrency.Code);
                ConversionRate = conversionData.ConversionRate;
                ConversionTimestamp = conversionData.ConversionTimestamp;
                
                CalculateConversionValue();

                SetConversionStatus(ConversionStatus.ConversionDone);
            }
            catch (Exception ex)
            {
                logger.LogError("Could not convert currencies using FromValue: {FromValue}, FromCurrency: {FromCurrency.Code} and ToCurrency: {ToCurrency.Code}", ex);
                SetConversionStatus(ConversionStatus.ErrorInConversion);
            }
        }

        private void VerifyPreConversionStatus()
        {
            if (ConversionStatus != ConversionStatus.PreparedForConversion)
                throw new InvalidOleVariantTypeException($"Data is not prepared for conversion. Status: {ConversionStatus}.");
        }

        private void VerifyPostConversionStatus()
        {
            if (ConversionStatus != ConversionStatus.ConversionDone)
                throw new InvalidOleVariantTypeException($"Conversion is not done. Status: {ConversionStatus}.");
        }

        private void CalculateConversionValue()
        {
            ConversionValue = ConversionRate * FromValue;
        }
    }
}
