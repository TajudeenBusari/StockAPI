using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Stock.API.Dtos;

namespace Stock.API.IntegrationTests2;

public class StockControllerTests
{
    [Fact]
    public async Task testAddStockDto()
    {
        //Arrange
        var appFactory = new StockAPIApplicationFactory();
        CreateRequestStockDto requestStockDto = new CreateRequestStockDto
        {
            CompanyName = "TJLogis",
            Symbol = "TJL",
            Purchase = 100,
            LastDiv = 200,
            Industry = "Logistics",
            MarketCap = 100000000000
        };
        /*requestStockDto.CompanyName = "TJLogis";
        requestStockDto.Symbol = "TJL";
        requestStockDto.Purchase = 100;
        requestStockDto.LastDiv = 200;
        requestStockDto.Industry = "Logistics";
        requestStockDto.MarketCap = 1000000000;*/

        var client = appFactory.CreateClient();

        //Act
        //var response = await client.PostAsJsonAsync("/api/stock", requestStockDto);
        var response = await client.PostAsJsonAsync("/api/stock", requestStockDto);

        //Assert
        response.EnsureSuccessStatusCode();
        var stockResponse = await response.Content.ReadFromJsonAsync<StockDto>();
        stockResponse?.Id.Should().BePositive();
        stockResponse?.CompanyName.Should().Contain("TJLogis");

    }
}