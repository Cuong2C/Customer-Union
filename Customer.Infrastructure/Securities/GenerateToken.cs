using Customer_Union.Application.Interfaces.Securities;
using Customer_Union.Authentication;
using Customer_Union.Domain.IRepository;
using Customer_Union.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Customer_Union.Infrastructure.Securities;

public class GenerateToken(IClientCredentialRepository clientCredentialRepository, ITokenAuthenticationServices tokenAuthenticationServices, IUnitOfWork unitOfWork, ILogger<GenerateToken> logger) : IGenerateToken
{
    public async Task<string> GenerateTokenAsync(string clientCode, string clientSecret)
    {
        if (!await clientCredentialRepository.ValidateClientSecretAsync(clientCode, clientSecret))
        {
            logger.LogError($"Invalid client code or client secret for client code {clientCode}");
            return string.Empty;
        }

        var token = tokenAuthenticationServices.GenerateTokenAsync(clientCode);

        return token;
    }
}
