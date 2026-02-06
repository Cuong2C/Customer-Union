using Customer_Union.Application.Interfaces.ClientSources;
using Customer_Union.Domain.Entities;
using Customer_Union.Domain.IRepository;
using Customer_Union.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Customer_Union.Infrastructure.ClientSources;

public class UpdateClientSource(ILogger<UpdateClientSource> logger, IClientSourceRepository clientSourceRepository, IUnitOfWork unitOfWork) : IUpdateClientSource
{
    public async Task<bool> UpdateClientSourceAsync(ClientSource clientSource)
    {
        var result = await clientSourceRepository.UpdateClientSourceAsync(clientSource);

        if(result == 0)
        {
            logger.LogWarning("Failed to update client source with code {ClientCode}.", clientSource.ClientCode);
            return false;
        }
        
        unitOfWork.Commit();
        return true;
    }
}
