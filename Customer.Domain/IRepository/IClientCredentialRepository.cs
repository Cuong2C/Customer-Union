using Customer_Union.Domain.Entities;

namespace Customer_Union.Domain.IRepository
{
    public interface IClientCredentialRepository
    {
        Task<string> CreateClientCredentialAsync(string clientCode);
        Task<bool> ValidateClientSecretAsync(string clientCode, string clientSecret);
        Task RevokeClientCredentialsAsync(string clientCode);
    }
}
