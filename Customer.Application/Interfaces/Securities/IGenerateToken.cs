namespace Customer_Union.Application.Interfaces.Securities;

public interface IGenerateToken
{
    Task<string> GenerateTokenAsync(string clientCode, string clientSecret);
}
