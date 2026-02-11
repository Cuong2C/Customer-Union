namespace Customer_Union.EndpointHandlers.CustomerHandlers;

public class GetCustomerByTaxCodeHandler(IGetCustomerByTaxCode getCustomerByTaxCode, ILogger<GetCustomerByTaxCodeHandler> logger, IMapper mapper)
{
    public async Task<Results<Ok<CustomerResponse>, NotFound>> GetCustomerByTaxCodeAsync(string taxCode)
    {
        var customer = await getCustomerByTaxCode.GetCustomerByTaxCodeAsync(taxCode);

        if (customer == null)
        {
            logger.LogWarning($"Customer with tax code {taxCode} not found.");
            return TypedResults.NotFound();
        }

        logger.LogInformation($"Retrieved customer with tax code {taxCode} successfully.");
        var customerResponse = mapper.Map<CustomerResponse>(customer);

        return TypedResults.Ok(customerResponse);
    }
}
