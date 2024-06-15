namespace Stock.API.IntegrationTests2.Fixtures;
using Stock.API.Dtos;
using Stock.API.Models;

public class StockFixtures
{
    public static List<Stock> GetTestStocks() => new()
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
        
        new Stock()
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
}