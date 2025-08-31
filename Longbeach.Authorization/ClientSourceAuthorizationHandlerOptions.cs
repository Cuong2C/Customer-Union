using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Longbeach.Authorization;

public class ClientSourceAuthorizationHandlerOptions : AuthenticationSchemeOptions
{
    public Func<string, SecurityToken, ClaimsPrincipal, bool> ClientValidator { get; set; } = (clientId, token, principal) => false;
    public string IssuerSigningKey { get; set; } = string.Empty;
}
