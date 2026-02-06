using Customer_Union.Domain.Entities;

namespace Customer_Union.Application.Interfaces.Customers;

public interface IGetCustomerById
{
    Task<Customer?> GetCustomerByIdAsync(Guid id);
}
