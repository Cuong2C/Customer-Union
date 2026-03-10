using Asp.Versioning;
using Customer_Union.Application.Interfaces.Customers;
using Customer_Union.Application.Interfaces.Securities;
using Customer_Union.Configuration;
using Customer_Union.EndpointHandlers.ClientSourceHandlers;
using Customer_Union.EndpointHandlers.CustomerHandlers;
using Customer_Union.EndpointHandlers.Securities;
using Customer_Union.Infrastructure.ClientSources;
using Customer_Union.Infrastructure.Customers;
using Customer_Union.Infrastructure.Securities;
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

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddAuthentication("Client-Source").AddClientSourceAuthentication();

        builder.Services.AddScoped<IClientSourceRepository, ClientSourceRepository>();
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddScoped<ICustomerHistoryRepository, CustomerHistoryRepository>();

        builder.Services.AddScoped<IAddClientSource, AddClientSource>();
        builder.Services.AddScoped<IUpdateClientSource, UpdateClientSource>();
        builder.Services.AddScoped<IGetAllClientSource, GetAllClientSource>();
        builder.Services.AddScoped<IGetClientSourceByCode, GetClientSourceByCode>();
        builder.Services.AddScoped<IDeleteClientSource, DeleteClientSource>();

        builder.Services.AddScoped<IAddCustomer, AddCustomer>();
        builder.Services.AddScoped<IUpdateCustomer, UpdateCustomer>();
        builder.Services.AddScoped<IGetCustomers, GetCustomers>();
        builder.Services.AddScoped<IGetCustomerById, GetCustomerById>();
        builder.Services.AddScoped<IDeleteCustomer, DeleteCustomer>();
        builder.Services.AddScoped<ICheckNewVersionCustomer, CheckNewVersionCustomer>();
        builder.Services.AddScoped<IGetCustomerByPhone, GetCustomerByPhone>();
        builder.Services.AddScoped<IGetCustomerByTaxCode, GetCustomerByTaxCode>();
        builder.Services.AddScoped<IGetCustomerByPearlCode, GetCustomerByPearlCustomerCode>();

        builder.Services.AddScoped<ICreateClientSecret, CreateClientSecret>();
        builder.Services.AddScoped<IGenerateToken, GenerateToken>();
        builder.Services.AddScoped<IRevokeToken, RevokeToken>();

        builder.Services.AddScoped<AddClientSourceHandler>();
        builder.Services.AddScoped<UpdateClientSourceHandler>();
        builder.Services.AddScoped<GetAllClientSourceHandler>();
        builder.Services.AddScoped<GetClientSourceByCodeHandler>();
        builder.Services.AddScoped<DeleteClientSourceHandler>();

        builder.Services.AddScoped<AddCustomerHandler>();
        builder.Services.AddScoped<UpdateCustomerHandler>();
        builder.Services.AddScoped<GetCustomersHandler>();
        builder.Services.AddScoped<GetCustomerByIdHandler>();
        builder.Services.AddScoped<DeleteCustomerHandler>();
        builder.Services.AddScoped<CheckNewVersionCustomerHandler>();
        builder.Services.AddScoped<GetCustomerByPhoneHandler>();
        builder.Services.AddScoped<GetCustomerByTaxCodeHandler>();
        builder.Services.AddScoped<GetCustomerByPearlCustomerCodeHandler>();
        
        builder.Services.AddScoped<CreateClientSecretHandler>();    
        builder.Services.AddScoped<GenerateTokenHandler>();
        builder.Services.AddScoped<RevokeTokenHandler>();

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
