using Customer_Union.Application.Interfaces.Securities;
using System.IdentityModel.Tokens.Jwt;

namespace CustomerUnion.EndpointHandlers.Securities;

public class RevokeTokenHandler(IRevokeToken revokeToken)
{
    public async Task<Results<Ok, BadRequest>> RevokeTokenAsync(RevokeTokenRequest revokeTokenRequest, HttpContext httpContext)
    {
        if (string.IsNullOrWhiteSpace(revokeTokenRequest.ClientCode) || string.IsNullOrWhiteSpace(revokeTokenRequest.ClientSecret))
        {
            return TypedResults.BadRequest();
        }

        var jti = httpContext.User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

        var result = await revokeToken.RevokeTokenAsync(revokeTokenRequest.ClientCode, revokeTokenRequest.ClientSecret, jti);

        if (!result)
        {
            return TypedResults.BadRequest();
        }

        return TypedResults.Ok();
    }
}
