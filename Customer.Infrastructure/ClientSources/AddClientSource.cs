using Customer_Union.Application.Interfaces.ClientSources;
using Customer_Union.Domain.Entities;
using Customer_Union.Domain.IRepository;
using Customer_Union.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Customer_Union.Infrastructure.ClientSources
{
    public class AddClientSource(IClientSourceRepository repository, IUnitOfWork unitOfWork, ILogger<AddClientSource> logger) : IAddClientSource
    {
        public async Task<bool> AddClientSourceAsync(ClientSource clientSource)
        {
            if (string.IsNullOrEmpty(clientSource.ClientCode))
            {
                logger.LogWarning("Client code is null or empty.");
                return false;
            }

            var existingClientSource = await repository.IsValidClientSource(clientSource.ClientCode);
            if (existingClientSource)
            {
                logger.LogWarning($"Client source with code {clientSource.ClientCode} already exists.");
                return false;
            }

            await repository.AddClientSourceAsync(clientSource);
            unitOfWork.Commit();
            return true;
        }
    }
}
