namespace CurrencyConversor.Domain.Models
{
    public class ExternalConversionData
    {
        public string BaseCurrency { get; set; }
        
        public string FromCurrency { get; set; }
        
        public string ToCurrency { get; set; }
        
        /// <summary>
        /// Conversion between BaseCurrency (always USD) and FromCurency
        /// </summary>
        public decimal? BaseFromConversionRate { get; set; }

        /// <summary>
        /// Conversion between base (always USD) and ToCurency
        /// </summary>
        public decimal? BaseToConversionRate { get; set; }

        
        /// <summary>
        /// Conversion between FromCurrency and ToCurrency, based on BaseCurrency
        /// </summary>
        public decimal? ConversionRate => BaseToConversionRate / BaseFromConversionRate;
     
        public long? ConversionTimestamp { get; set; }
    }
}
