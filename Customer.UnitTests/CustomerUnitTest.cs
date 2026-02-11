using Dapper;
using Customer_Union.Domain.Entities;
using Customer_Union.Infrastructure.Data;
using Customer_Union.Infrastructure.Repository;
using SQLitePCL;

namespace Customer_Union.UnitTests;
// Query in Sqlite is different from SqlServer, if not cahnge the queries some bugs are appearing when running tests.
public class CustomerUnitTest
{
    public CustomerUnitTest()
    {
        SqlMapper.AddTypeHandler(new GuidTypeHandler());
        Batteries.Init();
    }

    [Fact]
    public async void Create_Customer_UnitTest()
    {
        // Arrange
        var connectionFactory = new SqliteConnectionFactory();
        using var unitOfWork = new DapperUnitOfWork(connectionFactory);

        var createTableSql = @"
                CREATE TABLE Customers (
                    Id TEXT NULL,
                    Name TEXT NULL,
                    TaxCode TEXT NULL,
                    Address TEXT NULL,
                    Phone TEXT NULL,
                    Phone2 TEXT NULL,
                    Phone3 TEXT NULL,
                    Email TEXT NULL,
                    Nationality TEXT NULL,
                    Province TEXT NULL,
                    District TEXT NULL,
                    Gender TEXT NULL,
                    DateOfBirth TEXT NULL, 
                    BankAccount TEXT NULL,
                    BankName TEXT NULL,
                    CustomerType TEXT NULL,
                    PearlCustomerCode TEXT NULL,
                    createdAt TEXT NULL, 
                    updatedAt TEXT NULL,
                    createdClientSourceCode TEXT NULL,
                    updatedClientSourceCode TEXT NULL,
                    hashCode TEXT NULL
                );";

        unitOfWork.Connection.Execute(createTableSql, transaction: unitOfWork.Transaction);

        var repo = new CustomerRepository(unitOfWork);

        var customer1 = new Customer()
        {
            Id = Guid.NewGuid(),
            Name = "Test Customer",
            TaxCode = "123456789",
            Address = "123 Test St",
            Phone = "1234567890",
            Phone2 = "0987654321",
            Phone3 = "1122334455",
            Email = null,
            Nationality = "VN",
            Province = "Hanoi",
            District = null,
            Gender = 2,
            DateOfBirth = DateTime.Now,
            BankAccount = "",
            BankName = "Test Bank",
            CustomerType = "Regular",
            PearlCustomerCode = "PEARL123",
        };
        customer1.SetAdditionalProperties("TestClientSource");

        var customer2 = new Customer()
        {
            Id = Guid.NewGuid(),
            Name = "Test Customer2",
            TaxCode = "",
            Address = "123 Test St",
            Phone = "1234567890",
            Phone2 = "0987654321",
            Phone3 = "1122334455",
            Email = null,
            Nationality = "VN",
            Province = "TPHCM",
            District = null,
            Gender = 2,
            DateOfBirth = DateTime.Now,
            BankAccount = "",
            BankName = "Test Bank",
            CustomerType = "Regular",
            PearlCustomerCode = null,
        };
        customer1.SetAdditionalProperties("TestClientSource");

        // Act
        var hashcode1 = await repo.AddCustomerAsync(customer1);
        var hashcode2 = await repo.AddCustomerAsync(customer2);

        var retrievedCustomer1 = await repo.GetCustomerByPearlCustomerCodeAsync(customer1.PearlCustomerCode);
        var retrievedCustomer1_2 = await repo.GetCustomerByIdAsync(customer1.Id);
        var retrievedCustomer2 = await repo.GetCustomerByIdAsync(customer2.Id);
        var retrievedCustomerByPhone = await repo.GetCustomersByPhoneAsync(customer2.Phone);
        //var allCustomers = await repo.GetAllCustomersAsync();

        // Assert
        Assert.NotNull(retrievedCustomer1);
        Assert.NotNull(retrievedCustomer1_2);
        Assert.NotNull(retrievedCustomer2);

        Assert.Equal(customer1.Name, retrievedCustomer1!.Name);
        Assert.Equal(customer1.Name, retrievedCustomer1_2!.Name);

        Assert.Equal(customer2.Name, retrievedCustomer2!.Name);
        Assert.Contains(retrievedCustomerByPhone, u => u.Id == customer1.Id);
        Assert.Contains(retrievedCustomerByPhone, u => u.Id == customer2.Id);
    }

    [Fact]
    public async void Create_Update_Customer_UnitTest()
    {
        var connectionFactory = new SqliteConnectionFactory();
        using var unitOfWork = new DapperUnitOfWork(connectionFactory);

        // Arrange
        var createTableSql = @"
                CREATE TABLE Customers (
                    Id TEXT NULL,
                    Name TEXT NULL,
                    TaxCode TEXT NULL,
                    Address TEXT NULL,
                    Phone TEXT NULL,
                    Phone2 TEXT NULL,
                    Phone3 TEXT NULL,
                    Email TEXT NULL,
                    Nationality TEXT NULL,
                    Province TEXT NULL,
                    District TEXT NULL,
                    Gender TEXT NULL,
                    DateOfBirth TEXT NULL, 
                    BankAccount TEXT NULL,
                    BankName TEXT NULL,
                    CustomerType TEXT NULL,
                    PearlCustomerCode TEXT NULL,
                    createdAt TEXT NULL, 
                    updatedAt TEXT NULL,
                    createdClientSourceCode TEXT NULL,
                    updatedClientSourceCode TEXT NULL,
                    hashCode TEXT NULL
                );";
        unitOfWork.Connection.Execute(createTableSql, transaction: unitOfWork.Transaction);

        var repo = new CustomerRepository(unitOfWork);

        var customer1 = new Customer()
        {
            Id = Guid.NewGuid(),
            Name = "Test Customer",
            TaxCode = "123456789",
            Address = "123 Test St",
            Phone = "1234567890",
            Phone2 = "0987654321",
            Phone3 = "1122334455",
            Email = null,
            Nationality = "VN",
            Province = "Hanoi",
            District = null,
            Gender = 1,
            DateOfBirth = DateTime.Now,
            BankAccount = "",
            BankName = "Test Bank",
            CustomerType = "Regular",
            PearlCustomerCode = "PEARL123",
        };
        customer1.SetAdditionalProperties("TestClientSource");

        var customer2 = new Customer()
        {
            Id = Guid.NewGuid(),
            Name = "Test Customer2",
            TaxCode = "",
            Address = "123 Test St",
            Phone = "1234567890",
            Phone2 = "0987654321",
            Phone3 = "1122334455",
            Email = null,
            Nationality = "VN",
            Province = "TPHCM",
            District = null,
            Gender = 2,
            DateOfBirth = DateTime.Now,
            BankAccount = "",
            BankName = "Test Bank",
            CustomerType = "Regular",
            PearlCustomerCode = null,
        };
        customer1.SetAdditionalProperties("TestClientSource");

        // Act
        var hashcode1 = await repo.AddCustomerAsync(customer1);
        var hashcode2 = await repo.AddCustomerAsync(customer2);

        var retrievedCustomer1 = await repo.GetCustomerByPearlCustomerCodeAsync(customer1.PearlCustomerCode);
        var retrievedCustomer1_2 = await repo.GetCustomerByIdAsync(customer1.Id);
        var retrievedCustomer2 = await repo.GetCustomerByIdAsync(customer2.Id);


        // Assert
        Assert.NotNull(retrievedCustomer1);
        Assert.NotNull(retrievedCustomer1_2);
        Assert.NotNull(retrievedCustomer2);
        Assert.Equal(retrievedCustomer1.Id, customer1.Id);
        Assert.Equal(customer1.Name, retrievedCustomer1!.Name);
        Assert.Equal(retrievedCustomer2.Id, customer2.Id);

        // Arrange
        customer1.Name = "Updated Customer Name";
        customer1.Phone = "000000012424";
        customer1.SetUpdateProperties("TestClientSource");
        customer2.Name = "Updated Customer Name 2";
        customer2.Address = "456 test2";
        customer2.SetUpdateProperties("TestClientSource");

        // Act
        await repo.UpdateCustomerAsync(customer1);
        var updatedCustomer1 = await repo.GetCustomerByIdAsync(customer1.Id);
        await repo.UpdateCustomerAsync(customer2);
        var updatedCustomer2 = await repo.GetCustomerByIdAsync(customer2.Id);

        // Assert
        Assert.NotNull(updatedCustomer1);
        Assert.NotNull(updatedCustomer2);
        Assert.Equal(retrievedCustomer1.Id, updatedCustomer1!.Id);
        Assert.Equal(retrievedCustomer2.Id, updatedCustomer2!.Id);
        Assert.NotEqual(retrievedCustomer1.Name, updatedCustomer1!.Name);
        Assert.NotEqual(retrievedCustomer2.Name, updatedCustomer2!.Name);
        Assert.NotEqual(retrievedCustomer1.Phone, updatedCustomer1!.Phone);
        Assert.Equal(retrievedCustomer2.Phone, updatedCustomer2!.Phone);
        Assert.NotEqual(retrievedCustomer2.Address, updatedCustomer2!.Address);

    }

    [Fact]
    public async void Create_CustomerHistory_UniTest()
    {
        var connectionFactory = new SqliteConnectionFactory();
        using var unitOfWork = new DapperUnitOfWork(connectionFactory);
        // Arrange
        var createTableSql = @"
                CREATE TABLE CustomerHistories (
                    Id TEXT NULL,
                    Name TEXT NULL,
                    TaxCode TEXT NULL,
                    Address TEXT NULL,
                    Phone TEXT NULL,
                    Phone2 TEXT NULL,
                    Phone3 TEXT NULL,
                    Email TEXT NULL,
                    Nationality TEXT NULL,
                    Province TEXT NULL,
                    District TEXT NULL,
                    Gender TEXT NULL,
                    DateOfBirth TEXT NULL, 
                    BankAccount TEXT NULL,
                    BankName TEXT NULL,
                    CustomerType TEXT NULL,
                    PearlCustomerCode TEXT NULL,
                    createdAt TEXT NULL, 
                    updatedAt TEXT NULL,
                    createdClientSourceCode TEXT NULL,
                    updatedClientSourceCode TEXT NULL,
                    hashCode TEXT NULL,
                    actionTime TEXT NULL,
	                AddedByAction TEXT NULL,
	                changedByClient TEXT NULL
                );";
        unitOfWork.Connection.Execute(createTableSql, transaction: unitOfWork.Transaction);

        var repo = new CustomerHistoryRepository(unitOfWork);

        var customerHistory = new CustomerHistory()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Test Customer",
            TaxCode = "123456789",
            Address = "123 Test St",
            Phone = "1234567890",
            Phone2 = "0987654321",
            Phone3 = "1122334455",
            Email = null,
            Nationality = "VN",
            Province = "Hanoi",
            District = null,
            Gender = 2,
            DateOfBirth = DateTime.Now,
            BankAccount = "",
            BankName = "Test Bank",
            CustomerType = "Regular",
            PearlCustomerCode = "PEARL123",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            CreatedClientSourceCode = "TestClientSource",
            UpdatedClientSourceCode = "TestClientSource",
            HashCode = "TestHashCode",
            ActionTime = DateTime.Now,
            AddedByAction = CustomerHistoryAddedByAction.Updated,
            ChangedByClient = "TestClientSource"
        };

        // Act
        var result = await repo.AddCustomerHistoryAsync(customerHistory);

        // Assert
        Assert.Equal(1, result);

    }

    [Fact]
    public async void Create_Delete_Customer_UnitTest()
    {
        var connectionFactory = new SqliteConnectionFactory();
        using var unitOfWork = new DapperUnitOfWork(connectionFactory);

        // Arrange
        var createTableSql = @"
                CREATE TABLE Customers (
                    Id TEXT NULL,
                    Name TEXT NULL,
                    TaxCode TEXT NULL,
                    Address TEXT NULL,
                    Phone TEXT NULL,
                    Phone2 TEXT NULL,
                    Phone3 TEXT NULL,
                    Email TEXT NULL,
                    Nationality TEXT NULL,
                    Province TEXT NULL,
                    District TEXT NULL,
                    Gender TEXT NULL,
                    DateOfBirth TEXT NULL, 
                    BankAccount TEXT NULL,
                    BankName TEXT NULL,
                    CustomerType TEXT NULL,
                    PearlCustomerCode TEXT NULL,
                    createdAt TEXT NULL, 
                    updatedAt TEXT NULL,
                    createdClientSourceCode TEXT NULL,
                    updatedClientSourceCode TEXT NULL,
                    hashCode TEXT NULL
                );";
        unitOfWork.Connection.Execute(createTableSql, transaction: unitOfWork.Transaction);

        var repo = new CustomerRepository(unitOfWork);

        var customer1 = new Customer()
        {
            Id = Guid.NewGuid(),
            Name = "Test Customer",
            TaxCode = "123456789",
            Address = "123 Test St",
            Phone = "1234567890",
            Phone2 = "0987654321",
            Phone3 = "1122334455",
            Email = null,
            Nationality = "VN",
            Province = "Hanoi",
            District = null,
            Gender = 2,
            DateOfBirth = DateTime.Now,
            BankAccount = "",
            BankName = "Test Bank",
            CustomerType = "Regular",
            PearlCustomerCode = "PEARL123",
        };
        customer1.SetAdditionalProperties("TestClientSource");

        var customer2 = new Customer()
        {
            Id = Guid.NewGuid(),
            Name = "Test Customer2",
            TaxCode = "",
            Address = "123 Test St",
            Phone = "1234567890",
            Phone2 = "0987654321",
            Phone3 = "1122334455",
            Email = null,
            Nationality = "VN",
            Province = "TPHCM",
            District = null,
            Gender = 2,
            DateOfBirth = DateTime.Now,
            BankAccount = "",
            BankName = "Test Bank",
            CustomerType = "Regular",
            PearlCustomerCode = null,
        };
        customer1.SetAdditionalProperties("TestClientSource");

        // Act
        var hashcode1 = await repo.AddCustomerAsync(customer1);
        var hashcode2 = await repo.AddCustomerAsync(customer2);

        var retrievedCustomer1 = await repo.GetCustomerByIdAsync(customer1.Id);
        var retrievedCustomer2 = await repo.GetCustomerByIdAsync(customer2.Id);

        // Assert
        Assert.NotNull(retrievedCustomer1);
        Assert.NotNull(retrievedCustomer2);
        Assert.Equal(retrievedCustomer1.Id, customer1.Id);
        Assert.Equal(customer1.Name, retrievedCustomer1!.Name);
        Assert.Equal(retrievedCustomer2.Id, customer2.Id);
        Assert.Equal(customer2.Name, retrievedCustomer2!.Name);

        // Act
        var deletedCount = await repo.DeleteCustomerAsync(customer1.Id);
        retrievedCustomer2 = await repo.GetCustomerByIdAsync(customer2.Id);
        var deletedCount2 = await repo.DeleteCustomerAsync(customer2.Id);

        // Assert
        Assert.Equal(1, deletedCount);
        Assert.Equal(1, deletedCount2);
        Assert.NotNull(retrievedCustomer2);

        // Act
        var deletedCustomer1 = await repo.GetCustomerByIdAsync(customer1.Id);
        var deletedCustomer2 = await repo.GetCustomerByIdAsync(customer2.Id);

        // Assert
        Assert.Null(deletedCustomer1);
        Assert.Null(deletedCustomer2);
    }

    
}
