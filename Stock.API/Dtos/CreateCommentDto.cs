namespace Stock.API.Dtos;

public class CreateCommentDto
{
    public string Title { get; set; } = string.Empty;
    
    public string Content { get; set; } = string.Empty;
    
    
    
}


/*for client to create a comment,only tittle and
 content is needed, so these two are only 
 exposed to the client*/