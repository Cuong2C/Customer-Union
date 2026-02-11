namespace Customer_Union.EndpointHandlers.CustomerHandlers;

public class CheckNewVersionCustomerHandler(ICheckNewVersionCustomer checkNewVersionCustomer, ILogger<CheckNewVersionCustomerHandler> logger)
{
    public async Task<Results<Ok, NotFound>> IsNewVersionCustomerAsync(Guid id, string hashCode)
    {
        var result = await checkNewVersionCustomer.IsNewVersionCustomerAsync(id, hashCode);

        if (!result)
        {
            logger.LogWarning($"This customer version with id {id} is not found.");
            return TypedResults.NotFound();
        }

        return TypedResults.Ok();
    }
}
