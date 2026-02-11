using Customer_Union.Application.Dtos;
using Customer_Union.Application.Interfaces.Customers;
using Customer_Union.Domain.Entities;
using Customer_Union.Domain.IRepository;
using Customer_Union.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Customer_Union.Infrastructure.Customers;

public class UpdateCustomer(ICustomerRepository customerRepository, ICustomerHistoryRepository customerHistoryRepository, IUnitOfWork unitOfWork, ILogger<UpdateCustomer> logger): IUpdateCustomer
{
    public async Task<HashCodeResponse?> UpdateCustomerAsync(string clientSourceCode, Customer customer, Guid id)
    {
        var customerInDb = await customerRepository.GetCustomerByIdAsync(id);
        if (customerInDb == null)
        {
            logger.LogError($"Customer with id {id} not found.");
            return null;
        }

        if (customerInDb.HashCode != customer.HashCode)
        {
            logger.LogError($"Hash code mismatch for customer with id {id}");
            return null;
        }

        var customerHistory = new CustomerHistory
        {
            Id = customerInDb.Id.ToString(),
            Name = customerInDb.Name,
            TaxCode = customerInDb.TaxCode,
            Address = customerInDb.Address,
            Phone = customerInDb.Phone,
            Phone2 = customerInDb.Phone2,
            Phone3 = customerInDb.Phone3,
            Email = customerInDb.Email,
            Nationality = customerInDb.Nationality,
            Province = customerInDb.Province,
            District = customerInDb.District,
            Gender = customerInDb.Gender,
            DateOfBirth = customerInDb.DateOfBirth,
            BankAccount = customerInDb.BankAccount,
            BankName = customerInDb.BankName,
            CustomerType = customerInDb.CustomerType,
            PearlCustomerCode = customerInDb.PearlCustomerCode,
            CreatedAt = customerInDb.CreatedAt,
            UpdatedAt = customerInDb.UpdatedAt,
            CreatedClientSourceCode = customerInDb.CreatedClientSourceCode,
            UpdatedClientSourceCode = customerInDb.UpdatedClientSourceCode,
            HashCode = customerInDb.HashCode
        };

        customer.SetUpdateProperties(clientSourceCode);
        customerHistory.SetAdditionalProperties(clientSourceCode, CustomerHistoryAddedByAction.Updated);

        await customerRepository.UpdateCustomerAsync(customer);
        await customerHistoryRepository.AddCustomerHistoryAsync(customerHistory);
        unitOfWork.Commit();

        return new HashCodeResponse
        {
            Id = customer.Id,
            HashCode = customer.HashCode
        };
    }
}
