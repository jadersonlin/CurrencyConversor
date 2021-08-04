using System;
using CurrencyConversor.Domain.Abstraction;

namespace CurrencyConversor.Domain.Models
{
    public class FailureTransaction : ConversionTransaction
    {
        private FailureTransaction()
        {
            ConversionStatus = ConversionStatus.ErrorInConversion;
        }

        public string ErrorMessage { get; set; }

        public static FailureTransaction CreateNew(CurrencyConversion conversion, string errorMessage)
        {
            if (conversion == null) 
                throw new ArgumentNullException(nameof(conversion));

            return new FailureTransaction
            {
                ErrorMessage = errorMessage,
                FromCurrency = conversion.FromCurrency.Code,
                ToCurrency = conversion.ToCurrency.Code,
                FromValue = conversion.FromValue,
                ConversionTimestamp = conversion.ConversionTimestamp ?? DateTime.UtcNow.ToFileTimeUtc(),
            };
        }
    }
}
