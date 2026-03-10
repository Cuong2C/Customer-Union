using Customer_Union.Application.Interfaces.Customers;
using Customer_Union.Domain.Entities;
using Customer_Union.Domain.IRepository;

namespace Customer_Union.Infrastructure.Customers;

public class GetCustomerByTaxCode(ICustomerRepository customerRepository) : IGetCustomerByTaxCode
{
    public Task<Customer?> GetCustomerByTaxCodeAsync(string taxCode)
    {
        var result = customerRepository.GetCustomerByTaxcodeAsync(taxCode);

        return result;
    }
}
