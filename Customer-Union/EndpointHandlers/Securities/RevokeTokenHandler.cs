using Customer_Union.Application.Interfaces.Securities;

namespace Customer_Union.EndpointHandlers.Securities;

public class RevokeTokenHandler(IRevokeToken revokeToken)
{
    public async Task<IResult> RevokeTokenAsync(RevokeTokenRequest revokeTokenRequest)
    {
        if (string.IsNullOrWhiteSpace(revokeTokenRequest.ClientCode) || string.IsNullOrWhiteSpace(revokeTokenRequest.ClientSecret))
        {
            throw new BadRequestException("ClientCode and ClientSecret are required to revoke a token.");
        }

        var result = await revokeToken.RevokeTokenAsync(revokeTokenRequest.ClientCode, revokeTokenRequest.ClientSecret);

        if (!result)
        {
            throw new BadRequestException("Failed to revoke the token. Please check the provided credentials.");
        }

        return TypedResults.Ok();
    }
}
