namespace Stock.API.Dtos;

public class UpdateRequestCommentDto
{
    public string Title { get; set; } = string.Empty;
    
    public string Content { get; set; } = string.Empty;
}