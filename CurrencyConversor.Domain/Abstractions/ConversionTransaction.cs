using CurrencyConversor.Domain.Models;

namespace CurrencyConversor.Domain.Abstractions
{
    public abstract class ConversionTransaction
    {
        public string Id { get; set; }
        public ConversionStatus ConversionStatus { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal? FromValue { get; set; }
        public long? ConversionTimestamp { get; set; }
        public string UserId { get; set; }
    }
}
