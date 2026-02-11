using Customer_Union.Application.Dtos;
using Customer_Union.Domain.Entities;

namespace Customer_Union.Application.Interfaces.Customers;

public interface IUpdateCustomer
{
    Task<HashCodeResponse?> UpdateCustomerAsync(string clientSourceCode, Customer customer, Guid id);
}
