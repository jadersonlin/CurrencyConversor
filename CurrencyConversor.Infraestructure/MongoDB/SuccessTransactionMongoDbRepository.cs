using CurrencyConversor.Domain.Models;
using CurrencyConversor.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CurrencyConversor.Domain.Abstractions;
using MongoDB.Driver;

namespace CurrencyConversor.Infraestructure.MongoDB
{
    public class SuccessTransactionMongoDbRepository<T> : IConversionTransactionRepository<T> where T : SuccessTransaction
    {
        private readonly ICurrencyContext context;

        public SuccessTransactionMongoDbRepository(ICurrencyContext context)
        {
            this.context = context;
        }

        public async Task Insert(T transaction)
        {
            await context
                .SuccessTransactions
                .InsertOneAsync(transaction);
        }

        public async Task<IList<T>> GetTransactions()
        {
            return (IList<T>)await context
                .SuccessTransactions
                .Find(t => true)
                .ToListAsync();
        }
    }
}
