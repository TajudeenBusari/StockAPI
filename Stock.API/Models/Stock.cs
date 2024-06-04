using System.ComponentModel.DataAnnotations.Schema;

namespace Stock.API.Models;
[Table("Stocks")]
public class Stock
{
    public int Id { get; set; }
    
    public string CompanyName { get; set; } = string.Empty;
    
    public string Symbol { get; set; } = string.Empty;
    
    [Column(TypeName = "decimal(18,2)")] 
    public decimal Purchase { get; set; }
    
    [Column(TypeName = "decimal(18,2)")] 
    public decimal LastDiv { get; set; }
    
    public string Industry { get; set; } = string.Empty;
    
    public long MarketCap { get; set; }
    //one to many
    public List<Comment> Comments { get; set; } = new List<Comment>();
    
    //many to many
    public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
}