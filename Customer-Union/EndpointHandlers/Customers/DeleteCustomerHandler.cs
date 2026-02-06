namespace CustomerUnion.EndpointHandlers.CustomerHandlers;

public class DeleteCustomerHandler(IDeleteCustomer deleteCustomer, ILogger<DeleteCustomerHandler> logger, IUnitOfWork unitOfWork)
{
    public async Task<IResult> DeleteCustomerAsync(HttpContext httpContext, Guid id)
    {
        try 
        { 
            var result = await deleteCustomer.DeleteCustomerAsync(id);
            if (result == 0)
            {
                logger.LogError($"Customer with id {id} not found.");
                return Results.StatusCode(StatusCodes.Status404NotFound);
            }

            unitOfWork.Commit();
            logger.LogInformation($"Customer with id {id} deleted successfully.");
            return TypedResults.Ok();
        }
        catch (Exception ex)
        {
            unitOfWork.Rollback();
            logger.LogError($"Failed to delete customer: {ex.Message}");
            return TypedResults.BadRequest();
        }
    }
}
