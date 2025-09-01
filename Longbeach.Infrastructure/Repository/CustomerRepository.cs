using Dapper;
using Longbeach.Domain.Entities;
using Longbeach.Domain.IRepository;
using Longbeach.Infrastructure.Data;
using Microsoft.Identity.Client;

namespace Longbeach.Infrastructure.Repository;

public class CustomerRepository(IUnitOfWork unitOfWork) : ICustomerRepository
{
    private const string INSERT_CUSTOMER_QUERY = @"
        INSERT INTO Customers (
            Id, Name, TaxCode, Address, Phone, Phone2, Phone3, Email, Nationality, Province, District, Gender, DateOfBirth, 
            BankAccount, BankName, CustomerType, PearlCustomerCode, CreatedAt, UpdatedAt, CreatedClientSourceCode, UpdatedClientSourceCode, HashCode
        )
        VALUES (
            @Id, @Name, @TaxCode, @Address, @Phone, @Phone2, @Phone3, @Email, @Nationality, @Province, @District, @Gender, @DateOfBirth, 
            @BankAccount, @BankName, @CustomerType, @PearlCustomerCode, @CreatedAt, @UpdatedAt, @CreatedClientSourceCode, @UpdatedClientSourceCode, @HashCode
        )";

    private const string GET_CUSTOMER_BY_ID_QUERY = @"
            SELECT Id, Name, TaxCode, Address, Phone, Phone2, Phone3,
            Email, Nationality, Province, District, Gender, DateOfBirth, 
            BankAccount, BankName, CustomerType, PearlCustomerCode,
            CreatedAt, UpdatedAt, CreatedClientSourceCode, UpdatedClientSourceCode, HashCode FROM Customers WHERE Id = @Id";

    private const string GET_CUSTOMER_BY_TAXCODE_QUERY = @"
            SELECT Id, Name, TaxCode, Address, Phone, Phone2, Phone3,
            Email, Nationality, Province, District, Gender, DateOfBirth, 
            BankAccount, BankName, CustomerType, PearlCustomerCode,
            CreatedAt, UpdatedAt, CreatedClientSourceCode, UpdatedClientSourceCode, HashCode FROM Customers WHERE TaxCode = @TaxCode";

    private const string GET_CUSTOMER_BY_PHONE_QUERY = @"
            SELECT Id, Name, TaxCode, Address, Phone, Phone2, Phone3,
            Email, Nationality, Province, District, Gender, DateOfBirth, 
            BankAccount, BankName, CustomerType, PearlCustomerCode,
            CreatedAt, UpdatedAt, CreatedClientSourceCode, UpdatedClientSourceCode, HashCode FROM Customers WHERE Phone = @Phone";

    private const string GET_CUSTOMERS_QUERY = @"
            SELECT TOP(@pageSize) Id, Name, TaxCode, Address, Phone, Phone2, Phone3,
            Email, Nationality, Province, District, Gender, DateOfBirth, 
            BankAccount, BankName, CustomerType, PearlCustomerCode,
            CreatedAt, UpdatedAt, CreatedClientSourceCode, UpdatedClientSourceCode, HashCode FROM Customers
            WHERE CreatedAt < @cursorDate OR (CreatedAt = @cursorDate AND Id < @cursorId)
            ORDER BY CreatedAt DESC, Id DESC";

    private const string GET_CUSTOMER_BY_PEARLCUSTOMERCODE_QUERY = @"
            SELECT Id, Name, TaxCode, Address, Phone, Phone2, Phone3,
            Email, Nationality, Province, District, Gender, DateOfBirth, 
            BankAccount, BankName, CustomerType, PearlCustomerCode,
            CreatedAt, UpdatedAt, CreatedClientSourceCode, UpdatedClientSourceCode, HashCode FROM Customers WHERE PearlCustomerCode = @PearlCustomerCode";

    private const string DELETE_CUSTOMER_QUERY = @"DELETE FROM Customers WHERE Id = @Id";

    private const string UPDATE_CUSTOMER_QUERY = @"
            UPDATE Customers SET Name = @Name, TaxCode = @TaxCode, Address = @Address, Phone = @Phone, Phone2 = @Phone2, Phone3 = @Phone3,
            Email = @Email, Nationality = @Nationality, Province = @Province, District = @District, Gender = @Gender, DateOfBirth = @DateOfBirth, 
            BankAccount = @BankAccount, BankName = @BankName, CustomerType = @CustomerType, PearlCustomerCode = @PearlCustomerCode,
            UpdatedAt = @UpdatedAt, UpdatedClientSourceCode = @UpdatedClientSourceCode, HashCode = @HashCode WHERE Id = @Id";

    private const string IS_NEW_VERSION_CUSTOMER_QUERY = @"
            SELECT TOP 1 1 FROM Customers WHERE Id = @Id AND HashCode = @HashCode";

    private const string GET_HASHCODE_BY_ID_QUERY = @"
            SELECT HashCode FROM Customers WHERE Id = @Id";

    public async Task<int> AddCustomerAsync(Customer customer)
    {
        var connection = unitOfWork.Connection;
        return await connection.ExecuteAsync(
            INSERT_CUSTOMER_QUERY,
            new { 
                customer.Id,
                customer.Name,
                customer.TaxCode,
                customer.Address,
                customer.Phone,
                customer.Phone2,
                customer.Phone3,
                customer.Email,
                customer.Nationality,
                customer.Province,
                customer.District,
                customer.Gender,
                customer.DateOfBirth,
                customer.BankAccount,
                customer.BankName,
                customer.CustomerType,
                customer.PearlCustomerCode,
                customer.CreatedAt,
                customer.UpdatedAt,
                customer.CreatedClientSourceCode,
                customer.UpdatedClientSourceCode,
                customer.HashCode
            },
            transaction: unitOfWork.Transaction);
    }

    public Task<int> DeleteCustomerAsync(Guid id)
    {
        var connection = unitOfWork.Connection;
        return connection.ExecuteAsync(
            DELETE_CUSTOMER_QUERY,
            new { Id = id },
            transaction: unitOfWork.Transaction);
    }

    public Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        throw new NotImplementedException(); // not in use currently
    }

    public async Task<IEnumerable<Customer>> GetCustomersByPhoneAsync(string phoneNumber)
    {
        var connection = unitOfWork.Connection;
        return await connection.QueryAsync<Customer>(
            GET_CUSTOMER_BY_PHONE_QUERY,
            new { Phone = phoneNumber },
            transaction: unitOfWork.Transaction);
    }

    public async Task<Customer?> GetCustomerByIdAsync(Guid id)
    {
        var connection = unitOfWork.Connection;
        return await connection.QueryFirstOrDefaultAsync<Customer>(
            GET_CUSTOMER_BY_ID_QUERY,
            new { Id = id },
            transaction: unitOfWork.Transaction);
    }

    public async Task<Customer?> GetCustomerByTaxcodeAsync(string taxCode)
    {
        var connection = unitOfWork.Connection;
        return await connection.QueryFirstOrDefaultAsync<Customer>(
            GET_CUSTOMER_BY_TAXCODE_QUERY,
            new { TaxCode = taxCode },
            transaction: unitOfWork.Transaction);
    }

    public async Task<Customer?> GetCustomerByPearlCustomerCodeAsync(string pearlCustomerCode)
    {
        var connection = unitOfWork.Connection;
        return await connection.QueryFirstOrDefaultAsync<Customer>(
            GET_CUSTOMER_BY_PEARLCUSTOMERCODE_QUERY,
            new { PearlCustomerCode = pearlCustomerCode },
            transaction: unitOfWork.Transaction);
    }

    public async Task<string> GetHashCodeByIdAsync(Guid id)
    {
        var connection = unitOfWork.Connection;
        var result =  await connection.ExecuteScalarAsync<string>(
            GET_HASHCODE_BY_ID_QUERY,
            new { Id = id },
            transaction: unitOfWork.Transaction);

        return result ?? string.Empty;
    }

    public async Task<bool> IsNewVersionCustomerAsync(Guid id, string hashCode)
    {
        var connection = unitOfWork.Connection;
        return await connection.ExecuteScalarAsync<bool>(
            IS_NEW_VERSION_CUSTOMER_QUERY,
            new { Id = id, HashCode = hashCode },
            transaction: unitOfWork.Transaction);
    }

    public async Task<int> UpdateCustomerAsync(Customer customer)
    {
        var connection = unitOfWork.Connection;
        return await connection.ExecuteAsync(
            UPDATE_CUSTOMER_QUERY,
            new {
                customer.Id,
                customer.Name,
                customer.TaxCode,
                customer.Address,
                customer.Phone,
                customer.Phone2,
                customer.Phone3,
                customer.Email,
                customer.Nationality,
                customer.Province,
                customer.District,
                customer.Gender,
                customer.DateOfBirth,
                customer.BankAccount,
                customer.BankName,
                customer.CustomerType,
                customer.PearlCustomerCode,
                customer.UpdatedAt,
                customer.UpdatedClientSourceCode,
                customer.HashCode
            },
            transaction: unitOfWork.Transaction);
    }

    public async Task<IEnumerable<Customer>> GetCustomersAsync(DateTime? cursorDate, Guid? cursorId, int pageSize)
    {
        var connection = unitOfWork.Connection;
        return await connection.QueryAsync<Customer>(
            GET_CUSTOMERS_QUERY,
            new { cursorDate, cursorId, pageSize },
            transaction: unitOfWork.Transaction);
    }
}
