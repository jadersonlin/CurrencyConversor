using System.Collections.Generic;

namespace CurrencyConversor.Infraestructure.External
{
    public class ApiLayerExchangeSuccessData
    {
        public bool Success { get; set; }

        public string Terms { get; set; }

        public string Privacy { get; set; }

        public long Timestamp { get; set; }

        public string Source { get; set; }

        public IDictionary<string, decimal> Quotes { get; set; }
    }
}
