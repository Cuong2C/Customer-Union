
using Customer_Union.Application.Dtos;

namespace Customer_Union.Services.IServices;

public interface IAuthManagerHandlers
{
    Task<Results<Ok<CreateClientSecretResponse>, BadRequest>> CreateClientSecretAsync(CreateClientSecretRequest createClientSecretRequest);
    Task<Results<Ok<GenrateTokenResponse>, BadRequest>> GenerateTokenAsync(GenrateTokenRequest genrateTokenRequest);
    Task<Results<Ok, BadRequest>> RevokeTokenAsync(RevokeTokenRequest revokeTokenRequest, HttpContext httpContext);
}
