using Microsoft.AspNetCore.Authentication;

namespace Customer_Union.Authorization;

public static class ClientSourceAuthorizationHandlerExtensions
{
    public static AuthenticationBuilder AddClientSourceAuthentication(this AuthenticationBuilder builder)
    {
        builder.AddScheme<ClientSourceAuthorizationHandlerOptions, ClientSourceAuthorizationHandler>("Client-Source", _ => { });
        return builder;
    }
}
