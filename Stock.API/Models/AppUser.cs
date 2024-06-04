using Microsoft.AspNetCore.Identity;

namespace Stock.API.Models;

public class AppUser: IdentityUser
{
    //many to many
    public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
}
