
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Stock.API.Models;

namespace Stock.API.Data;

public class ApplicationDBContext: IdentityDbContext<AppUser>
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> dbContextOptions)
    :base(dbContextOptions)
    {
    }
    
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<Models.Stock> Stock { get; set; }
    public DbSet<Comment> Comments { get; set; }
    
    //seed atleaset one role first
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        List<IdentityRole> roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole
            {
                Name = "User",
                NormalizedName = "USER"
            }
        };
        builder.Entity<IdentityRole>().HasData(roles);
    }
    
}




/*We use IdentityDbContext instead of the DbContext
 because we are now implementing api security
 Register in the program.cs
 *System.Text
 * http://localhost:5239/swagger/index.html
*/