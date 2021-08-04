using System.ComponentModel;

namespace CurrencyConversor.Domain.Models
{
    public enum ConversionStatus
    {
        [Description("Not set")]
        NotSet = 0,

        [Description("Prepared for conversion")]
        PreparedForConversion = 1,

        [Description("Conversion done")]
        ConversionDone = 2,

        [Description("Error in Conversion")]
        ErrorInConversion = 3
    }
}
