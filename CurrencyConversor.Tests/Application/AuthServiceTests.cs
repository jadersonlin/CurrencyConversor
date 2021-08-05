using CurrencyConversor.Application.Impl;
using CurrencyConversor.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CurrencyConversor.Tests.Application
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly ILogger<AuthService> logger;

        public AuthServiceTests()
        {
            ILoggerFactory nullLoggerFactory = new NullLoggerFactory();
            logger = nullLoggerFactory.CreateLogger<AuthService>();
            userRepositoryMock = new Mock<IUserRepository>();
        }

        [Fact]
        public async Task Can_verify_user_exists()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            userRepositoryMock.Setup(_ => _.Exists(userId)).ReturnsAsync(true);

            //Act
            var authService = new AuthService(userRepositoryMock.Object, logger);
            var result = await authService.UserExists(userId);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Try_verify_user_exists_with_error_on_repository()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            userRepositoryMock.Setup(_ => _.Exists(userId)).ThrowsAsync(new TimeoutException());

            //Act
            var authService = new AuthService(userRepositoryMock.Object, logger);

            //Assert
            await Assert.ThrowsAsync<TimeoutException>(() => authService.UserExists(userId));
        }
    }
}
