namespace Customer_Union.EndpointHandlers.ClientSourceHandlers;

public class DeleteClientSourceHandler(IDeleteClientSource deleteClientSource, ILogger<DeleteClientSourceHandler> logger)
{
    public async Task<Results<Ok, NotFound>> DeleteClientSourceAsync(string clientSourceCode)
    {
        var result = await deleteClientSource.DeleteClientSourceAsync(clientSourceCode);
        if (!result)
        {
            return TypedResults.NotFound();
        }

        logger.LogInformation($"Client source with code {clientSourceCode} deleted successfully.");
        return TypedResults.Ok();
    }
}
