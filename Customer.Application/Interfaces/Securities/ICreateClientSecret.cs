namespace Customer_Union.Application.Interfaces.Securities;

public interface ICreateClientSecret
{
    Task<string> CreateClientSecretAsync(string clientCode);
}
