using Dapper;
using Customer_Union.Domain.Entities;
using Customer_Union.Domain.IRepository;
using Customer_Union.Infrastructure.Data;
using System.Security.Cryptography;
using System.Text;

namespace Customer_Union.Infrastructure.Repository;

public class ClientCredentialRepository(string clientSecretHashKey, IUnitOfWork unitOfWork) : IClientCredentialRepository
{
    private const string CREATE_CLIENT_CREDENTIAL_QUERY = @"
        INSERT INTO ClientCredentials (Id, clientCode, secretHash, createdAt, isRevoked)
        VALUES (@Id, @clientCode, @secretHash, @createdAt, @isRevoked )";

    private const string VALIDATE_CLIENT_SECRET_QUERY = @"
        SELECT 1 FROM ClientCredentials 
        WHERE clientCode = @clientCode AND secretHash = @secretHash AND isRevoked = 0";

    private const string REVOKE_CLIENT_CREDENTIALS_QUERY = @"
        UPDATE ClientCredentials 
        SET isRevoked = 1 
        WHERE clientCode = @clientCode";
    public async Task<string> CreateClientCredentialAsync(string clientCode)
    {
        var clientSecret = GenerateClientSecret(32);
        var clientCredential = new ClientCredential
        {
            Id = Guid.NewGuid(),
            ClientCode = clientCode,
            SecretHash = HashWithHMACSHA256(clientSecret, clientSecretHashKey),
            CreatedAt = DateTime.Now,
            IsRevoked = false
        };

        var connection = unitOfWork.Connection;
        var result = await connection.ExecuteAsync(
            CREATE_CLIENT_CREDENTIAL_QUERY, 
            new
            {
                Id = clientCredential.Id,
                clientCode = clientCredential.ClientCode,
                secretHash = clientCredential.SecretHash,
                createdAt = clientCredential.CreatedAt,
                isRevoked = clientCredential.IsRevoked
            }, 
            transaction: unitOfWork.Transaction);

        return result == 1 ? clientSecret : "";
    }

    public async Task<bool> ValidateClientSecretAsync(string clientCode, string clientSecret)
    {
        var secretHash = HashWithHMACSHA256(clientSecret, clientSecretHashKey);
        var connection = unitOfWork.Connection;
        return await connection.ExecuteScalarAsync<bool>(
            VALIDATE_CLIENT_SECRET_QUERY,
            new { clientCode, secretHash },
            transaction: unitOfWork.Transaction);
    }

    // only one client credential of every Client Application can be valid at a time, so when add a new one we must revoke the remains
    public async Task RevokeClientCredentialsAsync(string clientCode)
    {
        var connection = unitOfWork.Connection;
        await connection.ExecuteAsync(
            REVOKE_CLIENT_CREDENTIALS_QUERY,
            new { clientCode },
            transaction: unitOfWork.Transaction);
    }

    private string GenerateClientSecret(int length)
    {
        var bytes = new byte[length];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToBase64String(bytes);
    }

    private string HashWithHMACSHA256(string clientSecret, string key)
    {
        var keyBytes = Encoding.UTF8.GetBytes(key);
        var secretBytes = Encoding.UTF8.GetBytes(clientSecret);

        using var hmac = new HMACSHA256(keyBytes);
        var hashBytes = hmac.ComputeHash(secretBytes);
        return Convert.ToHexString(hashBytes);
    }
}
