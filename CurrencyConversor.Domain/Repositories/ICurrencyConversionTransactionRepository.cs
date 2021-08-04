using System.Collections;
using System.Threading.Tasks;

namespace CurrencyConversor.Domain.Repositories
{
    public interface ICurrencyConversionTransactionRepository
    {
        Task<bool> Insert();

        Task<IList>
    }
}
