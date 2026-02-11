using Customer_Union.Application.Dtos;
using Customer_Union.Application.Interfaces.Securities;
using Customer_Union.Authentication;
using Customer_Union.Domain.IRepository;
using Customer_Union.Infrastructure.Data;
using Customer_Union.Infrastructure.Securities.Auth;
using Microsoft.Extensions.Logging;

namespace Customer_Union.Infrastructure.Securities;

public class GenrateToken(IClientCredentialRepository clientCredentialRepository, ITokenAuthenticationServices tokenAuthenticationServices, IUnitOfWork unitOfWork, ILogger<GenrateToken> logger) : IGenrateToken
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
