using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace UserManagement.Api.IntegrationTests
{
    public class UserControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public UserControllerTests()
        {
            _client = new HttpClient
            {
                BaseAddress = new System.Uri("http://localhost:5000")
            };
        }

        [Fact]
        public async Task GetUsers_ReturnsSuccessStatusCode()
        {
            // Arrange
            var request = "/User";

            // Act
            var response = await _client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetUser_ReturnsCorrectUser()
        {
            // Arrange
            var request = "/User/1";

            // Act
            var response = await _client.GetAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("Luis Horacio", responseBody);
        }
    }
}
