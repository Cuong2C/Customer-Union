using Longbeach.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Longbeach.Domain.IRepository
{
    public interface ICustomerHistoryRepository
    {
        Task<int> AddCustomerHistoryAsync(CustomerHistory customerHistory);
    }
}
