using Stock.API.Dtos;

namespace Stock.API.Interfaces;

public interface IStockRepository
{
    //GET ALL
    Task<List<Models.Stock>> GetAllAsync();
    
    //GET A SINGLE STOCK
    Task<Models.Stock?> GetByIdAsync(int id); //will have firstOrDefault

    //CREATE A STOCK
    Task <Models.Stock>CreateAsync(Models.Stock stockModel);

    //UPDATE A STOCK
    Task <Models.Stock?>UpdateAsync(int id, UpdateRequestStockDto updateRequestStockDto);

    //DELETE A STOCK
    Task<Models.Stock?> DeleteAsync(int id);
    
    //CHECK IF STOCK EXISTS
    Task<bool> StockExists(int id);
}