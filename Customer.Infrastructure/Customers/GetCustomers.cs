using Customer_Union.Application.Dtos;
using Customer_Union.Application.Interfaces.Customers;
using Customer_Union.Domain.Entities;
using Customer_Union.Domain.IRepository;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Customer_Union.Infrastructure.Customers;

public class GetCustomers(IDistributedCache cache, ICustomerRepository customerRepository) : IGetCustomers
{
    public async Task<PagedResult<Customer>> GetCustomersAsync(DateTime? cursorDate, Guid? cursorId, int pageSize = 20, string direction = "next")
    {
        string key = $"customer:{cursorDate}:{cursorId}:{pageSize}:{direction}";
        var cachedCustomers = await cache.GetStringAsync(key);

        if (!string.IsNullOrEmpty(cachedCustomers))
        {
            var redisResult = JsonSerializer.Deserialize<PagedResult<Customer>>(cachedCustomers);
            if (redisResult != null)
            {
                return redisResult;
            }
        }

        var customers = await customerRepository.GetCustomersAsync(cursorDate, cursorId, pageSize + 1, direction);
        if (!customers.Any())
        {
            var emptyResult = new PagedResult<Customer>
            {
                Items = new List<Customer>(),
                NextCursor = null,
                PreviousCursor = null,
                HasMore = false,
                pageSize = pageSize
            };

            return emptyResult;
        }

        var pagedResult = new PagedResult<Customer>
        {
            Items = direction == "next" ? customers.Take(pageSize).ToList() : customers.Take(pageSize).Reverse().ToList(),
            HasMore = direction == "next" ? customers.Count() > pageSize : true,
            pageSize = pageSize
        };

        if (pagedResult.Items.Any())
        {
            var lastCustomer = pagedResult.Items.Last();
            pagedResult.NextCursor = new CustomerCursor
            {
                CursorDate = lastCustomer.CreatedAt,
                CursorId = lastCustomer.Id
            };

            var firstCustomer = pagedResult.Items.First();
            pagedResult.PreviousCursor = new CustomerCursor
            {
                CursorDate = firstCustomer.CreatedAt,
                CursorId = firstCustomer.Id
            };
        }

        cachedCustomers = JsonSerializer.Serialize(pagedResult);
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15),
        };
        await cache.SetStringAsync(key, cachedCustomers, cacheOptions);

        return pagedResult;
    }
}
