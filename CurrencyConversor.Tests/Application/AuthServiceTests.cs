using CurrencyConversor.Application.Impl;
using CurrencyConversor.Domain.Repositories;
using Moq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CurrencyConversor.Tests.Application
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> userRepositoryMock;

        public AuthServiceTests()
        {
            userRepositoryMock = new Mock<IUserRepository>();
        }

        [Fact]
        public async Task Can_verify_user_exists()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            userRepositoryMock.Setup(_ => _.Exists(userId)).ReturnsAsync(true);

            //Act and Assert
            var authService = new AuthService(userRepositoryMock.Object);
            await Assert.IsAssignableFrom<Task>(authService.ValidateUser(userId));
        }

        [Fact]
        public async Task Try_verify_user_exists_with_error_on_repository()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            userRepositoryMock.Setup(_ => _.Exists(userId)).ThrowsAsync(new TimeoutException());

            //Act
            var authService = new AuthService(userRepositoryMock.Object);

            //Assert
            await Assert.ThrowsAsync<TimeoutException>(() => authService.ValidateUser(userId));
        }

        [Fact]
        public async Task Try_verify_user_exists_false()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            userRepositoryMock.Setup(_ => _.Exists(userId)).ReturnsAsync(false);

            //Act
            var authService = new AuthService(userRepositoryMock.Object);

            //Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => authService.ValidateUser(userId));
        }
    }
}
