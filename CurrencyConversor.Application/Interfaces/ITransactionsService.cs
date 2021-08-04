using CurrencyConversor.Application.Dtos;
using System.Threading.Tasks;

namespace CurrencyConversor.Application.Interfaces
{
    public interface ITransactionsService
    {
        Task<GetTransactionsResult> GetAllTransactions();
    }
}
