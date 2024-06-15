using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Stock.API.Dtos;
using Stock.API.Objects;

namespace Stock.API.IntegrationTests;

public class StockControllerTest: IntegrationTest
{
    [Fact]
    public async Task testFindAllStocks_Returns_Success()
    {
        //Arrange
        await AuthenticateAsync();
        var stocks = Helper.GetStockDtos();
        //Act
        var response = await testClient.GetAsync("/api/stock");
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotAcceptable);
        


    }
    
}
//extends the IntegrationTest class

/*
 *Here's an example of an integration test for a controller endpoint secured by JWT:

csharp
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace YourNamespace.IntegrationTests
{
    public class YourControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public YourControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetData_WithValidToken_ReturnsSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c"; // Replace with a valid JWT token
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await client.GetAsync("/your-controller-endpoint");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("expected response content", responseContent);
        }

        [Fact]
        public async Task GetData_WithoutToken_ReturnsUnauthorized()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/your-controller-endpoint");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}


In this example, we're testing a controller endpoint that requires a JWT token for authorization. We're using the WebApplicationFactory from Microsoft.AspNetCore.Mvc.Testing to create a test client that can make requests to the controller endpoint.

In the first test, we're setting a valid JWT token in the request header and then making a GET request to the controller endpoint. We're asserting that the response is successful and contains the expected content.

In the second test, we're making a GET request to the controller endpoint without a JWT token and asserting that the response is unauthorized (401).

Remember to replace the your-controller-endpoint and expected response content placeholders with the actual values for your controller endpoint and expected response.
 *
 * 
 */