namespace Customer_Union.EndpointHandlers.CustomerHandlers;

public class CheckNewVersionCustomerHandler(ICheckNewVersionCustomer checkNewVersionCustomer, ILogger<CheckNewVersionCustomerHandler> logger)
{
    public async Task<Results<Ok, NotFound>> IsNewVersionCustomerAsync(Guid id, string hashCode)
    {
        var result = await checkNewVersionCustomer.IsNewVersionCustomerAsync(id, hashCode);

        if (!result)
        {
            logger.LogWarning($"Customer with id {id} is not a new version.");
            return TypedResults.NotFound();
        }

        return TypedResults.Ok();
    }
}
