using Customer_Union.Application.Interfaces.Customers;
using Customer_Union.Domain.Entities;
using Customer_Union.Domain.IRepository;

namespace Customer_Union.Infrastructure.Customers;

public class GetCustomerById(ICustomerRepository customerRepository) : IGetCustomerById
{
    public async Task<Customer?> GetCustomerByIdAsync(Guid id)
    {
        var customer = await customerRepository.GetCustomerByIdAsync(id);

        return customer;
    }
}
