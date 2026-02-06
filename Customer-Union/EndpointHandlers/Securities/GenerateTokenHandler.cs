using Customer_Union.Application.Interfaces.Securities;

namespace CustomerUnion.EndpointHandlers.Securities;

public class GenerateTokenHandler(IGenrateToken genrateToken)
{
    public async Task<Results<Ok<GenrateTokenResponse>, BadRequest>> GenerateTokenAsync(GenrateTokenRequest genrateTokenRequest)
    {
        var result =  await genrateToken.GenerateTokenAsync(genrateTokenRequest.ClientCode, genrateTokenRequest.ClientSecret);

        if (string.IsNullOrEmpty(result))
        {
            return TypedResults.BadRequest();
        }

        var response = new GenrateTokenResponse
        {
            ClientCode = genrateTokenRequest.ClientCode,
            Token = result
        };

        return TypedResults.Ok(response);
    }
}
