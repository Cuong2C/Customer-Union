using Longbeach_Customer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OutputCaching;

namespace Longbeach_Customer.Apis;

public static class LongbeachApi
{
    public static IEndpointRouteBuilder MapLongbeachApi(this IEndpointRouteBuilder endpoints)
    {
        var vApi = endpoints.NewVersionedApi("Longbeach");
        var v1 = vApi.MapGroup("/api/v{version:apiVersion}/longbeach").HasApiVersion(1, 0);

        // Client Source APIs
        v1.MapGet("/client-sources", [Authorize] (IClientSourceServices clientSourceServices) => clientSourceServices.GetAllClientSourcesAsync());
        v1.MapGet("/client-sources/{clientSourceCode}",
            [Authorize](IClientSourceServices clientSourceServices, string clientSourceCode) => clientSourceServices.GetClientSourceByCodeAsync(clientSourceCode));
        v1.MapPost("/client-sources", 
            [AllowAnonymous] (IClientSourceServices clientSourceServices, ClientSource clientSource) => clientSourceServices.AddClientSourceAsync(clientSource));
        v1.MapPut("/client-sources", 
            [AllowAnonymous] (IClientSourceServices clientSourceServices, ClientSource clientSource) => clientSourceServices.UpdateClientSourceAsync(clientSource));
        v1.MapDelete("/client-sources/{clientCode}", 
            [AllowAnonymous] (IClientSourceServices clientSourceServices, string clientCode) => clientSourceServices.DeleteClientSourceAsync(clientCode));

        // Customer APIs
        v1.MapGet("/customers/customer-phone/{phoneNumber}", 
            [Authorize] (ICustomerServices customerServices, string phoneNumber) => customerServices.GetCustomerByPhoneAsync(phoneNumber));
        v1.MapGet("/customers/customer-taxcode/{taxCode}", 
            [Authorize] (ICustomerServices customerServices, string taxCode) => customerServices.GetCustomerByTaxcodeAsync(taxCode));
        v1.MapGet("/customers/customer-pearl/{pearlCustomerCode}",
            [Authorize] (ICustomerServices customerServices, string pearlCustomerCode) => customerServices.GetCustomerByPearlCustomerCodeAsync(pearlCustomerCode));
        v1.MapGet("/customers", 
            [Authorize] (ICustomerServices CustomerServices, DateTime? cursorDate, Guid? cursorId, int pageSize = 20, string direction = "next") => CustomerServices.GetCustomersAsync(cursorDate, cursorId, pageSize, direction)); 
        v1.MapGet("/customers/{id}", 
            [Authorize] (ICustomerServices customerServices, Guid id) => customerServices.GetCustomerByIdAsync(id));
        v1.MapGet("/customers/{id}/{hashCode}",
            [Authorize] (ICustomerServices customerServices, Guid id, string hashCode) => customerServices.IsNewVersionCustomerAsync(id, hashCode));
        v1.MapPost("/customers",
            [Authorize](HttpContext httpContext, CustomerRequest customerRequest, ICustomerServices customerServices) => customerServices.AddCustomerAsync(httpContext, customerRequest));
        v1.MapPut("/customers/{id}",
            [Authorize] (HttpContext httpContext, CustomerRequest customerRequest, ICustomerServices customerServices, Guid id) => customerServices.UpdateCustomerAsync(httpContext, customerRequest, id));
        v1.MapDelete("/customers/{id}",
            [Authorize] (HttpContext httpContext, Guid id, ICustomerServices customerServices) => customerServices.DeleteCustomerAsync(httpContext, id));

        // AuthManager APIs
        v1.MapPost("/auth/tokens",
            [AllowAnonymous] (GenrateTokenRequest genrateTokenRequest, IAuthManagerHandlers authManagerServices) => authManagerServices.GenerateTokenAsync(genrateTokenRequest)); 
        v1.MapPost("/auth/tokens/revoke",
            [Authorize] (RevokeTokenRequest revokeTokenRequest, IAuthManagerHandlers authManagerServices, HttpContext httpContext) => authManagerServices.RevokeTokenAsync(revokeTokenRequest, httpContext));
        v1.MapPost("/auth/client-secrets",
            [AllowAnonymous] (CreateClientSecretRequest clientSecretRequest, IAuthManagerHandlers authManagerServices) => authManagerServices.CreateClientSecretAsync(clientSecretRequest)); 

        return endpoints;
    }

}
