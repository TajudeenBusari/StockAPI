using Stock.API.Dtos;
using Stock.API.Models;

namespace Stock.API.Mappers;

public static class StockMappers
{
    //stock To stockDto
    public static StockDto ToStockDto(this Models.Stock stockModel)
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
                .Select(c => c.MapFromCommentToCommentDto())
                .ToList()
        };

    }
    
    //stockDto To stock
    public static Models.Stock ToStockFromStockDto(this CreateRequestStockDto createRequestStockDto)
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
    
}