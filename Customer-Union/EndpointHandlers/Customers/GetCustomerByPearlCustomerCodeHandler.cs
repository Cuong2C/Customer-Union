namespace Customer_Union.EndpointHandlers.CustomerHandlers;

public class GetCustomerByPearlCustomerCodeHandler(IGetCustomerByPearlCode getCustomerByPearlCode, ILogger<GetCustomerByPearlCustomerCodeHandler> logger, IMapper mapper)
{
    public async Task<IResult> GetCustomerByPearlCustomerCodeAsync(string pearlCustomerCode)
    {
        var customer = await getCustomerByPearlCode.GetCustomerByPearlCustomerCodeAsync(pearlCustomerCode);

        if (customer == null)
        {
            logger.LogWarning($"Customer with pearlCustomerCode {pearlCustomerCode} not found.");
            throw new NotFoundException($"Customer with pearlCustomerCode {pearlCustomerCode} not found.");
        }

        logger.LogInformation($"Retrieved customer with pearlCustomerCode {pearlCustomerCode} successfully.");
        var customerResponse = mapper.Map<CustomerResponse>(customer);

        return TypedResults.Ok(customerResponse);
    }
}
