using CurrencyConversor.Domain.Models;
using MongoDB.Driver;

namespace CurrencyConversor.Infraestructure.MongoDB
{
    public interface ICurrencyContext
    {
        IMongoCollection<Currency> Currencies { get; }

        IMongoCollection<SuccessTransaction> SuccessTransactions { get; }

        IMongoCollection<FailureTransaction> FailureTransactions { get; }

        IMongoCollection<User> Users { get; }
    }
}
