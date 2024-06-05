
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

    public DbSet<Portfolio> Portfolios { get; set; }
    public DbSet<Models.Stock> Stock { get; set; }
    public DbSet<Comment> Comments { get; set; }
    
    
    //seed atleaset one role first
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        //build foreign key
        builder.Entity<Portfolio>(x => x.HasKey(p => new
        {
            p.AppUserId, p.StockId
        }));
        
        // portfolios with appUser
        builder.Entity<Portfolio>()
            .HasOne(u => u.AppUser)
            .WithMany(u => u.Portfolios)
            .HasForeignKey(p => p.AppUserId);
        
        // portfolios with stock
        builder.Entity<Portfolio>()
            .HasOne(u => u.Stock)
            .WithMany(u => u.Portfolios)
            .HasForeignKey(p => p.StockId);
        
        //build role
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
 * 
 *IMPORTANT NOTE
 * We have changed the table name and do
 * many to many relationship, it is good to delete
 * migration folder and the database to start afresh
 * NB: Don't do in a work environment where other
 * software developers are working wth you
*/