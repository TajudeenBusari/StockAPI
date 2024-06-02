using System.ComponentModel.DataAnnotations;

namespace Stock.API.Dtos;

public class CreateCommentDto
{
    [Required]
    [MinLength(5, ErrorMessage = "Title must be 5 characters")]
    [MaxLength(250, ErrorMessage = "Title cannot be over 250 characters")]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [MinLength(5, ErrorMessage = "Content must be 5 characters")]
    [MaxLength(250, ErrorMessage = "Content cannot be over 250 characters")]
    public string Content { get; set; } = string.Empty;
    
    
    
}


/*for client to create a comment,only tittle and
 content is needed, so these two are only 
 exposed to the client*/