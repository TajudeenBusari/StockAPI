using Microsoft.EntityFrameworkCore;
using Stock.API.Data;
using Stock.API.Interfaces;
using Stock.API.Models;

namespace Stock.API.Repository;

public class PortfolioRepository: IPortfolioRepository
{
    private readonly ApplicationDBContext _context;
    public PortfolioRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    
    //GET PORTFOLIO
    public async Task<List<Models.Stock>> GetUserPortfolio(AppUser user)
    {
        //filter all portfolios by AppUserId
        return
           await  _context
               .Portfolios
               .Where(u => u.AppUserId == user.Id)
                .Select(stock => new Models.Stock
                {
                    Id = stock.StockId,
                    Symbol = stock.Stock.Symbol,
                    CompanyName = stock.Stock.CompanyName,
                    Purchase = stock.Stock.Purchase,
                    LastDiv = stock.Stock.LastDiv,
                    Industry = stock.Stock.Industry,
                    MarketCap = stock.Stock.MarketCap
                    
                }).ToListAsync();
    }

    //CREATE PORTFOLIO
    public async Task<Portfolio> CreateUserPortfolioAsync(Portfolio portfolio)
    {
        await _context.Portfolios.AddAsync(portfolio);
        await _context.SaveChangesAsync();
        return portfolio;
    }

    //DELETE PORTFOLIO
    public async Task<Portfolio> DeleteUserPortfolioAsync(AppUser appUser, string symbol)
    {
        var portfolioModel = await _context
            .Portfolios
            .FirstOrDefaultAsync(x =>
            x.AppUserId == appUser.Id && x.Stock.Symbol.ToLower() == symbol.ToLower());

        if (portfolioModel == null)
        {
            return null;
        }

        _context.Portfolios.Remove(portfolioModel);
        await _context.SaveChangesAsync();
        return portfolioModel;
    }
}