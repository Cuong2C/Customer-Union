using Customer_Union.Application.Interfaces.ClientSources;

namespace CustomerUnion.EndpointHandlers.ClientSourceHandlers;

public class UpdateClientSourceHandler(IUpdateClientSource updateClientSource, ILogger<UpdateClientSourceHandler> logger, IMapper mapper)
{
    public async Task<Results<Ok, BadRequest>> UpdateClientSourceAsync(ClientSourceRequest clientSourceRequest)
    {
        var clientSource = mapper.Map<ClientSource>(clientSourceRequest);

        var result = await updateClientSource.UpdateClientSourceAsync(clientSource);
        if (!result)
        {
            return TypedResults.BadRequest();
        }


        logger.LogInformation($"Client source {clientSource.ClientCode} updated successfully.");
        return TypedResults.Ok();

    }
}
