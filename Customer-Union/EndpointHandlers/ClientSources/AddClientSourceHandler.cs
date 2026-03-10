namespace Customer_Union.EndpointHandlers.ClientSourceHandlers;

public class AddClientSourceHandler(ILogger<AddClientSourceHandler> logger, IAddClientSource addClientSource, IMapper mapper)
{
    public async Task<Results<Ok, BadRequest>> AddClientSourceAsync(ClientSourceRequest clientSourceRequest)
    {
        var clientSource = mapper.Map<ClientSource>(clientSourceRequest);
        var result = await addClientSource.AddClientSourceAsync(clientSource);

        if (!result)
        {
            logger.LogWarning("Failed to add client source: {ClientCode}", clientSource.ClientCode);
            return TypedResults.BadRequest();
        }

        logger.LogInformation("Client source added successfully: {ClientCode}", clientSource.ClientCode);
        return TypedResults.Ok();
    }
}
