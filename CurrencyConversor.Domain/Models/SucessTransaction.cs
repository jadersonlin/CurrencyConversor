using System;
using CurrencyConversor.Domain.Abstraction;

namespace CurrencyConversor.Domain.Models
{
    public class SucessTransaction : ConversionTransaction
    {
        private SucessTransaction()
        {
            ConversionStatus = ConversionStatus.ConversionDone;
        }

        public decimal? ConversionValue { get; private set; }

        public decimal? ConversionRate { get; private set; }

        public static SucessTransaction CreateNew(CurrencyConversion conversion)
        {
            return new SucessTransaction
            {
                Id = Guid.NewGuid().ToString(),
                FromValue = conversion.FromValue,
                ConversionValue = conversion.ConversionValue,
                ConversionRate = conversion.ConversionRate,
                ToCurrency = conversion.ToCurrency.Code,
                FromCurrency = conversion.FromCurrency.Code,
                ConversionTimestamp = conversion.ConversionTimestamp
            };
        }
    }
}
