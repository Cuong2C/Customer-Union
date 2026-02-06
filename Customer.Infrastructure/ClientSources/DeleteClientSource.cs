using Customer_Union.Application.Interfaces.ClientSources;
using Customer_Union.Domain.IRepository;
using Customer_Union.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Customer_Union.Infrastructure.ClientSources;

public class DeleteClientSource(IClientSourceRepository clientSourceRepository, ILogger<DeleteClientSource> logger, IUnitOfWork unitOfWork) : IDeleteClientSource
{
    public async Task<bool> DeleteClientSourceAsync(string clientSourceCode)
    {
        var result = await clientSourceRepository.DeleteClientSourceAsync(clientSourceCode);
        if (result == 0)
        {
            logger.LogWarning($"Client source with code {clientSourceCode} not found.");
            return false;
        }

        unitOfWork.Commit();
        return true;
    }
}
