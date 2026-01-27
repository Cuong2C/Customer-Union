using Dapper;
using Customer_Union.Authentication;
using Customer_Union.Domain.Entities;
using Customer_Union.Infrastructure.Data;
using Customer_Union.Infrastructure.Repository;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer_Union.UnitTests
{
    public class AuthManagerUnitest
    {
        public AuthManagerUnitest()
        {
            SqlMapper.AddTypeHandler(new GuidTypeHandler());
            Batteries.Init();
        }

        [Fact]
        public async Task Create_ClientCredential_UnitTest()
        {
            var connectionFactory = new SqliteConnectionFactory();
            using var unitOfWork = new DapperUnitOfWork(connectionFactory);

            // Arrange
            var createTableSql = @"
                CREATE TABLE ClientCredentials (
                    Id TEXT NULL,
                    clientCode TEXT NULL,
                    secretHash TEXT NULL,
                    createdAt TEXT NULL,
                    IsRevoked INTEGER NULL
                );";
            unitOfWork.Connection.Execute(createTableSql, transaction: unitOfWork.Transaction);

            createTableSql = @"
            CREATE TABLE ClientSources (
            clientCode TEXT NOT NULL,
            clientName TEXT,
            Description TEXT,
            isActive INTEGER);";

            unitOfWork.Connection.Execute(createTableSql, transaction: unitOfWork.Transaction);

            var clientCode = "TestClientCode";
            var clientSource1 = new ClientSource
            {
                ClientCode = "CS001",
                ClientName = clientCode,
                Description = "This is a test client source.",
                IsActive = true
            };
            var clientSourceRepo = new ClientSourceRepository(unitOfWork);
            var clientCredentialRepo = new ClientCredentialRepository(clientCode, unitOfWork);

            // Act
            await clientSourceRepo.AddClientSourceAsync(clientSource1);

            var clientSecret = await clientCredentialRepo.CreateClientCredentialAsync(clientCode);
            var isValid = await clientCredentialRepo.ValidateClientSecretAsync(clientCode, clientSecret);

            // Assert
            Assert.False(string.IsNullOrEmpty(clientSecret), "Client secret should not be empty after creation.");
            Assert.True(isValid, "Client secret should be valid after creation.");

        }

        [Fact]
        public async Task Revoke_ClientCredential_UnitTest()
        {
            var connectionFactory = new SqliteConnectionFactory();
            using var unitOfWork = new DapperUnitOfWork(connectionFactory);
            // Arrange
            var createTableSql = @"
                CREATE TABLE ClientCredentials (
                    Id TEXT NULL,
                    clientCode TEXT NULL,
                    secretHash TEXT NULL,
                    createdAt TEXT NULL,
                    IsRevoked INTEGER NULL
                );";
            unitOfWork.Connection.Execute(createTableSql, transaction: unitOfWork.Transaction);
            createTableSql = @"
                CREATE TABLE ClientSources (
                clientCode TEXT NOT NULL,
                clientName TEXT,
                Description TEXT,
                isActive INTEGER);";
            unitOfWork.Connection.Execute(createTableSql, transaction: unitOfWork.Transaction);
            var clientCode = "TestClientCode";
            var clientSource1 = new ClientSource
            {
                ClientCode = clientCode,
                ClientName = clientCode,
                Description = "This is a test client source.",
                IsActive = true
            };
            var clientSourceRepo = new ClientSourceRepository(unitOfWork);
            var clientCredentialRepo = new ClientCredentialRepository(clientCode, unitOfWork);

            // Act
            await clientSourceRepo.AddClientSourceAsync(clientSource1);
            var clientSecret = await clientCredentialRepo.CreateClientCredentialAsync(clientCode);
            var isValidBeforeRevoke = await clientCredentialRepo.ValidateClientSecretAsync(clientCode, clientSecret);
            await clientCredentialRepo.RevokeClientCredentialsAsync(clientCode);
            var isValidAfterRevoke = await clientCredentialRepo.ValidateClientSecretAsync(clientCode, clientSecret);

            // Assert
            Assert.False(string.IsNullOrEmpty(clientSecret), "Client secret should not be empty after creation.");
            Assert.True(isValidBeforeRevoke, "Client secret should be valid before revocation.");
            Assert.False(isValidAfterRevoke, "Client secret should not be valid after revocation.");
        }

        [Fact]
        public void GenerateToken_UnitTest()
        {
            // Arrange
            var connectionFactory = new SqliteConnectionFactory();
            var tokenService = new TokenAuthenticationServices("adfaTJ34343KKD@ssssss8788llll@ggg", connectionFactory);

            // Act
            var token = tokenService.GenerateTokenAsync("TestClientCode");
            var isValid = tokenService.ValidateToken(token, out var securityToken, out var principal);

            // Assert
            Assert.False(string.IsNullOrEmpty(token));
            Assert.True(isValid);
        }

        [Fact]
        public void RevokeToken_UnitTest()
        {
            // Arrange

            var connectionFactory = new SqliteConnectionFactory();
            var tokenService = new TokenAuthenticationServices("adfaTJ34343KKD@ssssss8788llll@ggg", connectionFactory);

            var createTableSql = @"
                CREATE TABLE InvalidTokens (
                    jti TEXT NULL,
                    createdAt TEXT NULL,
                    note TEXT NULL
                );";
            connectionFactory.CreateConnection().Execute(createTableSql);

            // Act
            var token = tokenService.GenerateTokenAsync("TestClientCode");
            var isValid = tokenService.ValidateToken(token, out var securityToken, out var principal);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // get jti value from the token
            string jtiValue = jwtToken.Claims.FirstOrDefault(c => c.Type == "jti")?.Value ?? "";

            //*** for test purpose, we do not use using in RevokeTokenAsync method and IsInBackList method when create connection
            var isRevoked = tokenService.RevokeTokenAsync(jtiValue); 
            var isRevokedFalse = tokenService.IsRevokedToken("invalid-jti"); 
            var isRevokedTrue = tokenService.IsRevokedToken(jtiValue); 

            // Assert
            Assert.True(isValid);
            Assert.NotNull(securityToken);
            Assert.NotNull(principal);
            Assert.False(isRevokedFalse, "Token should not be revoked with an invalid JTI.");
            Assert.True(isRevokedTrue, "Token should be revoked with a valid JTI.");

        }
    }
}
