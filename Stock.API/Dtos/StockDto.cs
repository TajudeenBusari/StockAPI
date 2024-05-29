namespace Stock.API.Dtos;

public class StockDto
{
    public int Id { get; set; }
    
    public string CompanyName { get; set; } = string.Empty;
    
    public string Symbol { get; set; } = string.Empty;
    
    
    public decimal Purchase { get; set; }
    
    
    public decimal LastDiv { get; set; }
    
    public string Industry { get; set; } = string.Empty;
    
    public long MarketCap { get; set; }
    
    //one to many
    //we don't want to return any comment to the client
    /*public List<Comment> Comments { get; set; } = new List<Comment>();*/
}