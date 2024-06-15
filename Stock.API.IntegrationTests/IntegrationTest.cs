using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Stock.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Linq;
using Stock.API.Dtos;

namespace Stock.API.IntegrationTests;

public class IntegrationTest
{
    protected readonly HttpClient testClient;

    //protected readonly IWebHostBuilder WebHostBuilder;
    protected IntegrationTest()
    {

        var appFactory = new WebApplicationFactory<Program>()
            
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll(typeof(ApplicationDBContext));
                    services.AddDbContext<ApplicationDBContext>(options =>
                    {
                        options.UseInMemoryDatabase("testDB");
                    });

                });
            });
        testClient = appFactory.CreateClient();
    }

    protected async Task AuthenticateAsync()
    {
        testClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("bearer", await GetJwtAsync("investorTJ", "Password@123", "investor@yahoo.com"));
    }

   

    private async Task<string> GetJwtAsync(string username, string password, string email)
    {

        var user = new RegisterDto()
        {
            Username = username,
            Password = password,
            Email = email
        };
        
        //var res = await testClient.PostAsJsonAsync("/api/account/register", user);
        var httpContent = new StringContent(JsonConvert.SerializeObject(user)
            , Encoding.UTF8, "application/json");
        
        var res = await testClient.PostAsync("/api/account/register", httpContent
            );

       
        var model = await res.Content.ReadFromJsonAsync<NewUserDto>();
        
        return model.Token;
    }
} 
