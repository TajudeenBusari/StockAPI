namespace Stock.API.Objects;

public class QueryObject
{
    public string? Symbol { get; set; } = null;
    public string? CompanyName { get; set; } = null;
    
    public string? SortBy { get; set; } = null;
    
    public bool IsDescending { get; set; } = false;

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 20;

}
/*In this class we will create properties:
 symbol and company name
 sortBy and ascending
 pageNumber*/