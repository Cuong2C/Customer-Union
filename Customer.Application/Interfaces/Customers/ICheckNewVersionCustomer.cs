namespace Customer_Union.Application.Interfaces.Customers;

public interface ICheckNewVersionCustomer
{
    Task<bool> IsNewVersionCustomerAsync(Guid customerId, string hashCode);
}
