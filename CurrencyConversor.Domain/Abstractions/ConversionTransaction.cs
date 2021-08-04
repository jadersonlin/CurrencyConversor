using CurrencyConversor.Domain.Models;

namespace CurrencyConversor.Domain.Abstraction
{
    public abstract class ConversionTransaction
    {
        public string Id { get; private protected set; }
        public ConversionStatus ConversionStatus { get; private protected set; }
        public string FromCurrency { get; private protected set; }
        public string ToCurrency { get; private protected set; }
        public decimal FromValue { get; private protected set; }
        public long? ConversionTimestamp { get; private protected set; }
    }
}
