using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Stock.API.Data;
using Stock.API.Dtos;
using Stock.API.Interfaces;
using Stock.API.Objects;

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
    public async Task<List<Models.Stock>> GetAllAsync(QueryObject queryObject)
    {
        /*without using query param:
         return await _context
            .Stock
            .Include(c => c.Comments)
            .ToListAsync();*/
        
       var stocks = _context
           .Stock
           .Include(c => c.Comments)
           .ThenInclude(a => a.AppUser)
           .AsQueryable();
       
       //filtering
       if (!string.IsNullOrWhiteSpace(queryObject.CompanyName))
       {
           stocks = stocks
               .Where(s => s.CompanyName.Contains(queryObject.CompanyName));
       }

       if (!string.IsNullOrWhiteSpace(queryObject.Symbol))
       {
           stocks = stocks
               .Where(s => s.Symbol.Contains(queryObject.Symbol));
       }
       //sorting
       if (!string.IsNullOrWhiteSpace(queryObject.SortBy))
       {
           //sort by symbol or anyone you wish
           if (queryObject.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
           {
               stocks = queryObject.IsDescending
                   ? stocks.OrderByDescending(s => s.Symbol)
                   : stocks.OrderBy(s => s.Symbol);
           }
       }
       //pagination
       var skipNumber = (queryObject.PageNumber - 1) * queryObject.PageSize;
       

       return await stocks.Skip(skipNumber).Take(queryObject.PageSize).ToListAsync();
    }
    
    //GET A SINGLE BY ID
    public async Task<Models.Stock?> GetByIdAsync(int id)
    {
        return await _context
            .Stock
            .Include(c => c.Comments)
            .ThenInclude(a => a.AppUser)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    //GET A SINGLE BY SYMBOL
    public async Task<Models.Stock?> GetBySymbolAsync(string symbol)
    {
        return await _context
            .Stock
            .FirstOrDefaultAsync(s => s.Symbol == symbol);
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

    //CHECK IF Stock exists
    public Task<bool> StockExists(int id)
    {
        return _context
            .Stock
            .AnyAsync(s => s.Id == id);
    }
}