using Longbeach.Domain.Entities;

namespace Longbeach.Domain.IRepository;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetAllCustomersAsync();
    Task<Customer?> GetCustomerByIdAsync(Guid id);
    Task<IEnumerable<Customer>> GetCustomersByPhoneAsync(string phoneNumber);
    Task<Customer?> GetCustomerByTaxcodeAsync(string taxCode);
    Task<Customer?> GetCustomerByPearlCustomerCodeAsync(string pearlCustomerCode);
    Task<string> GetHashCodeByIdAsync(Guid id);
    Task<bool> IsNewVersionCustomerAsync(Guid id, string hashCode);
    Task<int> AddCustomerAsync(Customer customer);
    Task<int> UpdateCustomerAsync(Customer customer);
    Task<int> DeleteCustomerAsync(Guid id);

}
