namespace Stock.API.Interfaces;

public interface IFMPService
{
    Task<Models.Stock> FindStockBySymbolAsync(string symbol);
}


//Financial Modeling Prep == FMP