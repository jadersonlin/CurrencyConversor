using CurrencyConversor.Domain.Abstraction;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyConversor.Domain.Repositories
{
    public interface IConversionTransactionRepository<T> where T : ConversionTransaction
    {
        Task<bool> Insert(T transaction);

        Task<IList<T>> GetSuccessTransactions();

        Task<IList<T>> GetFailureTransactions();
    }
}
