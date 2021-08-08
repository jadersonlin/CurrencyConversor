using CurrencyConversor.Domain.Models;
using CurrencyConversor.Domain.Repositories;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace CurrencyConversor.Infraestructure.MongoDB
{
    public class UserMongoDbRepository : IUserRepository
    {
        private readonly ICurrencyContext context;

        public UserMongoDbRepository(ICurrencyContext context)
        {
            this.context = context;
        }

        public async Task<bool> Any()
        {
            return await context
                .Users
                .Find(u => true)
                .AnyAsync();
        }

        public async Task<bool> Exists(string userId)
        {
            return await context
                .Users
                .Find(u => u.UserId == userId)
                .AnyAsync();
        }

        public async Task InsertUser(User user)
        {
            await context
                .Users
                .InsertOneAsync(user);
        }
    }
}
