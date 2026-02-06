using Customer_Union.Domain.Entities;

namespace Customer_Union.Application.Interfaces.Customers;

public interface IGetCustomerByPearlCode
{
    Task<Customer?> GetCustomerByPearlCustomerCodeAsync(string pearlCustomerCode);
}
