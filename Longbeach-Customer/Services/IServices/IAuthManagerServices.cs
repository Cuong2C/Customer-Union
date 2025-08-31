using Longbeach_Customer.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Longbeach_Customer.Services.IServices;

public interface IAuthManagerServices
{
    Task<Results<Ok<CreateClientSecretResponse>, BadRequest>> CreateClientSecretAsync(CreateClientSecretRequest createClientSecretRequest);
    Task<Results<Ok<GenrateTokenResponse>, BadRequest>> GenerateTokenAsync(GenrateTokenRequest genrateTokenRequest);
    Task<Results<Ok, BadRequest>> RevokeTokenAsync(RevokeTokenRequest revokeTokenRequest, HttpContext httpContext);
}
