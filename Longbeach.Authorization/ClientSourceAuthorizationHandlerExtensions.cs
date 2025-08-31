using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Longbeach.Authorization;

public static class ClientSourceAuthorizationHandlerExtensions
{
    public static AuthenticationBuilder AddClientSourceAuthentication(this AuthenticationBuilder builder)
    {
        builder.AddScheme<ClientSourceAuthorizationHandlerOptions, ClientSourceAuthorizationHandler>("Client-Source", _ => { });
        return builder;
    }
}
