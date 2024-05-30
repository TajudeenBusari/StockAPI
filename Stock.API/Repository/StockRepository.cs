using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Stock.API.Data;
using Stock.API.Dtos;
using Stock.API.Interfaces;

namespace Stock.API.Repository;

public class StockRepository: IStockRepository
{
    //constructor that has applicationDBContext as param
    private readonly ApplicationDBContext _context;
    public StockRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    
    //GET ALL
    public async Task<List<Models.Stock>> GetAllAsync()
    {
        return await _context.Stock.ToListAsync();
    }
    
    //GET A SINGLE
    public async Task<Models.Stock?> GetByIdAsync(int id)
    {
        return await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);
    }
    
    //CREATE STOCK
    public async Task<Models.Stock> CreateAsync(Models.Stock stockModel)
    {
        await _context.Stock.AddAsync(stockModel);
        await _context.SaveChangesAsync();
        return stockModel;

    }

    //UPDATE STOCK
    public async Task<Models.Stock?> UpdateAsync(int id, UpdateRequestStockDto updateRequestStockDto)
    {
       var existingStock = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);
       if (existingStock == null)
       {
           return null;
       }
       
       existingStock.CompanyName = updateRequestStockDto.CompanyName;
       existingStock.Symbol = updateRequestStockDto.Symbol;
       existingStock.Purchase = updateRequestStockDto.Purchase;
       existingStock.LastDiv = updateRequestStockDto.LastDiv;
       existingStock.Industry = updateRequestStockDto.Industry;
       existingStock.MarketCap = updateRequestStockDto.MarketCap;

       await _context.SaveChangesAsync();
       return existingStock;
    }

    //DELETE A STOCK
    public async Task<Models.Stock?> DeleteAsync(int id)
    {
        var existingStockModel = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);
        if (existingStockModel == null)
        {
            return null;
        }

        _context.Stock.Remove(existingStockModel);
        await _context.SaveChangesAsync();
        return existingStockModel;
    }
}