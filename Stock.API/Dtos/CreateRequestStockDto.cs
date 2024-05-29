namespace Stock.API.Dtos;

public class CreateRequestStockDto
{
    public string CompanyName { get; set; } = string.Empty;
    
    public string Symbol { get; set; } = string.Empty;
    
    public decimal Purchase { get; set; }
    
    public decimal LastDiv { get; set; } 
    
    public string Industry { get; set; } = string.Empty;
    
    public long MarketCap { get; set; }
}
//no comment and Id here
