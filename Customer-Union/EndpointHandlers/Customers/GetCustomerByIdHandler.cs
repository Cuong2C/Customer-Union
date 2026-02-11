namespace Customer_Union.EndpointHandlers.CustomerHandlers;

public class GetCustomerByIdHandler(IGetCustomerById getCustomerById, ILogger<GetCustomerByIdHandler> logger, IMapper mapper)
{
    public async Task<Results<Ok<CustomerResponse>, NotFound>> GetCustomerByIdAsync(Guid id)
    {
        var customer = await getCustomerById.GetCustomerByIdAsync(id);

        if (customer == null)
        {
            logger.LogWarning($"Customer with id {id} not found.");
            return TypedResults.NotFound();
        }

        logger.LogInformation($"Retrieved customer with id {id} successfully.");
        var customerResponse = mapper.Map<CustomerResponse>(customer);

        return TypedResults.Ok(customerResponse);
    }
}
