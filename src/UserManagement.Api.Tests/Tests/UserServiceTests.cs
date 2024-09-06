using Moq;
using Xunit;
using UserManagement.Application.Services;
using UserManagement.Domain.Interfaces;
using UserManagement.Domain.Entities;

namespace UserManagement.Api.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public async  void GetUser_ShouldReturnUser()
        {
            // Arrange
            var mockRepository = new Mock<IUserRepository>();
            mockRepository.Setup(repo => repo.GetUserAsync(It.IsAny<int>())).ReturnsAsync(new User { Id = 1, Name = "Luis Horacio" });
            var userService = new UserService(mockRepository.Object);

            // Act
            var user = await userService.GetUserAsync(1);

            // Assert
            Assert.NotNull(user);
            Assert.Equal("Luis Horacio", user.Name);
        }
    }
}
