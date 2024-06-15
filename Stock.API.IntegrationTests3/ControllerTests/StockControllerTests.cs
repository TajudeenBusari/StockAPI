using System.Drawing.Text;
using System.Net;
using System.Net.Http;
using Moq;
using Newtonsoft.Json;
using Stock.API.Dtos;
using Stock.API.Objects;
using Stock.UnitTests.Fixtures;

namespace Stock.API.IntegrationTests3.ControllerTests;

public class StockControllerTests : IDisposable

{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly AppAuthentication _appAuthentication;

    public StockControllerTests()
    {
        _appAuthentication = new AppAuthentication();
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Get_All_Stocks_Returns_UnAuthourized()
    {
        //Arrange
        var queryObject = new QueryObject();
        queryObject.Symbol = "MSFT";
        queryObject.CompanyName = "Microsoft";
        queryObject.SortBy = "";
        queryObject.IsDescending = false;
        queryObject.PageNumber = 2;
        queryObject.PageSize = 20;
        
        var stocks = StocksFixture.GetTestStocks(); //from my unit test folder project
        
        _factory.MockstockRepository
            .Setup(repo => repo.GetAllAsync(queryObject)).ReturnsAsync(stocks);
        
        //Act
        var response = await _client.GetAsync("api/stock");
        
        //Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode); 
    }

    [Fact]
    public async Task Get_All_Stocks_Returns_ListOfStocks()
    {
        //Arrange
        await _appAuthentication.AuthenticateAsync();
        
        var response = await _client.GetAsync("api/stock");

        Assert.
            NotEqual(HttpStatusCode.Unauthorized, response.StatusCode);


    }

    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
    }

}

/*
 *Here is an example of an integration test for a web API controller with secured endpoints, written in C# using the xUnit testing framework and the Moq mocking library:

using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Xunit;
using YourNamespace.Controllers;

namespace YourNamespace.Tests
{
    public class SecuredApiControllerTests
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;
        private readonly Mock<ISecurityService> _securityServiceMock;

        public SecuredApiControllerTests()
        {
            _factory = new WebApplicationFactory<Startup>();
            _client = _factory.CreateClient();
            _securityServiceMock = new Mock<ISecurityService>();
        }

        [Fact]
        public async Task GetData_ReturnsUnauthorized_WhenUserNotAuthenticated()
        {
            // Arrange
            _securityServiceMock.Setup(s => s.IsAuthenticated(It.IsAny<HttpRequestMessage>()))
                .Returns(false);

            // Act
            var response = await _client.GetAsync("/api/secured/data");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetData_ReturnsOk_WhenUserAuthenticated()
        {
            // Arrange
            _securityServiceMock.Setup(s => s.IsAuthenticated(It.IsAny<HttpRequestMessage>()))
                .Returns(true);

            // Act
            var response = await _client.GetAsync("/api/secured/data");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetData_ReturnsForbidden_WhenUserNotAuthorized()
        {
            // Arrange
            _securityServiceMock.Setup(s => s.IsAuthenticated(It.IsAny<HttpRequestMessage>()))
                .Returns(true);
            _securityServiceMock.Setup(s => s.IsAuthorized(It.IsAny<HttpRequestMessage>(), It.IsAny<string>()))
                .Returns(false);

            // Act
            var response = await _client.GetAsync("/api/secured/data");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task GetData_ReturnsOk_WhenUserAuthorized()
        {
            // Arrange
            _securityServiceMock.Setup(s => s.IsAuthenticated(It.IsAny<HttpRequestMessage>()))
                .Returns(true);
            _securityServiceMock.Setup(s => s.IsAuthorized(It.IsAny<HttpRequestMessage>(), It.IsAny<string>()))
                .Returns(true);

            // Act
            var response = await _client.GetAsync("/api/secured/data");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}

This test suite covers the following scenarios:

1. A user is not authenticated: the API returns a 401 Unauthorized response.
2. A user is authenticated but not authorized: the API returns a 403 Forbidden response.
3. A user is authenticated and authorized: the API returns a 200 OK response.

Note that the ISecurityService is a mock object that simulates the behavior of the actual security service. The WebApplicationFactory is used to create an instance of the web API controller under test, and the HttpClient is used to send requests to the API.
 *
 *
 *
 *
 * 
 */