using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConversor.Application.Dtos
{
    public class GetAllCurrenciesResult : ResultBase
    {
        public IList<CurrencyDto> Currencies { get; set; }
    }
}
