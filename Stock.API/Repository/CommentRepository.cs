using Microsoft.EntityFrameworkCore;
using Stock.API.Data;
using Stock.API.Interfaces;
using Stock.API.Models;
using Stock.API.Objects;

namespace Stock.API.Repository;

public class CommentRepository: ICommentRepository
{
    //Constructor
    private readonly ApplicationDBContext _context;
    public CommentRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    
    //GET ALL COMMENTS
    public async Task<List<Comment>> GetAllAsync()
    {
        return await _context.Comments.ToListAsync();
    }

    //GET BY ID
    public async Task<Comment?> GetByIdAsync(int id)
    {
        return await _context
            .Comments
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    //CREATE
    public async Task<Comment> CreateAsync(Comment commentModel)
    {
        await _context.Comments.AddAsync(commentModel);
        await _context.SaveChangesAsync();
        return commentModel;
    }

    //UPDATE
    public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
    {
        var existingCommentModel = await _context
            .Comments
            .FirstOrDefaultAsync(x => x.Id == id);
        if (existingCommentModel == null)
        {
            return null;
        }
        //else update only Title and content
        existingCommentModel.Title = commentModel.Title;
        existingCommentModel.Content = commentModel.Content;
        await _context.SaveChangesAsync();
        return existingCommentModel;
    }

    //DELETE
    public async Task<Comment?> DeleteAsync(int id)
    {
        var existingCommentModel = await _context
            .Comments
            .FirstOrDefaultAsync(x => x.Id == id);
        if (existingCommentModel == null)
        {
            return null;
        }

        _context.Remove(existingCommentModel);
        await _context.SaveChangesAsync();
        return existingCommentModel;
    }
}