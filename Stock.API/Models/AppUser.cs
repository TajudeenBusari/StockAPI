using Microsoft.AspNetCore.Identity;

namespace Stock.API.Models;

public class AppUser: IdentityUser
{
    //public string Name { get; set; }
}
//this class can be empty, all user prop are inherited from IdentityUser