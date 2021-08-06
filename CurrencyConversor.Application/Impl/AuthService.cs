using System.Net;
using System.Net.Http;
using CurrencyConversor.Application.Interfaces;
using CurrencyConversor.Domain.Repositories;
using System.Threading.Tasks;

namespace CurrencyConversor.Application.Impl
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository userRepository;

        public AuthService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task ValidateUser(string userId)
        {
            var exists = await userRepository.Exists(userId);

            if (!exists)
                throw new HttpRequestException("Invalid user.", null, HttpStatusCode.Unauthorized);
        }
    }
}
