namespace Stock.API.Dtos;


//instead of just returning string user created, we will return this object

public class NewUserDto
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    
}
