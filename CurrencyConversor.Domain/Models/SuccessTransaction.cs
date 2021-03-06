using System;
using CurrencyConversor.Domain.Abstractions;

namespace CurrencyConversor.Domain.Models
{
    public class SuccessTransaction : ConversionTransaction
    {
        public SuccessTransaction()
        {
            ConversionStatus = ConversionStatus.ConversionDone;
        }

        public decimal ConversionRate { get; set; }
    }
}
