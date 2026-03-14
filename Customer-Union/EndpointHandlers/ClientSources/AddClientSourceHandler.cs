namespace Customer_Union.EndpointHandlers.ClientSourceHandlers;

public class AddClientSourceHandler(ILogger<AddClientSourceHandler> logger, IAddClientSource addClientSource, IMapper mapper)
{
    public async Task<IResult> AddClientSourceAsync(ClientSourceRequest clientSourceRequest)
    {
        var clientSource = mapper.Map<ClientSource>(clientSourceRequest);
        var result = await addClientSource.AddClientSourceAsync(clientSource);

        if (!result)
        {
            logger.LogWarning("Failed to add client source: {ClientCode}", clientSource.ClientCode);
            throw new BadRequestException($"Failed to add client source with code: {clientSource.ClientCode}");
        }

        logger.LogInformation("Client source added successfully: {ClientCode}", clientSource.ClientCode);
        return TypedResults.Ok();
    }
}
