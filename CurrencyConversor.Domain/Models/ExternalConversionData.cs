namespace CurrencyConversor.Domain.Models
{
    public class ExternalConversionData
    {
        public string FromCurrency { get; set; }

        public string ToCurrency { get; set; }

        public decimal? ConversionRate { get; set; }

        public long? ConversionTimestamp { get; set; }
    }
}
