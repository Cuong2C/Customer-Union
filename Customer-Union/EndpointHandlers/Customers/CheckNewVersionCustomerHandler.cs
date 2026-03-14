namespace Customer_Union.EndpointHandlers.CustomerHandlers;

public class CheckNewVersionCustomerHandler(ICheckNewVersionCustomer checkNewVersionCustomer, ILogger<CheckNewVersionCustomerHandler> logger)
{
    public async Task<IResult> IsNewVersionCustomerAsync(Guid id, string hashCode)
    {
        var result = await checkNewVersionCustomer.IsNewVersionCustomerAsync(id, hashCode);

        if (!result)
        {
            logger.LogWarning($"This customer version with id {id} is not found.");
            throw new NotFoundException($"Customer with id {id} and hash code {hashCode} not found.");
        }

        return TypedResults.Ok();
    }
}
