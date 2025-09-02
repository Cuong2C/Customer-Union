using Longbeach_Customer.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Longbeach_Customer.Services.IServices
{
    public interface ICustomerServices
    {
        Task<Results<Ok<IEnumerable<CustomerResponse>>, NotFound>> GetCustomerByPhoneAsync(string phoneNumber);
        Task<Results<Ok<CustomerResponse>, NotFound>> GetCustomerByIdAsync(Guid id);
        Task<Results<Ok<CustomerResponse>, NotFound>> GetCustomerByTaxcodeAsync(string taxcode);
        Task<Results<Ok<CustomerResponse>, NotFound>> GetCustomerByPearlCustomerCodeAsync(string pearlCustomerCode);
        Task<Results<Ok<PagedResult<CustomerResponse>>, NotFound>> GetCustomersAsync(DateTime? cursorDate, Guid? cursorId, int pageSize, string direction);
        Task<Results<Ok, NotFound>> IsNewVersionCustomerAsync(Guid id, string hashCode);
        Task<Results<Ok<HashCodeResponse>, BadRequest>> AddCustomerAsync(HttpContext httpContext, CustomerRequest customerRequest);
        Task<Results<Ok<HashCodeResponse>, BadRequest>> UpdateCustomerAsync(HttpContext httpContext, CustomerRequest customerRequest, Guid id);
        Task<Results<Ok, BadRequest>> DeleteCustomerAsync(HttpContext httpContext, Guid id);

    }
}
