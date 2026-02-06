using Customer_Union.Application.Interfaces.Securities;

namespace Customer_Union.EndpointHandlers.Securities;

public class CreateClientSecretHandler(ICreateClientSecret createClientSecret)
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

        return TypedResults.Ok(response);
    }
}
