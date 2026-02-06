using Customer_Union.Domain.Entities;

namespace Customer_Union.Application.Interfaces.ClientSources;

public interface IGetAllClientSource
{
    Task<IEnumerable<ClientSource>> GetAllClientSourcesAsync();
}
