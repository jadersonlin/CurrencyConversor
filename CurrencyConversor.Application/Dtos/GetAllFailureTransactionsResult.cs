using System.Collections.Generic;

namespace CurrencyConversor.Application.Dtos
{
    public class GetAllFailureTransactionsResult : ResultBase
    {
        public IList<FailureTransactionDto> Transactions { get; set; }
    }
}
