using System.Threading.Tasks;
using CurrencyConversor.Domain.Models;

namespace CurrencyConversor.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<bool> AnyUserExists();
        Task<bool> Exists(string userId);
        Task<bool> InsertUser(User user);
    }
}
