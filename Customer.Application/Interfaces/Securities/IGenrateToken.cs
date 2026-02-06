namespace Customer_Union.Application.Interfaces.Securities;

public interface IGenrateToken
{
    Task<string> GenerateTokenAsync(string clientCode, string clientSecret);
}
