using Customer_Union.Application.Dtos;
using Customer_Union.Domain.Entities;

namespace Customer_Union.Application.Interfaces.Customers;

public interface IGetCustomer
{
    Task<PagedResult<Customer>> GetCustomersAsync(DateTime? cursorDate, Guid? cursorId, int pageSize = 20, string direction = "next");
}
