using CurrencyConversor.Application.Interfaces;
using CurrencyConversor.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CurrencyConversor.Application.Impl
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository userRepository;
        private readonly ILogger<AuthService> logger;

        public AuthService(IUserRepository userRepository,
                           ILogger<AuthService> logger)
        {
            this.userRepository = userRepository;
            this.logger = logger;
        }

        public async Task<bool> UserExists(string userId)
        {
            try
            {
                return await userRepository.Exists(userId);
            }
            catch (Exception ex)
            {
                LogError(userId, ex);
                throw;
            }
        }

        private void LogError(string userId, Exception ex)
        {
            logger.LogError($"Erro ao consultar userId {userId}", ex);
        }
    }
}
