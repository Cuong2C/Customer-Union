using Asp.Versioning;
using Customer_Union.Configuration;
using Customer_Union.Infrastructure.Securities.Auth;
using Microsoft.Extensions.Options;
using Serilog;

namespace Customer_Union.BootStraping;

public static class ApplicationServiceExtensions
{
    public static IHostApplicationBuilder AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();
        builder.Services.AddSerilog();

        var connectionString = builder.Configuration.GetConnectionString("SqlServerConnection") ?? string.Empty;

        builder.Services.AddScoped<IUnitOfWork, DapperUnitOfWork>();

        builder.Services.AddSingleton<IDbConnectionFactory>(new SqlServerConnectionFactory(connectionString));

        builder.Services.AddSingleton<ISqlServerValidatorHandler, SqlServerValidatorHandler>();

        builder.Services.AddSingleton<ITokenAuthenticationServices>(serviceProvider =>  
            new TokenAuthenticationServices(builder.Configuration["Authentication:IssuerSigningKey"]!, 
            serviceProvider.GetRequiredService<IDbConnectionFactory>())
        );

        builder.Services.AddSingleton<IPostConfigureOptions<ClientSourceAuthorizationHandlerOptions>, ClientSourceAuthorizationHandlerOptionSetup>();

        builder.Services.AddAuthentication("Client-Source").AddClientSourceAuthentication();

        builder.Services.AddScoped<IClientSourceRepository, ClientSourceRepository>();

        builder.Services.AddScoped<IClientSourceServices, ClientSourceServices>();

        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

        builder.Services.AddScoped<ICustomerServices, CustomerServices>();

        builder.Services.AddScoped<IAuthManagerHandlers, AuthManagerServices>();

        builder.Services.AddScoped<ICustomerHistoryRepository, CustomerHistoryRepository>();

        builder.Services.AddScoped<IClientCredentialRepository>(serviceProvider => 
            new ClientCredentialRepository(builder.Configuration["Authentication:Client-Secret-Hash-Key"]!, 
            serviceProvider.GetRequiredService<IUnitOfWork>())
        );

        builder.Services.AddAuthorization();

        builder.Services.AddAutoMapper(typeof(MapperConfig));

        builder.Services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("x-version")
            );
        });

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");

        });

        builder.Services.AddHealthChecks()
            .AddSqlServer(connectionString)
            .AddRedis(builder.Configuration.GetConnectionString("RedisConnection")!);

        return builder;
    }
}
