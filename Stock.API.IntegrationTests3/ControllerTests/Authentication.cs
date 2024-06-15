using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using Stock.API.Dtos;

namespace Stock.API.IntegrationTests3.ControllerTests;

public class AppAuthentication
{
    protected readonly HttpClient testClient;

    public async Task AuthenticateAsync()
    {

        testClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("bearer", await GetJwtAsync());
    }

    private async Task<string> GetJwtAsync()
    {

        var response = await testClient.PostAsJsonAsync("/api/account/login", new LoginDto()
        {
            //username = "investorID",
            //Password = "Password@123",
            Username = "investorTJ",
            Password = "Password@123"
        });

        var regResponse = await response.Content.ReadAsStringAsync();
        //var jsonObj = new JsonObject();
        var jsonOBJ = JObject.Parse(regResponse);
        var token =jsonOBJ["token"]?.ToString();

        return token;
    }
    
}
