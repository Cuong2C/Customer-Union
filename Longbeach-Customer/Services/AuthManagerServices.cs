
using Longbeach_Customer.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.IdentityModel.Tokens.Jwt;

namespace Longbeach_Customer.Services;

public class AuthManagerServices(IClientCredentialRepository clientCredentialRepo, IClientSourceRepository clientSourceRepo,
    ITokenAuthenticationServices tokenAuthenticationServices, IUnitOfWork unitOfWork, ILogger<AuthManagerServices> logger) : IAuthManagerHandlers
{
    public async Task<Results<Ok<CreateClientSecretResponse>, BadRequest>> CreateClientSecretAsync(CreateClientSecretRequest createClientSecretRequest)
    {
        if (string.IsNullOrWhiteSpace(createClientSecretRequest.ClientCode))
        {
            logger.LogError("Client code is empty");
            return TypedResults.BadRequest();
        }
        if (!await clientSourceRepo.IsValidClientSource(createClientSecretRequest.ClientCode))
        {
            logger.LogError($"Client source with code {createClientSecretRequest.ClientCode} does not valid.");
            return TypedResults.BadRequest();
        }

        try
        {
            // only one client credential of every Client Application can be valid at a time, so when add a new one we must revoke the orthers
            await clientCredentialRepo.RevokeClientCredentialsAsync(createClientSecretRequest.ClientCode);

            var clientSecret = await clientCredentialRepo.CreateClientCredentialAsync(createClientSecretRequest.ClientCode);

            unitOfWork.Commit();

            var response = new CreateClientSecretResponse
            {
                ClientCode = createClientSecretRequest.ClientCode,
                ClientSecret = clientSecret
            };

            logger.LogInformation($"Client secret created successfully for client code {createClientSecretRequest.ClientCode}");
            return TypedResults.Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating client secret");
            unitOfWork.Rollback();
            return TypedResults.BadRequest();
        }
    }

    public async Task<Results<Ok<GenrateTokenResponse>, BadRequest>> GenerateTokenAsync(GenrateTokenRequest genrateTokenRequest)
    {
        if (string.IsNullOrWhiteSpace(genrateTokenRequest.ClientCode) || string.IsNullOrWhiteSpace(genrateTokenRequest.ClientSecret))
        {
            logger.LogError("Client code or client secret is empty");
            return TypedResults.BadRequest();
        }

        if (!await clientCredentialRepo.ValidateClientSecretAsync(genrateTokenRequest.ClientCode, genrateTokenRequest.ClientSecret))
        {
            logger.LogError($"Invalid client code or client secret for client code {genrateTokenRequest.ClientCode}");
            return TypedResults.BadRequest();
        }

        var token = tokenAuthenticationServices.GenerateTokenAsync(genrateTokenRequest.ClientCode);

        var response = new GenrateTokenResponse
        {
            ClientCode = genrateTokenRequest.ClientCode,
            Token = token
        };

        logger.LogInformation($"Token generated successfully for client code {genrateTokenRequest.ClientCode}");
        return TypedResults.Ok(response);
    }

    public async Task<Results<Ok, BadRequest>> RevokeTokenAsync(RevokeTokenRequest revokeTokenRequest, HttpContext httpContext)
    {
        if (string.IsNullOrWhiteSpace(revokeTokenRequest.ClientCode) || string.IsNullOrWhiteSpace(revokeTokenRequest.ClientSecret))
        {
            logger.LogError("Client code or client secret is empty");
            return TypedResults.BadRequest();
        }

        if (!await clientCredentialRepo.ValidateClientSecretAsync(revokeTokenRequest.ClientCode, revokeTokenRequest.ClientSecret))
        {
            logger.LogError($"Invalid  pair (client code and client secret): {revokeTokenRequest.ClientCode}");
            return TypedResults.BadRequest();
        }
        string clientSourceCode = httpContext.User.FindFirst("ClientSourceCode")!.Value;
        if (clientSourceCode != revokeTokenRequest.ClientCode)
        {
            logger.LogError($"Client source code of token ({clientSourceCode}) does not match with the request client code in body {revokeTokenRequest.ClientCode}");
            return TypedResults.BadRequest();
        }

        var jti = httpContext.User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
        tokenAuthenticationServices.RevokeTokenAsync(jti!);

        logger.LogInformation($"Token revoked successfully for client code {revokeTokenRequest.ClientCode}");
        return TypedResults.Ok();
    }
}
