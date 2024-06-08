using System.Data.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Stock.API.Controllers;
using Stock.API.Dtos;
using Stock.API.Interfaces;
using Stock.API.Mappers;
using Stock.API.Objects;
using Stock.UnitTests.Fixtures;

namespace Stock.UnitTests.Systems.Controllers;

public class StockControllerTest
{
    private readonly StockController _sut;
    private readonly Mock<IStockRepository> _mockStockRepo = new Mock<IStockRepository>();
    private readonly StockMappers _stockMappers;
    private readonly CommentMappers _commentMappers;
   
    public StockControllerTest()
    {
        _sut = new StockController(_mockStockRepo.Object);
        _stockMappers = new StockMappers();
        _commentMappers = new CommentMappers();


    }
    //1. test Find all stock
    [Fact]
    public async Task testFindAllStock_OnSuccess_ReturnsStatusCode200Ok()
    {
        //Arrange
        
        var queryObject = new QueryObject();
        queryObject.Symbol = "MSFT";
        queryObject.CompanyName = "Microsoft";
        queryObject.SortBy = "";
        queryObject.IsDescending = false;
        queryObject.PageNumber = 2;
        queryObject.PageSize = 20;
        
        var stocks = StocksFixture.GetTestStocks();
        
        var stockDomain =
        _mockStockRepo
            .Setup(repo => repo.GetAllAsync(queryObject))
            .ReturnsAsync(stocks);
        
        
        
        //Act
        var result = (OkObjectResult)await _sut.GetAll(queryObject);
        

        //Assert
        result.StatusCode.Should().Be(200);
        result.Should().BeAssignableTo<OkObjectResult>();
        _mockStockRepo
            .Verify(repo => repo.GetAllAsync(queryObject), Times.Once());
       
    }

    //2. test Find a single stock
    [Fact]
    public async Task testFindAStock_OnSuccess_ReturnsStatusCode200Ok()
    {
        //Arrange
        //var mockStockRepo = new Mock<IStockRepository>();
        var stocks = StocksFixture.GetTestStocks();
        _mockStockRepo
            .Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(stocks[0]);
        //var sut = new StockController(_mockStockRepo.Object);

        //Act
        var result = (OkObjectResult) await _sut.GetById(1);

        //Assert
        Assert.NotNull(result);
        result.StatusCode.Should().Be(200);
        Assert.Equal(1, StocksFixture.GetTestStocks()[0].Id);
        Assert.True(1 == StocksFixture.GetTestStocks()[0].Id);
    }
    
    //3. test create a stock
    [Fact]
    public async Task testAddAStock_OnSuccess_Returns201Created()
    {
        //Arrange
        var stockDto = StocksFixture.TestAddStock();
        
        //var stockModel = StocksFixture.TestAStockModelById();
        
        var stock = StockMappers.ToStockFromStockDto(stockDto);
        
        _mockStockRepo
            .Setup(repo => repo.CreateAsync(stock))
            .ReturnsAsync(stock);
        
               // OR
               
        /*_mockStockRepo
            .Setup(repo =>
                repo.CreateAsync(It.IsAny<API.Models.Stock>()))
            .ReturnsAsync(stockModel);*/
        
        //Act
        var result =(ObjectResult)await _sut.Create(stockDto);
        
        //Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(201);
        _mockStockRepo
            .Verify(repo => repo.CreateAsync(stock), Times.Never());

    }
    
    //4. Test Update a stock
    [Fact]
    public async Task testUpdateStock_OnSuccess_Returns200Ok()
    {
        //Arrange
        var stockModel = StocksFixture.TestAStockModelById();
        //var stockDto = StockMappers.ToStockDto(stockModel);
        var stockUpdate = StocksFixture.TestUpdateStock();
        _mockStockRepo
            .Setup(repo => repo
                .UpdateAsync(stockModel.Id, stockUpdate)).
            ReturnsAsync(stockModel);
        
        //Act
        var result = (OkObjectResult)await _sut.Update(stockModel.Id, stockUpdate);

        //Assert
        result.StatusCode.Should().Be(200);
        _mockStockRepo
            .Verify(repo => repo.UpdateAsync(stockModel.Id, stockUpdate), Times.Once());

    }
    
    //Test Delete A stock
    [Fact]
    public async Task testDeleteStock_OnSuccess_Returns200Ok()
    {
        //Arrange
        var stockModel = StocksFixture.TestAStockModelById();
        _mockStockRepo
            .Setup(repo => repo.DeleteAsync(stockModel.Id))
            .ReturnsAsync(stockModel);
        //Act
        var result = (OkObjectResult)await _sut.Delete(stockModel.Id);
        
        //Assert
        result.StatusCode.Should().Be(200);
        Assert.NotNull(result);

    }
}








 /*//3. test Find all stock returns a list
    [Fact]
    public async Task testFindAllStock_ReturnsAList()
    {
        //Arrange
        var stocks = StocksFixture.GetTestStocks();
        //var stockDto = StockMappers.ToStockDto(new API.Models.Stock());
        var mockStockRepo = new Mock<IStockRepository>();
        var stockDto = new StockDto();
        stockDto.Id = 1;
        stockDto.CompanyName = ",Microsoft";
        stockDto.Symbol = "MSFT";
        stockDto.Purchase = 100;
        stockDto.LastDiv = 100;
        stockDto.Industry = "Software";
        stockDto.MarketCap = 10000000000;
        
        
        var queryObject = new QueryObject();
        queryObject.Symbol = "MSFT";
        queryObject.CompanyName = "Microsoft";
        queryObject.SortBy = "";
        queryObject.IsDescending = false;
        queryObject.PageNumber = 2;
        queryObject.PageSize = 20;
        
        mockStockRepo
            .Setup(repo => repo.GetAllAsync(queryObject))
            .ReturnsAsync(stocks);
        var sut = new StockController(mockStockRepo.Object);
        
        
        //Act
        var result = await sut.GetAll(queryObject);
        
        
        //Assert
        result.Should().BeOfType<OkObjectResult>();
        var objectResult = (OkObjectResult)result;
        objectResult
            .Value
            .Should()
            .BeOfType<List<StockDto>>();
    }*/
    /*var stockDto = new List<StockDto>()
        {
            new StockDto()
            {
                Id = 1,
                CompanyName = "Microsoft",
                Symbol = "MSFT",
                Purchase = 100,
                LastDiv = 100,
                Industry = "",
                MarketCap = 10000000,
                Comments = new List<CommentDto>()
            },
            new StockDto()
            {
                Id = 2,
                CompanyName = "Tesla",
                Symbol = "TSL",
                Purchase = 100,
                LastDiv = 100,
                Industry = "",
                MarketCap = 10000000,
                Comments = new List<CommentDto>()
            },
            
           
        };*/
    