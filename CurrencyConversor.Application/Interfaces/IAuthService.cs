using System.Threading.Tasks;

namespace CurrencyConversor.Application.Interfaces
{
    public  interface IAuthService
    {
        Task ValidateUser(string userId);
    }
}
