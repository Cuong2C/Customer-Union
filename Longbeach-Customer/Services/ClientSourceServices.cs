using Longbeach.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Longbeach_Customer.Services;

public class ClientSourceServices(IClientSourceRepository repository, IUnitOfWork unitOfWork, ILogger<ClientSourceServices> logger) : IClientSourceServices
{
    public async Task<Results<Ok, BadRequest>> AddClientSourceAsync(ClientSource clientSource)
    {
        if (string.IsNullOrEmpty(clientSource.ClientCode))
        {
            logger.LogWarning("Client code is null or empty.");
            return TypedResults.BadRequest();
        }

        var existingClientSource = await repository.IsValidClientSource(clientSource.ClientCode);
        if (existingClientSource)
        {
            logger.LogWarning($"Client source with code {clientSource.ClientCode} already exists.");
            return TypedResults.BadRequest();
        }
        await repository.AddClientSourceAsync(clientSource);
        unitOfWork.Commit();
        logger.LogInformation($"Client source {clientSource.ClientCode} added successfully.");
        return TypedResults.Ok();
    }

    public async Task<Results<Ok, NotFound>> DeleteClientSourceAsync(string clientSourceCode)
    {
        try
        {

            var result = await repository.DeleteClientSourceAsync(clientSourceCode);
            if (result == 0)
            {
                logger.LogWarning($"Client source with code {clientSourceCode} not found.");
                return TypedResults.NotFound();
            }
            unitOfWork.Commit();
            logger.LogInformation($"Client source with code {clientSourceCode} deleted successfully.");
            return TypedResults.Ok();
        }
        catch(Exception e)
        {
            unitOfWork.Rollback();
            logger.LogError($"Failed to delete client source with code {clientSourceCode}: {e.Message}");
            return TypedResults.NotFound();
        }
    }

    public async Task<Ok<IEnumerable<ClientSource>>> GetAllClientSourcesAsync()
    {
        var clientSources = await repository.GetAllClientSourcesAsync();
        return TypedResults.Ok(clientSources);
    }

    public async Task<Results<Ok<ClientSource>, NotFound>> GetClientSourceByCodeAsync(string clientSourceCode)
    {

        var clientSource = await repository.GetClientSourceByCodeAsync(clientSourceCode);
        logger.LogInformation($"Retrieved client source with code {clientSourceCode} successfully.");
        if (clientSource == null)
        {
            logger.LogWarning($"Client source with code {clientSourceCode} not found.");
            return TypedResults.NotFound();
        }
        return TypedResults.Ok(clientSource);
       
    }

    public async Task<Results<Ok, BadRequest>> UpdateClientSourceAsync(ClientSource clientSource)
    {

        var result = await repository.UpdateClientSourceAsync(clientSource);
        if (result == 0)
        {
            logger.LogWarning($"Client source with code {clientSource.ClientCode} not found.");
            return TypedResults.BadRequest();
        }
        unitOfWork.Commit();
        logger.LogInformation($"Client source {clientSource.ClientCode} updated successfully.");
        return TypedResults.Ok();

    }
}
