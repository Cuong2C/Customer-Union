namespace Customer_Union.Application.Interfaces.Customers;

public interface IDeleteCustomer
{
    Task<int> DeleteCustomerAsync(Guid id, string clientSourceCode);
}
