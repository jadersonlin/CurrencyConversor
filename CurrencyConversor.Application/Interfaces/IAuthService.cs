using System.Threading.Tasks;

namespace CurrencyConversor.Application.Interfaces
{
    public  interface IAuthService
    {
        Task<bool> UserExists(string userId);
    }
}
