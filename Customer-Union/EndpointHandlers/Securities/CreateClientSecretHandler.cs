using Customer_Union.Application.Interfaces.Securities;

namespace Customer_Union.EndpointHandlers.Securities;

public class CreateClientSecretHandler(ICreateClientSecret createClientSecret, ILogger<CreateClientSecretHandler> logger)
{
    public async Task<Results<Ok<CreateClientSecretResponse>, BadRequest>> CreateClientSecretAsync(CreateClientSecretRequest createClientSecretRequest)
    {
        var result = await createClientSecret.CreateClientSecretAsync(createClientSecretRequest.ClientCode);

        if (string.IsNullOrEmpty(result))
        {
            return TypedResults.BadRequest();
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
