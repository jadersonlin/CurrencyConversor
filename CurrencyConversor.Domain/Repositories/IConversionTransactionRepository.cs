using System.Collections.Generic;
using System.Threading.Tasks;
using CurrencyConversor.Domain.Abstractions;

namespace CurrencyConversor.Domain.Repositories
{
    public interface IConversionTransactionRepository<T> where T : ConversionTransaction
    {
        Task Insert(T transaction);

        Task<IList<T>> GetTransactions();
    }
}
