using Customer_Union.Application.Interfaces.Securities;

namespace Customer_Union.EndpointHandlers.Securities;

public class RevokeTokenHandler(IRevokeToken revokeToken)
{
    public async Task<Results<Ok, BadRequest>> RevokeTokenAsync(RevokeTokenRequest revokeTokenRequest)
    {
        if (string.IsNullOrWhiteSpace(revokeTokenRequest.ClientCode) || string.IsNullOrWhiteSpace(revokeTokenRequest.ClientSecret))
        {
            return TypedResults.BadRequest();
        }

        var result = await revokeToken.RevokeTokenAsync(revokeTokenRequest.ClientCode, revokeTokenRequest.ClientSecret);

        if (!result)
        {
            return TypedResults.BadRequest();
        }

        return TypedResults.Ok();
    }
}
