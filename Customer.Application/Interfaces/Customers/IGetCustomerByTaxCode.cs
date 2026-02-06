using Customer_Union.Domain.Entities;

namespace Customer_Union.Application.Interfaces.Customers;

public interface IGetCustomerByTaxCode
{
    Task<Customer?> GetCustomerByTaxCodeAsync(string taxCode);
}
