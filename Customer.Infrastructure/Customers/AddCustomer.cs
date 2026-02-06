using Customer_Union.Application.Dtos;
using Customer_Union.Application.Interfaces.Customers;
using Customer_Union.Domain.Entities;
using Customer_Union.Domain.IRepository;
using Customer_Union.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Customer_Union.Infrastructure.Customers;

public class AddCustomer(ICustomerRepository customerRepository, ILogger<AddCustomer> logger, IUnitOfWork unitOfWork) : IAddCustomer
{
    public async Task<HashCodeResponse?> AddCustomerAsync(Customer customer)
    {
        if (string.IsNullOrEmpty(customer.Name))
        {
            logger.LogError("Customer name is required.");
            return null;
        }

        if (!string.IsNullOrEmpty(customer.TaxCode))
        {
            var customerByTaxcode = await customerRepository.GetCustomerByTaxcodeAsync(customer.TaxCode);
            if (customerByTaxcode != null)
            {
                logger.LogError("Customer with TaxCode {TaxCode} already exists.", customer.TaxCode);
                return null;
            }
        }

        await customerRepository.AddCustomerAsync(customer);
        unitOfWork.Commit();

        return new HashCodeResponse
        {
            Id = customer.Id,
            HashCode = customer.HashCode
        };
    }
}
