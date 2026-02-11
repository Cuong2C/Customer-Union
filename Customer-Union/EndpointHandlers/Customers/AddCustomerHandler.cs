namespace Customer_Union.EndpointHandlers.CustomerHandlers;

public class AddCustomerHandler(IAddCustomer addCustomer, ILogger<AddCustomerHandler> logger, IMapper mapper)
{
    public async Task<Results<Ok<HashCodeResponse>, BadRequest>> AddCustomerAsync(HttpContext httpContext, CustomerRequest customerRequest)
    {
        var customer = mapper.Map<Customer>(customerRequest);

        string clientSourceCode = httpContext.User.FindFirst("ClientSourceCode")!.Value;
        if (string.IsNullOrEmpty(clientSourceCode))
        {
            return TypedResults.BadRequest();
        }

        customer.SetAdditionalProperties(clientSourceCode);

        var result = await addCustomer.AddCustomerAsync(customer);

        if (result == null)
        {
            return TypedResults.BadRequest();
        }

        logger.LogInformation($"Customer id: {result.Id} added successfully");
        return TypedResults.Ok(result);
    }
}
