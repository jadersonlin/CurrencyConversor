using CurrencyConversor.Application.Dtos;
using System.Threading.Tasks;

namespace CurrencyConversor.Application.Interfaces
{
    public interface IConversionTransactionService
    {
        Task<GetAllSuccessTransactionsResult> GetAllSuccessfulTransactions();
        Task<GetAllFailureTransactionsResult> GetAllFailedTransactions();
        Task<GetConversionResult> RequestConversion(string fromCurrency, string toCurrency, decimal fromValueParam, string userIdParam);
    }
}
