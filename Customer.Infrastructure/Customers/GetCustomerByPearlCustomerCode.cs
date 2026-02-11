using Customer_Union.Application.Interfaces.Customers;
using Customer_Union.Domain.Entities;
using Customer_Union.Domain.IRepository;

namespace Customer_Union.Infrastructure.Customers;

public class GetCustomerByPearlCustomerCode(ICustomerRepository customerRepository) : IGetCustomerByPearlCode
{
    public async Task<Customer?> GetCustomerByPearlCustomerCodeAsync(string pearlCustomerCode)
    {
        var customer = await customerRepository.GetCustomerByPearlCustomerCodeAsync(pearlCustomerCode);

        return customer;
    }
}
