namespace Customer_Union.EndpointHandlers.CustomerHandlers;

public class GetCustomerByPearlCustomerCodeHandler(IGetCustomerByPearlCode getCustomerByPearlCode, ILogger<GetCustomerByPearlCustomerCodeHandler> logger, IMapper mapper)
{
    public async Task<Results<Ok<CustomerResponse>, NotFound>> GetCustomerByPearlCustomerCodeAsync(string pearlCustomerCode)
    {
        var customer = await getCustomerByPearlCode.GetCustomerByPearlCustomerCodeAsync(pearlCustomerCode);

        if (customer == null)
        {
            logger.LogWarning($"Customer with pearlCustomerCode {pearlCustomerCode} not found.");
            return TypedResults.NotFound();
        }

        logger.LogInformation($"Retrieved customer with pearlCustomerCode {pearlCustomerCode} successfully.");
        var customerResponse = mapper.Map<CustomerResponse>(customer);

        return TypedResults.Ok(customerResponse);
    }
}
