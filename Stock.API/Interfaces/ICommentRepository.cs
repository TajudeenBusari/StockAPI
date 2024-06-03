using Stock.API.Models;
using Stock.API.Objects;

namespace Stock.API.Interfaces;

public interface ICommentRepository
{
    //GET ALL
    Task<List<Comment>> GetAllAsync();
    
    //GET BY ID
    Task<Comment?> GetByIdAsync(int id);
    
    //CREATE
    Task<Comment> CreateAsync(Comment commentModel);
    
    //UPDATE
    Task<Comment?> UpdateAsync(int id, Comment commentModel);
    
    //DELETE
    Task<Comment?> DeleteAsync(int id);
}