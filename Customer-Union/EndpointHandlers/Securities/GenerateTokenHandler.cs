using Customer_Union.Application.Interfaces.Securities;

namespace Customer_Union.EndpointHandlers.Securities;

public class GenerateTokenHandler(IGenerateToken genrateToken, ILogger<GenerateTokenHandler> logger)
{
    public async Task<IResult> GenerateTokenAsync(GenrateTokenRequest genrateTokenRequest)
    {
        var result =  await genrateToken.GenerateTokenAsync(genrateTokenRequest.ClientCode, genrateTokenRequest.ClientSecret);

        if (string.IsNullOrEmpty(result))
        {
            throw new BadRequestException("Invalid ClientCode or ClientSecret. Token generation failed.");
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
