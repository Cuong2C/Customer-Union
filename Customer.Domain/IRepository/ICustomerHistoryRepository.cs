using Customer_Union.Domain.Entities;

namespace Customer_Union.Domain.IRepository
{
    public interface ICustomerHistoryRepository
    {
        Task<int> AddCustomerHistoryAsync(CustomerHistory customerHistory);
    }
}
