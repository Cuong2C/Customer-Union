using Customer_Union.Application.Dtos;

namespace Customer_Union.Application.Interfaces.Customers;

public interface IUpdateCustomer
{
    Task<HashCodeResponse> UpdateCustomerAsync(string clientSourceCode, CustomerRequest customerRequest, Guid id);
}
