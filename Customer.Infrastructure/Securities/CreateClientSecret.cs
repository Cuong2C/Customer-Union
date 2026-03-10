using Customer_Union.Application.Interfaces.Securities;
using Customer_Union.Domain.IRepository;
using Customer_Union.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Customer_Union.Infrastructure.Securities;

public class CreateClientSecret(IClientCredentialRepository clientCredentialRepository, IClientSourceRepository clientSourceRepository, IUnitOfWork unitOfWork, ILogger<CreateClientSecret> logger) : ICreateClientSecret
{
    public async Task<string> CreateClientSecretAsync(string clientCode)
    {
        if (string.IsNullOrWhiteSpace(clientCode))
        {
            logger.LogError("Client code is empty");
            return string.Empty;
        }
        if (!await clientSourceRepository.IsExistClientSource(clientCode))
        {
            logger.LogError($"Client source with code {clientCode} does not valid.");
            return string.Empty;
        }

        // only one client credential of every Client Application can be valid at a time, so when add a new one we must revoke the orthers
        await clientCredentialRepository.RevokeClientCredentialsAsync(clientCode);

        var clientSecret = await clientCredentialRepository.CreateClientCredentialAsync(clientCode);

        unitOfWork.Commit();

        return clientSecret;
    }
}
