using AutoMapper;

namespace Customer_Union.EndpointHandlers.CustomerHandlers;

public class UpdateCustomerHandler(IUpdateCustomer updateCustomer, ILogger<UpdateCustomerHandler> logger, IMapper mapper)
{
    public async Task<Results<Ok<HashCodeResponse>, BadRequest>> UpdateCustomerAsync(HttpContext httpContext, CustomerRequest customerRequest, Guid id)
    {
        var customer = mapper.Map<Customer>(customerRequest);
        customer.Id = id;

        string clientSourceCode = httpContext.User.FindFirst("ClientSourceCode")!.Value;

        var result = await updateCustomer.UpdateCustomerAsync(clientSourceCode, customer, id);

        if(result == null)
        {
            logger.LogWarning("Failed to update customer with ID {CustomerId}.", id);
            return TypedResults.BadRequest();
        }

        logger.LogInformation("Customer with ID {CustomerId} updated successfully.", id);

        return TypedResults.Ok(result);
    }
}