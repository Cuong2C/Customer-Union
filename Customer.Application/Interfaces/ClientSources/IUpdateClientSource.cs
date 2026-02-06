using Customer_Union.Domain.Entities;

namespace Customer_Union.Application.Interfaces.ClientSources;

public interface IUpdateClientSource
{
    Task<bool> UpdateClientSourceAsync(ClientSource clientSource);
}
