using System.Threading.Tasks;
using CurrencyConversor.Domain.Models;

namespace CurrencyConversor.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<bool> Any();
        Task<bool> Exists(string userId);
        Task InsertUser(User user);
    }
}
