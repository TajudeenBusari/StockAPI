
using Microsoft.EntityFrameworkCore;
using Stock.API.Models;

namespace Stock.API.Data;

public class ApplicationDBContext: DbContext
{
    public ApplicationDBContext(DbContextOptions dbContextOptions)
    :base(dbContextOptions)
    {
        
    }
    public DbSet<Models.Stock> Stock { get; set; }
    public DbSet<Comment> Comments { get; set; }
}