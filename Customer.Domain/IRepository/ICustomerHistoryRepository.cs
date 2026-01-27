using Customer_Union.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer_Union.Domain.IRepository
{
    public interface ICustomerHistoryRepository
    {
        Task<int> AddCustomerHistoryAsync(CustomerHistory customerHistory);
    }
}
