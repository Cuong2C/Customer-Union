namespace Customer_Union.EndpointHandlers.ClientSourceHandlers;

public class GetClientSourceByCodeHandler(ILogger<GetClientSourceByCodeHandler> logger, IGetClientSourceByCode getClientSourceByCode, IMapper mapper)
{
    public async Task<IResult> GetClientSourceByCodeAsync(string clientSourceCode)
    {
        var clientSource = await getClientSourceByCode.GetClientSourceByCodeAsync(clientSourceCode);
        
        if (clientSource == null)
        {
            logger.LogWarning($"Client source with code {clientSourceCode} not found.");
            throw new NotFoundException($"Client source with code {clientSourceCode} not found.");
        }

        logger.LogInformation($"Retrieved client source with code {clientSourceCode} successfully.");
        var result = mapper.Map<ClientSourceResponse>(clientSource);
        return TypedResults.Ok(result);
    }
}
