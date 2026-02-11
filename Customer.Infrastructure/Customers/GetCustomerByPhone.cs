using Customer_Union.Application.Interfaces.Customers;
using Customer_Union.Domain.Entities;
using Customer_Union.Domain.IRepository;

namespace Customer_Union.Infrastructure.Customers;

public class GetCustomerByPhone(ICustomerRepository customerRepository) : IGetCustomerByPhone
{
    public Task<IEnumerable<Customer>> GetCustomerByPhoneAsync(string phoneNumber)
    {
        var customers = customerRepository.GetCustomersByPhoneAsync(phoneNumber);

        return customers;
    }
}