namespace Customer_Union.EndpointHandlers.ClientSourceHandlers;

public class DeleteClientSourceHandler(IDeleteClientSource deleteClientSource, ILogger<DeleteClientSourceHandler> logger)
{
    public async Task<IResult> DeleteClientSourceAsync(string clientSourceCode)
    {
        var result = await deleteClientSource.DeleteClientSourceAsync(clientSourceCode);
        if (!result)
        {
            throw new NotFoundException($"Client source with code {clientSourceCode} not found.");
        }

        logger.LogInformation($"Client source with code {clientSourceCode} deleted successfully.");
        return TypedResults.Ok();
    }
}
