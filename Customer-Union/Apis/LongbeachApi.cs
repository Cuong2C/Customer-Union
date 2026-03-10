using Customer_Union.EndpointHandlers.ClientSourceHandlers;
using Customer_Union.EndpointHandlers.CustomerHandlers;
using Customer_Union.EndpointHandlers.Securities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Customer_Union.Apis;

public static class LongbeachApi
{
    public static IEndpointRouteBuilder MapLongbeachApi(this IEndpointRouteBuilder endpoints)
    {
        var vApi = endpoints.NewVersionedApi("Longbeach");
        var v1 = vApi.MapGroup("/api/v{version:apiVersion}/longbeach").HasApiVersion(1, 0);

        // Client Source APIs
        v1.MapGet("/client-sources", [Authorize] ([FromServices] GetAllClientSourceHandler handler) => handler.GetAllClientSourcesAsync());
        v1.MapGet("/client-sources/{clientSourceCode}",
            [Authorize] ([FromServices] GetClientSourceByCodeHandler handler, string clientSourceCode) => handler.GetClientSourceByCodeAsync(clientSourceCode));
        v1.MapPost("/client-sources",
            [AllowAnonymous] ([FromServices] AddClientSourceHandler handler, ClientSourceRequest clientSourceRequest) => handler.AddClientSourceAsync(clientSourceRequest));
        v1.MapPut("/client-sources",
            [AllowAnonymous] ([FromServices] UpdateClientSourceHandler handler, ClientSourceRequest clientSourceRequest) => handler.UpdateClientSourceAsync(clientSourceRequest));
        v1.MapDelete("/client-sources/{clientCode}",
            [AllowAnonymous] ([FromServices] DeleteClientSourceHandler handler, string clientCode) => handler.DeleteClientSourceAsync(clientCode));

        // Customer APIs
        v1.MapGet("/customers/customer-phone/{phoneNumber}",
            [Authorize] ([FromServices] GetCustomerByPhoneHandler handler, string phoneNumber) => handler.GetCustomerByPhoneAsync(phoneNumber));
        v1.MapGet("/customers/customer-taxcode/{taxCode}",
            [Authorize] ([FromServices] GetCustomerByTaxCodeHandler handler, string taxCode) => handler.GetCustomerByTaxCodeAsync(taxCode));
        v1.MapGet("/customers/customer-pearl/{pearlCustomerCode}",
            [Authorize] ([FromServices] GetCustomerByPearlCustomerCodeHandler handler, string pearlCustomerCode) => handler.GetCustomerByPearlCustomerCodeAsync(pearlCustomerCode));
        v1.MapGet("/customers",
            [Authorize] ([FromServices] GetCustomersHandler handler, DateTime? cursorDate, Guid? cursorId, int pageSize = 20, string direction = "next")
                => handler.GetCustomersAsync(cursorDate, cursorId, pageSize, direction));
        v1.MapGet("/customers/{id}",
            [Authorize] ([FromServices] GetCustomerByIdHandler handler, Guid id) => handler.GetCustomerByIdAsync(id));
        v1.MapGet("/customers/{id}/{hashCode}",
            [Authorize] ([FromServices] CheckNewVersionCustomerHandler handler, Guid id, string hashCode) => handler.IsNewVersionCustomerAsync(id, hashCode));
        v1.MapPost("/customers",
            [Authorize] ([FromServices] AddCustomerHandler handler, HttpContext httpContext, CustomerRequest customerRequest) => handler.AddCustomerAsync(httpContext, customerRequest));
        v1.MapPut("/customers/{id}",
            [Authorize] ([FromServices] UpdateCustomerHandler handler, HttpContext httpContext, CustomerRequest customerRequest, Guid id) => handler.UpdateCustomerAsync(httpContext, customerRequest, id));
        v1.MapDelete("/customers/{id}",
            [Authorize] ([FromServices] DeleteCustomerHandler handler, HttpContext httpContext, Guid id) => handler.DeleteCustomerAsync(httpContext, id));

        // AuthManager APIs
        v1.MapPost("/auth/tokens",
            [AllowAnonymous] ([FromServices] GenerateTokenHandler handler, GenrateTokenRequest genrateTokenRequest) => handler.GenerateTokenAsync(genrateTokenRequest));
        v1.MapPost("/auth/tokens/revoke",
            [Authorize] ([FromServices] RevokeTokenHandler handler, RevokeTokenRequest revokeTokenRequest) => handler.RevokeTokenAsync(revokeTokenRequest));
        v1.MapPost("/auth/client-secrets",
            [AllowAnonymous] ([FromServices] CreateClientSecretHandler handler, CreateClientSecretRequest clientSecretRequest) => handler.CreateClientSecretAsync(clientSecretRequest));                           

        return endpoints;
    }

}
