using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace Customer_Union.BootStraping;

public class ClientSourceAuthorizationHandlerOptionSetup : IPostConfigureOptions<ClientSourceAuthorizationHandlerOptions>
{
    private readonly ISqlServerValidatorHandler _handler;
    private readonly IConfiguration _config;

    public ClientSourceAuthorizationHandlerOptionSetup(ISqlServerValidatorHandler handler, IConfiguration config)
    {
        _handler = handler;
        _config = config;
    }

    public void PostConfigure(string? name, ClientSourceAuthorizationHandlerOptions options)
    {
        options.ClientValidator = (clientSource, token, principal) => 
        {
            var jti = principal.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            if(string.IsNullOrEmpty(jti))
            {
                return false;
            }

            return _handler.Validate(clientSource, jti);
        };
        options.IssuerSigningKey = _config["Authentication:IssuerSigningKey"] ?? string.Empty;
    }

}
