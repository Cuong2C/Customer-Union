namespace Customer_Union.EndpointHandlers.CustomerHandlers;

public class AddCustomerHandler(IAddCustomer addCustomer, ILogger<AddCustomerHandler> logger, IMapper mapper)
{
    public async Task<IResult> AddCustomerAsync(HttpContext httpContext, CustomerRequest customerRequest)
    {
        var customer = mapper.Map<Customer>(customerRequest);

        string clientSourceCode = httpContext.User.FindFirst("ClientSourceCode")!.Value;
        if (string.IsNullOrEmpty(clientSourceCode))
        {
            throw new BadRequestException("Client source code is missing in the token.");
        }

        customer.SetAdditionalProperties(clientSourceCode);

        var result = await addCustomer.AddCustomerAsync(customer);

        if (result == null)
        {
            throw new Exception("Failed to add customer.");
        }

        logger.LogInformation($"Customer id: {result.Id} added successfully");
        return TypedResults.Ok(result);
    }
}
