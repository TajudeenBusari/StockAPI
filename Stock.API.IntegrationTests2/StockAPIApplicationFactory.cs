using System.Collections.Immutable;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Stock.API.Data;

namespace Stock.API.IntegrationTests2;

internal class StockAPIApplicationFactory: WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ApplicationDBContext>));
            var connectionString = GetConnectionString();
            services.AddSqlServer<ApplicationDBContext>(connectionString);
            services.AddAuthentication("TestScheme")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });
            var dbContext = createDbContext(services);
            dbContext.Database.EnsureDeleted();

        });
    }

    private static string? GetConnectionString()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<StockAPIApplicationFactory>()
            .Build();
        var conString = configuration.GetConnectionString("StockAPI");
        return conString;

    }

    private static ApplicationDBContext createDbContext(IServiceCollection serviceCollections)
    {
        var serviceProvider = serviceCollections.BuildServiceProvider();
        var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
        return dbContext;
    }
}