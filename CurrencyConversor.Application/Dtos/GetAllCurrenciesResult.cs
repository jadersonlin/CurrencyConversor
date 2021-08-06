using System.Collections.Generic;

namespace CurrencyConversor.Application.Dtos
{
    public class GetAllCurrenciesResult : ResultBase
    {
        public IList<CurrencyDto> Currencies { get; set; }
    }
}
