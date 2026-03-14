namespace Customer_Union.EndpointHandlers.ClientSourceHandlers;

public class UpdateClientSourceHandler(IUpdateClientSource updateClientSource, ILogger<UpdateClientSourceHandler> logger, IMapper mapper)
{
    public async Task<IResult> UpdateClientSourceAsync(ClientSourceRequest clientSourceRequest)
    {
        var clientSource = mapper.Map<ClientSource>(clientSourceRequest);

        var result = await updateClientSource.UpdateClientSourceAsync(clientSource);
        if (!result)
        {
            throw new BadRequestException($"Failed to update client source with code: {clientSource.ClientCode}");
        }


        logger.LogInformation($"Client source {clientSource.ClientCode} updated successfully.");
        return TypedResults.Ok();

    }
}
