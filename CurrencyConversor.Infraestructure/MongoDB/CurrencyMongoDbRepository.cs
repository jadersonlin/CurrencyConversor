using CurrencyConversor.Domain.Models;
using CurrencyConversor.Domain.Repositories;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyConversor.Infraestructure.MongoDB
{
    public class CurrencyMongoDbRepository : ICurrencyRepository
    {
        private readonly ICurrencyContext context;

        public CurrencyMongoDbRepository(ICurrencyContext context)
        {
            this.context = context;
        }

        public async Task<IList<Currency>> GetAll()
        {
            return await context
                .Currencies
                .Find(c => true)
                .ToListAsync();
        }

        public async Task<Currency> Get(string code)
        {
            return await context
                .Currencies
                .Find(c => c.Code == code)
                .FirstOrDefaultAsync();
        }

        public async Task InsertMany(IList<Currency> currencies)
        {
            await context
                .Currencies
                .InsertManyAsync(currencies);
        }
    }
}
