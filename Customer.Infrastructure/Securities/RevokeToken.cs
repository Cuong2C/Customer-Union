using Customer_Union.Application.Dtos;
using Customer_Union.Application.Interfaces.Securities;
using Customer_Union.Authentication;
using Customer_Union.Domain.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;

namespace Customer_Union.Infrastructure.Securities;

public class RevokeToken(IClientCredentialRepository clientCredentialRepository, ITokenAuthenticationServices tokenAuthenticationServices, HttpContext httpContext, ILogger<RevokeToken> logger) : IRevokeToken
{
    public async Task<bool> RevokeTokenAsync(string clientCode, string clientSecret)
    {
        if (!await clientCredentialRepository.ValidateClientSecretAsync(clientCode, clientSecret))
        {
            logger.LogError($"Invalid  pair (client code and client secret): {clientCode}");
            return false;
        }

        string clientSourceCode = httpContext.User.FindFirst("ClientSourceCode")!.Value;
        if (clientSourceCode != clientCode)
        {
            logger.LogError($"Client source code of token ({clientSourceCode}) does not match with the request client code in body {clientCode}");
            return false;
        }

        var jti = httpContext.User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

        var result = tokenAuthenticationServices.RevokeTokenAsync(jti!);

        return result;
    }
}
