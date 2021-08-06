namespace CurrencyConversor.Application.Dtos
{
    public class GetConversionResult : ResultBase
    {
        public string Id { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal FromValue { get; set; }
        public long ConversionTimestamp { get; set; }
        public string UserId { get; set; }
        public decimal ConversionValue => FromValue * ConversionRate;
        public decimal ConversionRate { get; set; }
    }
}
