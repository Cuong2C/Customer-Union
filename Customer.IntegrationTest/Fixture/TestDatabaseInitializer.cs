using Dapper;
using Customer_Union.Infrastructure.Data;
using Microsoft.Data.SqlClient;

namespace Customer_Union.IntegrationTest.Fixture;

public static class TestDatabaseInitializer
{
    public static async Task InitializeDatabaseAsync()
    {
        var baseConnectionString = "Server=localhost,1435;User Id=sa;Password=SqlDocker123;TrustServerCertificate=True";

        //  connect and create the database if it does not exist
        var masterBuilder = new SqlConnectionStringBuilder(baseConnectionString)
        {
            InitialCatalog = "master"
        };

        await using (var masterConnection = new SqlConnection(masterBuilder.ToString()))
        {
            await masterConnection.OpenAsync();
            var checkDbSql = "IF DB_ID('TestDb') IS NULL CREATE DATABASE TestDb";
            await masterConnection.ExecuteAsync(checkDbSql);
        }

        var connectionStringDbBuilder = new SqlConnectionStringBuilder(baseConnectionString)
        {
            InitialCatalog = "TestDb"
        };

        var connectionString = connectionStringDbBuilder.ToString();

        var connection = new SqlServerConnectionFactory(connectionString);
        var unitOfWork = new DapperUnitOfWork(connection);

        // create tables
        unitOfWork.Connection.Execute(@"
            IF OBJECT_ID('Customers') IS NULL
			CREATE TABLE Customers (
				Id UNIQUEIDENTIFIER PRIMARY KEY,
				Name NVARCHAR(255) NOT NULL,
				TaxCode NVARCHAR(50),
				Address NVARCHAR(255),
				Phone NVARCHAR(20),
				Phone2 NVARCHAR(20),
				Phone3 NVARCHAR(20),
				Email NVARCHAR(100),
				Nationality NVARCHAR(100),
				Province NVARCHAR(100),
				District NVARCHAR(100),
				Gender INT NOT NULL,
				DateOfBirth DATETIME,
				BankAccount NVARCHAR(50),
				BankName NVARCHAR(100),
				CustomerType NVARCHAR(50),
				PearlCustomerCode NVARCHAR(50),
				CreatedAt DATETIME NOT NULL,
				UpdatedAt DATETIME NOT NULL,
				CreatedClientSourceCode NVARCHAR(50) NOT NULL,
				UpdatedClientSourceCode NVARCHAR(50) NOT NULL,
				HashCode NVARCHAR(255) NOT NULL
			)", transaction: unitOfWork.Transaction);

        unitOfWork.Connection.Execute(@"
			IF OBJECT_ID('CustomerHistories') IS NULL
			CREATE TABLE [CustomerHistories](
				Id NVARCHAR(50) NOT NULL,
				Name NVARCHAR(255) NOT NULL,
				TaxCode NVARCHAR(50),
				Address NVARCHAR(255),
				Phone NVARCHAR(20),
				Phone2 NVARCHAR(20),
				Phone3 NVARCHAR(20),
				Email NVARCHAR(100),
				Nationality NVARCHAR(100),
				Province NVARCHAR(100),
				District NVARCHAR(100),
				Gender INT NOT NULL,
				DateOfBirth DATETIME,
				BankAccount NVARCHAR(50),
				BankName NVARCHAR(100),
				CustomerType NVARCHAR(50),
				PearlCustomerCode NVARCHAR(50),
				CreatedAt DATETIME NOT NULL,
				UpdatedAt DATETIME NOT NULL,
				CreatedClientSourceCode NVARCHAR(50) NOT NULL,
				UpdatedClientSourceCode NVARCHAR(50) NOT NULL,
				HashCode NVARCHAR(255) NOT NULL,
				ActionTime DATETIME NOT NULL,
				AddedByAction NVARCHAR(100) NOT NULL,
				ChangedByClient NVARCHAR(100) NOT NULL
			)", transaction: unitOfWork.Transaction);

        unitOfWork.Connection.Execute(@"
			IF OBJECT_ID('ClientSources') IS NULL
			CREATE TABLE [ClientSources](
				ClientCode NVARCHAR(50) PRIMARY KEY,
				ClientName NVARCHAR(100) NOT NULL,
				Description NVARCHAR(255),
				IsActive BIT NOT NULL DEFAULT 1
			)", transaction: unitOfWork.Transaction);

        unitOfWork.Connection.Execute(@"
			IF OBJECT_ID('InvalidTokens') IS NULL
			CREATE TABLE [InvalidTokens](
					[jti] [nvarchar](512) NOT NULL,
					[createdAt] [datetime] NOT NULL,
					[note] [nvarchar](512) NULL
			)", transaction: unitOfWork.Transaction);

        unitOfWork.Connection.Execute(@"
			IF OBJECT_ID('ClientCredentials') IS NULL
			CREATE TABLE [ClientCredentials](
				Id UNIQUEIDENTIFIER PRIMARY KEY,
				ClientCode NVARCHAR(50) NOT NULL,
				SecretHash NVARCHAR(255) NOT NULL,
				CreatedAt DATETIME NOT NULL,
				IsRevoked BIT NOT NULL
			)", transaction: unitOfWork.Transaction);

        unitOfWork.Connection.Execute(@"
			IF NOT EXISTS (SELECT 1 FROM ClientSources WHERE clientCode = 'CLIENT01')
			BEGIN
				INSERT INTO ClientSources (clientCode, clientName, Description, isActive)
				VALUES('CLIENT01','CLIENT 01', 'TEST 01', 1 )
			END

			IF NOT EXISTS (SELECT 1 FROM ClientSources WHERE clientCode = 'CLIENT02')
			BEGIN
				INSERT INTO ClientSources (clientCode, clientName, Description, isActive)
				VALUES('CLIENT02','CLIENT 02', 'TEST 02', 1 )
			END

			IF NOT EXISTS (SELECT 1 FROM ClientSources WHERE clientCode = 'CLIENT03')
			BEGIN
				INSERT INTO ClientSources (clientCode, clientName, Description, isActive)
				VALUES('CLIENT03','CLIENT 03', 'TEST 03', 0 )
			END
			", transaction: unitOfWork.Transaction);
        unitOfWork.Commit();
    }
}
