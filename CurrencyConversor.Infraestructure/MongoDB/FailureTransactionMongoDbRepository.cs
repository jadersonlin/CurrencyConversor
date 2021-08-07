using CurrencyConversor.Domain.Models;
using CurrencyConversor.Domain.Repositories;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyConversor.Infraestructure.MongoDB
{
    public class FailureTransactionMongoDbRepository<T> : IConversionTransactionRepository<T> where T : FailureTransaction
    {
        private readonly ICurrencyContext context;

        public FailureTransactionMongoDbRepository(ICurrencyContext context)
        {
            this.context = context;
        }

        public async Task Insert(T transaction)
        {
            await context
                .FailureTransactions
                .InsertOneAsync(transaction);
        }

        public async Task<IList<T>> GetTransactions()
        {
            return (IList<T>)await context
                .FailureTransactions
                .Find(t => true)
                .ToListAsync();
        }
    }
}
