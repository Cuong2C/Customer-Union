using Customer_Union.Application.Interfaces.Customers;
using Customer_Union.Domain.Entities;
using Customer_Union.Domain.IRepository;
using Customer_Union.Infrastructure.Data;

namespace Customer_Union.Infrastructure.Customers;

public class DeleteCustomer(ICustomerRepository customerRepository, ICustomerHistoryRepository customerHistoryRepository, IUnitOfWork unitOfWork) : IDeleteCustomer
{
    public async Task<int> DeleteCustomerAsync(Guid id, string clientSourceCode)
    {
        var customerInDb = await customerRepository.GetCustomerByIdAsync(id);

        if (customerInDb is null)
        {
            return 0;
        }

        var result = await customerRepository.DeleteCustomerAsync(id);

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

        customerHistory.SetAdditionalProperties(clientSourceCode, CustomerHistoryAddedByAction.Deleted);

        await customerHistoryRepository.AddCustomerHistoryAsync(customerHistory);

        unitOfWork.Commit();

        return result;
    }
}
