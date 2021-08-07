using CurrencyConversor.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyConversor.Domain.Repositories
{
    public interface ICurrencyRepository
    {
        Task<IList<Currency>> GetAll();

        Task<Currency> Get(string code);

        Task InsertMany(IList<Currency> currencies);
    }
}
