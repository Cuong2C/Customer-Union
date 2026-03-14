namespace Customer_Union.EndpointHandlers.CustomerHandlers;

public class GetCustomerByPhoneHandler(IGetCustomerByPhone getCustomerByPhone, ILogger<GetCustomerByPhoneHandler> logger, IMapper mapper)
{
    public async Task<IResult> GetCustomerByPhoneAsync(string phoneNumber)
    {
        var customers = await getCustomerByPhone.GetCustomerByPhoneAsync(phoneNumber);

        if (!customers.Any())
        {
            logger.LogWarning($"Customer with phone {phoneNumber} not found.");
            return TypedResults.NotFound();
        }

        logger.LogInformation($"Retrieved customer with phone {phoneNumber} successfully.");
        var customerResponse = mapper.Map<IEnumerable<CustomerResponse>>(customers);

        return TypedResults.Ok(customerResponse);
    }
}
