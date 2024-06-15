using Stock.API.Dtos;

namespace Stock.API.IntegrationTests;

public class Helper
{
    public static List<StockDto> GetStockDtos() => new()
    {
        new StockDto
        {
            CompanyName = "Microsoft",
            Symbol = "MSFT",
            Purchase = 100,
            LastDiv = 90,
            Industry = "Software",
            MarketCap = 1000000000,

        },
        new StockDto()
        {
            CompanyName = "Microsoft1",
            Symbol = "MSFT1",
            Purchase = 1001,
            LastDiv = 901,
            Industry = "Software1",
            MarketCap = 10000000001,

        }
    };
}