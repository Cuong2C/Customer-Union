using Customer_Union.Domain.Entities;

namespace Customer_Union.Application.Interfaces.Customers;

public interface IGetCustomerByPhone
{
    Task<IEnumerable<Customer>> GetCustomerByPhoneAsync(string phoneNumber);
}
