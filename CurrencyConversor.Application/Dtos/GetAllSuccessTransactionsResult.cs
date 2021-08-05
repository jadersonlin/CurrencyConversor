using System.Collections.Generic;

namespace CurrencyConversor.Application.Dtos
{
    public class GetAllSuccessTransactionsResult : ResultBase
    {
        public IList<SuccessTransactionDto> Transactions { get; set; }
    }
}
