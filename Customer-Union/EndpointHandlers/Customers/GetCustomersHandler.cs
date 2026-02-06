namespace CustomerUnion.EndpointHandlers.CustomerHandlers;

public class GetCustomersHandler(IGetCustomer getCustomer, ILogger<GetCustomersHandler> logger, IMapper mapper)
{
    public async Task<Results<Ok<PagedResult<CustomerResponse>>, NotFound>> GetCustomersAsync(DateTime? cursorDate, Guid? cursorId, int pageSize = 20, string direction = "next")
    {
        var result = await getCustomer.GetCustomersAsync(cursorDate, cursorId, pageSize, direction);
        if (result.Items.Count == 0)
        {
            logger.LogInformation("No customers found for the given cursorDate: {CursorDate}, cursorId: {CursorId}", cursorDate, cursorId);
            return TypedResults.NotFound();
        }

        var customerResponses = mapper.Map<PagedResult<CustomerResponse>>(result.Items);

        return TypedResults.Ok(customerResponses);
    }
}
