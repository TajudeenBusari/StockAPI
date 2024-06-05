using Stock.API.Models;

namespace Stock.API.Interfaces;

public interface IPortfolioRepository
{
    Task<List<Models.Stock>> GetUserPortfolio(AppUser user);
}