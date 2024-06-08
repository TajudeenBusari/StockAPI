using Stock.API.Dtos;
using Stock.API.Models;

namespace Stock.API.Mappers;

public class StockMappers
{
    //stock To stockDto
    public static StockDto ToStockDto( Models.Stock stockModel)
    {
        return new StockDto
        {
            Id = stockModel.Id,
            Symbol = stockModel.Symbol,
            CompanyName = stockModel.CompanyName,
            Purchase = stockModel.Purchase,
            LastDiv = stockModel.LastDiv,
            Industry = stockModel.Industry,
            MarketCap = stockModel.MarketCap,
            Comments = stockModel.Comments
                .Select(c => CommentMappers.MapFromCommentToCommentDto(c))
                .ToList()
            //c.MapFromCommentToCommentDto()
        };

    }
    
    //stockDto To stock
    public static Models.Stock ToStockFromStockDto( CreateRequestStockDto createRequestStockDto)
    {
        return new Models.Stock
        {
            Symbol = createRequestStockDto.Symbol,
            CompanyName = createRequestStockDto.CompanyName,
            Purchase = createRequestStockDto.Purchase,
            LastDiv = createRequestStockDto.LastDiv,
            Industry = createRequestStockDto.Industry,
            MarketCap = createRequestStockDto.MarketCap
        };
    }
    
    //From FMP To stock
    public static Models.Stock MapFromFMPStock(FMPStock fmpStock)
    {
        return new Models.Stock
        {
            Symbol = fmpStock.symbol,
            CompanyName = fmpStock.companyName,
            Purchase = (decimal)fmpStock.price,
            LastDiv = (decimal)fmpStock.lastDiv,
            Industry = fmpStock.industry,
            MarketCap = fmpStock.mktCap
        };

    }
    
}