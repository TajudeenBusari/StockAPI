using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Stock.API.Interfaces;

namespace Stock.API.IntegrationTests3.ControllerTests;

internal class CustomWebApplicationFactory: WebApplicationFactory<Program>
{
    public Mock<IStockRepository> MockstockRepository { get; }

    public CustomWebApplicationFactory()
    {
        MockstockRepository = new Mock<IStockRepository>();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureTestServices(services =>
        {
            services.AddSingleton(MockstockRepository.Object);
            services.AddAuthentication();

        });
    }
}