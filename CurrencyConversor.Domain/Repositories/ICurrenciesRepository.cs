using CurrencyConversor.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyConversor.Domain.Repositories
{
    public interface ICurrenciesRepository
    {
        Task<IList<Currency>> GetAll();

        Task<bool> InsertMany();
    }
}
