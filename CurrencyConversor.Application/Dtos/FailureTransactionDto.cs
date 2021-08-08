using System;

namespace CurrencyConversor.Application.Dtos
{
    public class FailureTransactionDto
    {
        public string Id { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal? FromValue { get; set; }
        public DateTime ConversionTimestamp { get; set; }
        public string UserId { get; set; }
        public string ErrorMessage { get; set; }
    }
}
