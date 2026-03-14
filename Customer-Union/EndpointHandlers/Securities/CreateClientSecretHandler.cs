using Customer_Union.Application.Interfaces.Securities;

namespace Customer_Union.EndpointHandlers.Securities;

public class CreateClientSecretHandler(ICreateClientSecret createClientSecret, ILogger<CreateClientSecretHandler> logger)
{
    public async Task<IResult> CreateClientSecretAsync(CreateClientSecretRequest createClientSecretRequest)
    {
        var result = await createClientSecret.CreateClientSecretAsync(createClientSecretRequest.ClientCode);

        if (string.IsNullOrEmpty(result))
        {
            throw new Exception($"Failed to create client secret for client code: {createClientSecretRequest.ClientCode}");
        }

        var response = new CreateClientSecretResponse
        {
            ClientCode = createClientSecretRequest.ClientCode,
            ClientSecret = result
        };
        
        logger.LogInformation("Client secret created for client code: {ClientCode}", createClientSecretRequest.ClientCode);

        return TypedResults.Ok(response);
    }
}
