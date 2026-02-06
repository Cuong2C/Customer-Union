namespace Customer_Union.Application.Interfaces.Securities;

public interface IRevokeToken
{
    Task<bool> RevokeTokenAsync(string clientCode, string clientSecret, string? jti);
}
