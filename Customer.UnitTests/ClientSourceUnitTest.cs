
using Dapper;
using Customer_Union.Domain.Entities;
using Customer_Union.Infrastructure.Data;
using Customer_Union.Infrastructure.Repository;
using SQLitePCL;

namespace Customer_Union.UnitTests;

public class ClientSourceUnitTest
{
    [Fact]
    public async void Create_Update_Delete_ClientSources_UnitTest()
    {
        // Arrange
        Batteries.Init();
        var connectionFactory = new SqliteConnectionFactory();
        using var unitOfWork = new DapperUnitOfWork(connectionFactory);

        var createTableSql = @"
            CREATE TABLE ClientSources (
            clientCode TEXT NOT NULL,
            clientName TEXT,
            Description TEXT,
            isActive INTEGER);";

        unitOfWork.Connection.Execute(createTableSql, transaction: unitOfWork.Transaction);

        var repo = new ClientSourceRepository(unitOfWork);

        var clientSource1 = new ClientSource
        {
            ClientCode = "CS001",
            ClientName = "Test Client",
            Description = "This is a test client source.",
            IsActive = true
        };

        var clientSource2 = new ClientSource
        {
            ClientCode = "CS002",
            ClientName = "Test Client2",
            Description = "This is a test client source2.",
            IsActive = true
        };

        var clientSource3 = new ClientSource
        {
            ClientCode = "CS003",
            ClientName = "Test Client3",
            Description = "This is a test client source3.",
            IsActive = true
        };

        // Act
        await repo.AddClientSourceAsync(clientSource1);
        await repo.AddClientSourceAsync(clientSource2);
        await repo.AddClientSourceAsync(clientSource3);
        var retrievedClientSource = await repo.GetClientSourceByCodeAsync(clientSource1.ClientCode);
        var retrievedClientSource2 = await repo.GetClientSourceByCodeAsync(clientSource2.ClientCode);
        var allClientSources = await repo.GetAllClientSourcesAsync();

        retrievedClientSource2!.ClientName = "Updated Client Name";
        retrievedClientSource2.Description = "Updated Description";

        var resultUpdate2 = await repo.UpdateClientSourceAsync(retrievedClientSource2);
        var updatedClientSource2 = await repo.GetClientSourceByCodeAsync(clientSource2.ClientCode);

        var resultDelete = await repo.DeleteClientSourceAsync(clientSource1.ClientCode);
        var allClientSources2 = await repo.GetAllClientSourcesAsync();

        unitOfWork.Commit();

        // Assert
        Assert.NotNull(retrievedClientSource);
        Assert.Equal(clientSource1.ClientCode, retrievedClientSource.ClientCode);
        Assert.Equal(clientSource1.ClientName, retrievedClientSource.ClientName);
        Assert.Equal(clientSource1.Description, retrievedClientSource.Description);

        Assert.NotNull(allClientSources);
        Assert.Equal(3, allClientSources.Count());
        Assert.Contains(allClientSources, cs => cs.ClientCode == clientSource1.ClientCode);
        Assert.Contains(allClientSources, cs => cs.ClientCode == clientSource2.ClientCode);
        Assert.Contains(allClientSources, cs => cs.ClientCode == clientSource3.ClientCode);

        Assert.Equal(1, resultDelete);
        Assert.DoesNotContain(allClientSources2, cs => cs.ClientCode == clientSource1.ClientCode);
        Assert.Equal(2, allClientSources2.Count());

        Assert.NotNull(retrievedClientSource2);
        Assert.NotNull(updatedClientSource2);
        Assert.Equal(clientSource2.ClientCode, retrievedClientSource2.ClientCode);
        Assert.NotEqual(clientSource2.ClientName, retrievedClientSource2.ClientName);
        Assert.NotEqual(clientSource2.Description, retrievedClientSource2.Description);


    }




}