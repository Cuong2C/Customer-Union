using Customer_Union.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Customer_Union.IntegrationTest;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var connectionString = "Server=localhost,1435;Database=TestDb;User Id=sa;Password=SqlDocker123;TrustServerCertificate=True";

            var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(IDbConnectionFactory));
            if (descriptor != null) services.Remove(descriptor);

            services.AddSingleton<IDbConnectionFactory>(new SqlServerConnectionFactory(connectionString));
            //services.AddScoped<IUnitOfWork, DapperUnitOfWork>();
        });
    }
}
