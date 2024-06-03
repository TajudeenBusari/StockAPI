namespace Stock.API.Objects;

public class QueryObject
{
    public string? Symbol { get; set; } = null;
    public string? CompanyName { get; set; } = null;
    
    public string? SortBy { get; set; } = null;
    
    public bool IsDescending { get; set; } = false;

}
/*In this class we will create properties:
 symbol and company name
 sortBy and ascending*/