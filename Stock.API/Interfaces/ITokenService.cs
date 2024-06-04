using Stock.API.Models;

namespace Stock.API.Interfaces;

public interface ITokenService
{
    string CreateToken(AppUser user);
}