namespace CustomerUnion.EndpointHandlers.CustomerHandlers;

public class UpdateCustomerHandler(IUpdateCustomer updateCustomer, Logger<UpdateCustomerHandler> logger)
{
    public async Task<Results<Ok<HashCodeResponse>, BadRequest>> UpdateCustomerAsync(HttpContext httpContext, CustomerRequest customerRequest, Guid id)
    {
        string clientSourceCode = httpContext.User.FindFirst("ClientSourceCode")!.Value;

        var result = await updateCustomer.UpdateCustomerAsync(clientSourceCode, customerRequest, id);

        logger.LogInformation("Customer with ID {CustomerId} updated successfully.", id);

        return TypedResults.Ok(result);
    }
}