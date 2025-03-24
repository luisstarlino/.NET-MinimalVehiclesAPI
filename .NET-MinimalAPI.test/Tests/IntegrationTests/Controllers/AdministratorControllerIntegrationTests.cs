using _NET_MinimalAPI.test.Tests.Setup;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;


namespace _NET_MinimalAPI.test.Tests.IntegrationTests.Controllers
{
    public class AdministratorControllerIntegrationTests
    {
        private readonly HttpClient _client;

        public AdministratorControllerIntegrationTests(WebApplicationFactory<TestStartup> factory)
        {
            _client = factory.CreateClient();
        }

        
        [Fact]
        public async Task AddAdministrator_ShouldReturn201Created()
        {
            // Arrange
            var newAdmin = new
            {
                Mail = "newadmin@mail.com",
                Password = "123456",
                Profile = "admin"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/administrator", newAdmin);

            // Assert
            Xunit.Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
