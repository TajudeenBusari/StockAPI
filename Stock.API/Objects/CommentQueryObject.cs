namespace Stock.API.Objects;

public class CommentQueryObject
{
    public string Symbol { get; set; }
    public bool IsDescending { get; set; } = true;
}