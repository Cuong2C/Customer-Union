namespace Customer_Union.EndpointHandlers.CustomerHandlers;

public class DeleteCustomerHandler(IDeleteCustomer deleteCustomer, ILogger<DeleteCustomerHandler> logger)
{
    public async Task<IResult> DeleteCustomerAsync(HttpContext httpContext, Guid id)
    {
        string clientSourceCode = httpContext.User.FindFirst("ClientSourceCode")!.Value;

        var result = await deleteCustomer.DeleteCustomerAsync(id, clientSourceCode);

        if (result == 0)
        {
            logger.LogError($"Customer with id {id} not found.");
            throw new NotFoundException($"Customer with id {id} not found.");
        }

        logger.LogInformation($"Customer with id {id} deleted successfully.");
        return TypedResults.Ok();
    }
}
