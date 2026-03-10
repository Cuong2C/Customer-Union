using Customer_Union.Application.Interfaces.Securities;

namespace Customer_Union.EndpointHandlers.Securities;

public class GenerateTokenHandler(IGenerateToken genrateToken, ILogger<GenerateTokenHandler> logger)
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

        logger.LogInformation("Token generated successfully for ClientCode: {ClientCode}", genrateTokenRequest.ClientCode);

        return TypedResults.Ok(response);
    }
}
