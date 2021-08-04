using CurrencyConversor.Application.Dtos;
using System.Threading.Tasks;

namespace CurrencyConversor.Application.Interfaces
{
    public interface IConversionTransactionsService
    {
        Task<GetTransactionsResult> GetAllTransactions();
    }
}
