namespace Customer_Union.EndpointHandlers.CustomerHandlers;

public class GetCustomerByIdHandler(IGetCustomerById getCustomerById, ILogger<GetCustomerByIdHandler> logger, IMapper mapper)
{
    public async Task<IResult> GetCustomerByIdAsync(Guid id)
    {
        var customer = await getCustomerById.GetCustomerByIdAsync(id);

        if (customer == null)
        {
            logger.LogWarning($"Customer with id {id} not found.");
            throw new NotFoundException($"Customer with id {id} not found.");
        }

        logger.LogInformation($"Retrieved customer with id {id} successfully.");
        var customerResponse = mapper.Map<CustomerResponse>(customer);

        return TypedResults.Ok(customerResponse);
    }
}
