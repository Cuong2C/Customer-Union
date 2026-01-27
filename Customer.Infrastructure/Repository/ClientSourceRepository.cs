using Dapper;
using Customer_Union.Domain.Entities;
using Customer_Union.Domain.IRepository;
using Customer_Union.Infrastructure.Data;
using System.Data.Common;

namespace Customer_Union.Infrastructure.Repository;

public class ClientSourceRepository(IUnitOfWork unitOfWork) : IClientSourceRepository
{
    private const string INSERT_CLIENT_SOURCE_QUERY = "INSERT INTO ClientSources (clientCode, clientName, description, isActive) VALUES (@clientCode, @clientName, @description, @isActive)";
    private const string UPDATE_CLIENT_SOURCE_QUERY = "UPDATE ClientSources SET clientName = @clientName, description = @description, isActive = @isActive WHERE clientCode = @clientCode";
    private const string DELETE_CLIENT_SOURCE_QUERY = "DELETE FROM ClientSources WHERE clientCode = @clientCode";
    private const string GET_ALL_CLIENT_SOURCES_QUERY = "SELECT clientCode, clientName, description, isActive FROM ClientSources WHERE isActive = 1";
    private const string GET_CLIENT_SOURCE_BY_CODE_QUERY = "SELECT clientCode, clientName, description, isActive FROM ClientSources WHERE clientCode = @clientCode";
    private const string IS_VALID_CLIENT_SOURCE_QUERY = "SELECT TOP 1 1 FROM ClientSources WHERE clientCode = @clientCode AND isActive = 1";
    public async Task AddClientSourceAsync(ClientSource clientSource)
    {
        var connection = unitOfWork.Connection;
        await connection.ExecuteAsync(
            INSERT_CLIENT_SOURCE_QUERY,
            new
            {
                clientCode = clientSource.ClientCode,
                clientName = clientSource.ClientName,
                description = clientSource.Description,
                isActive = clientSource.IsActive
            },
            transaction: unitOfWork.Transaction);
    }

    public async Task<int> DeleteClientSourceAsync(string clientCode)
    {
        var connection = unitOfWork.Connection;
        return await connection.ExecuteAsync(
            DELETE_CLIENT_SOURCE_QUERY,
            new { clientCode },
            transaction: unitOfWork.Transaction);
    }

    public async Task<ClientSource?> GetClientSourceByCodeAsync(string clientCode)
    {
        var connection = unitOfWork.Connection;
        var result = await connection.QuerySingleOrDefaultAsync<ClientSource>(
            GET_CLIENT_SOURCE_BY_CODE_QUERY,
            new { clientCode },
            transaction: unitOfWork.Transaction);
        return result;
    }

    public async Task<IEnumerable<ClientSource>> GetAllClientSourcesAsync()
    {
        var connection = unitOfWork.Connection;
        return await connection.QueryAsync<ClientSource>(GET_ALL_CLIENT_SOURCES_QUERY, transaction:unitOfWork.Transaction);
    }

    public async Task<int> UpdateClientSourceAsync(ClientSource clientSource)
    {
        var connection = unitOfWork.Connection;
        return await connection.ExecuteAsync(
            UPDATE_CLIENT_SOURCE_QUERY,
            new
            {
                clientCode = clientSource.ClientCode,
                clientName = clientSource.ClientName,
                description = clientSource.Description,
                isActive = clientSource.IsActive
            },
            transaction: unitOfWork.Transaction);
    }

    public async Task<bool> IsValidClientSource(string clientCode)
    {
        var connection = unitOfWork.Connection;
        return await connection.ExecuteScalarAsync<bool>(
            IS_VALID_CLIENT_SOURCE_QUERY,
            new { clientCode },
            transaction: unitOfWork.Transaction);
    }
}
