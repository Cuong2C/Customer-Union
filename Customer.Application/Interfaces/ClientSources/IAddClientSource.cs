using Customer_Union.Domain.Entities;

namespace Customer_Union.Application.Interfaces.ClientSources;

public interface IAddClientSource
{
    Task<bool> AddClientSourceAsync(ClientSource clientSource);
}
