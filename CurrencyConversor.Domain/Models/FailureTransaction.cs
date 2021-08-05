using System;
using CurrencyConversor.Domain.Abstractions;
using CurrencyConversor.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace CurrencyConversor.Domain.Models
{
    public class FailureTransaction : ConversionTransaction
    {
        public FailureTransaction()
        {
            ConversionStatus = ConversionStatus.ErrorInConversion;
        }

        public string ErrorMessage { get; set; }
    }
}
