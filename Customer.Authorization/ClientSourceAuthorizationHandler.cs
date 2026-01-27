using Customer_Union.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace Customer_Union.Authorization;

public class ClientSourceAuthorizationHandler : AuthenticationHandler<ClientSourceAuthorizationHandlerOptions>
{
    private readonly ITokenAuthenticationServices _tokenServices;
    public ClientSourceAuthorizationHandler(IOptionsMonitor<ClientSourceAuthorizationHandlerOptions> options, 
        ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, ITokenAuthenticationServices tokenServices)
        : base(options, logger, encoder, clock)
    {
        _tokenServices = tokenServices;
    }
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var clientSource = Context.Request.Headers["Client-Source"];
        var token = Context.Request.Headers["Token"];

        if (clientSource.Count == 0)
        {
            return Task.FromResult(AuthenticateResult.Fail("Missing Client-Source"));
        }

        if (token.Count == 0)
        {
            return Task.FromResult(AuthenticateResult.Fail("Missing token header"));
        }

        var clientSourceValue = clientSource.FirstOrDefault();
        var tokenValue = token.FirstOrDefault();

        if (!string.IsNullOrEmpty(clientSource) && !string.IsNullOrEmpty(tokenValue) && VerifyClient(clientSourceValue, tokenValue, out var principal))
        {
            ((ClaimsIdentity)principal!.Identity!).AddClaim(new Claim("ClientSourceCode", clientSourceValue!));
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        return Task.FromResult(AuthenticateResult.Fail("Invalid token"));
    }

    private bool VerifyClient(string? clientSourceValue, string tokenValue, out ClaimsPrincipal? principal)
    {
        if (!_tokenServices.ValidateToken(tokenValue, out var token, out principal))
        {
            return false;
        }

        var sub = (token as JwtSecurityToken)!.Subject;

        if(clientSourceValue != sub)
        {
            return false;
        }

        return Options.ClientValidator(clientSourceValue, token!, principal!);
    }

   
}
