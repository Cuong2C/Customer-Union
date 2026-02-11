using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Customer_Union.Authentication;

public interface ITokenAuthenticationServices
{
    string GenerateTokenAsync(string clientCode);
    bool ValidateToken(string tokenValue, out SecurityToken? token, out ClaimsPrincipal? principal);
    bool RevokeTokenAsync(string jti);
    bool IsRevokedToken(string jti);
}
