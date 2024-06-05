using Stock.API.Dtos;
using Stock.API.Models;

namespace Stock.API.Mappers;

public static class CommentMappers
{
    //Comment to commentDto
    public static CommentDto MapFromCommentToCommentDto(this Comment commentModel)
    {
        return new CommentDto
        {
            Id = commentModel.Id,
            Title = commentModel.Title,
            Content = commentModel.Content,
            CreatedOn = commentModel.CreatedOn,
            CreatedBy = commentModel.AppUser.UserName,
            StockId = commentModel.StockId
        };
    }
    
    //CreateCommentDto to comment
    public static Comment MapFromCreateCommentDtoToComment(this CreateCommentDto createCommentDto, int stockId)
    {
        return new Comment
        {
            Title = createCommentDto.Title,
            Content = createCommentDto.Content,
            StockId = stockId
        };
    }
    
    //UpdateCommentDto To comment
    public static Comment MapFromUpdateCommentDtoToComment(this UpdateRequestCommentDto updateRequestCommentDto)
    {
        return new Comment
        {
            Title = updateRequestCommentDto.Title,
            Content = updateRequestCommentDto.Content
        };
    }
}