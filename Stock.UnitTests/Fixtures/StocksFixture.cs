using Stock.API.Dtos;
using Stock.API.Models;

namespace Stock.UnitTests.Fixtures;

public static class StocksFixture
{
    public static List<API.Models.Stock> GetTestStocks() => new()
    {
        new API.Models.Stock()
        {
            Id = 1,
            CompanyName = "Microsoft",
            Symbol = "MSFT",
            Purchase = 100,
            LastDiv = 90,
            Industry = "Software",
            MarketCap = 1000000000,
            Comments = new List<Comment>(),
            Portfolios = new List<Portfolio>()
            

        },
        
        new API.Models.Stock()
        {
            Id = 2,
            CompanyName = "Tesla",
            Symbol = "TSL",
            Purchase = 100,
            LastDiv = 90,
            Industry = "Automobile",
            MarketCap = 100000000,
            Comments = new List<Comment>(),
            Portfolios = new List<Portfolio>()
           

        }
    };

    public static List<API.Models.Stock> GetTestStocksEmpty()
    {
        return new List<API.Models.Stock>();
    }

    public static CreateRequestStockDto TestAddStock()
    {
        return new CreateRequestStockDto
        {
            CompanyName = "TJ Logis",
            Symbol = "TJL",
            Purchase = 100,
            LastDiv = 100,
            Industry = "Logistics",
            MarketCap = 1000000000,

        };
    }

    public static API.Models.Stock TestAStockModelById()
    {
        return new API.Models.Stock
        {
            Id = 3,
            CompanyName = "Nvidia",
            Symbol = "NVD",
            Purchase = 100,
            LastDiv = 90,
            Industry = "Software/AI",
            MarketCap = 100000000,
            Comments = new List<Comment>(),
            Portfolios = new List<Portfolio>()

        };
    }
    
    public static UpdateRequestStockDto TestUpdateStock()
    {
        return new UpdateRequestStockDto
        {
           
            CompanyName = "TJ Logis",
            Symbol = "TJL",
            Purchase = 100,
            LastDiv = 100,
            Industry = "Logistics",
            MarketCap = 1000000000,

        };
    }

}