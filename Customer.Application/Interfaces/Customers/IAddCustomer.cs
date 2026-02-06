using Customer_Union.Application.Dtos;
using Customer_Union.Domain.Entities;

namespace Customer_Union.Application.Interfaces.Customers;

public interface IAddCustomer
{
    Task<HashCodeResponse?> AddCustomerAsync(Customer customer);
}
