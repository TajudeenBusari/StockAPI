using Stock.API.Dtos;

namespace Stock.API.IntegrationTest4;

public class IntegrationTest4Helper
{
    public static List<StockDto> GetStockDtos() => new()
    {
        new StockDto
        {
            Id= 1,
            CompanyName = "Microsoft",
            Symbol = "MSFT",
            Purchase = 100,
            LastDiv = 90,
            Industry = "Software",
            MarketCap = 1000000000,

        },
        new StockDto()
        {
            Id = 2,
            CompanyName = "Microsoft1",
            Symbol = "MSFT1",
            Purchase = 1001,
            LastDiv = 901,
            Industry = "Software1",
            MarketCap = 10000000001,

        }
    };
    
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
    
    public static UpdateRequestStockDto TestUpdateStock()
    {
        return new UpdateRequestStockDto()
        {
            
            CompanyName = "TJ Logis",
            Symbol = "TJL",
            Purchase = 100,
            LastDiv = 100,
            Industry = "Logistics",
            MarketCap = 1000000000,

        };
    } 
    
    public static StockDto TestStockDto()
    {
        return new StockDto()
        {
            Id = 1,
            CompanyName = "TJ Logis",
            Symbol = "TJL",
            Purchase = 100,
            LastDiv = 100,
            Industry = "Logistics",
            MarketCap = 1000000000,

        };
    } 

}