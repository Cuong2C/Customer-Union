namespace CustomerUnion.EndpointHandlers.ClientSourceHandlers;

public class GetClientSourceByCodeHandler(ILogger<GetClientSourceByCodeHandler> logger, IGetClientSourceByCode getClientSourceByCode, IMapper mapper)
{
    public async Task<Results<Ok<ClientSourceResponse>, NotFound>> GetClientSourceByCodeAsync(string clientSourceCode)
    {
        var clientSource = await getClientSourceByCode.GetClientSourceByCodeAsync(clientSourceCode);
        
        if (clientSource == null)
        {
            logger.LogWarning($"Client source with code {clientSourceCode} not found.");
            return TypedResults.NotFound();
        }

        logger.LogInformation($"Retrieved client source with code {clientSourceCode} successfully.");
        var result = mapper.Map<ClientSourceResponse>(clientSource);
        return TypedResults.Ok(result);
    }
}
