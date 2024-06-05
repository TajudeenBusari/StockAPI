using Stock.API.Dtos;
using Stock.API.Objects;

namespace Stock.API.Interfaces;

public interface IStockRepository
{
    //GET ALL
    Task<List<Models.Stock>> GetAllAsync(QueryObject queryObject);
    
    //GET A SINGLE STOCK BY ID
    Task<Models.Stock?> GetByIdAsync(int id); //will have firstOrDefault
    
    //GET A SINGLE STOCK BY SYMBOL
    Task<Models.Stock?> GetBySymbolAsync(string symbol);

    //CREATE A STOCK
    Task <Models.Stock>CreateAsync(Models.Stock stockModel);

    //UPDATE A STOCK
    Task <Models.Stock?>UpdateAsync(int id, UpdateRequestStockDto updateRequestStockDto);

    //DELETE A STOCK
    Task<Models.Stock?> DeleteAsync(int id);
    
    //CHECK IF STOCK EXISTS
    Task<bool> StockExists(int id);
}