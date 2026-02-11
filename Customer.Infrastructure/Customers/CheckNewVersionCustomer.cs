using Customer_Union.Application.Interfaces.Customers;
using Customer_Union.Domain.IRepository;

namespace Customer_Union.Infrastructure.Customers;

public class CheckNewVersionCustomer(ICustomerRepository customerRepository) : ICheckNewVersionCustomer
{
    public async Task<bool> IsNewVersionCustomerAsync(Guid customerId, string hashCode)
    {
        var result = await customerRepository.IsNewVersionCustomerAsync(customerId, hashCode);

        return result;
    }
}
