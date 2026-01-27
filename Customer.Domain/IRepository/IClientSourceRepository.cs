using Customer_Union.Domain.Entities;

namespace Customer_Union.Domain.IRepository;

public interface IClientSourceRepository
{
    Task<IEnumerable<ClientSource>> GetAllClientSourcesAsync();
    Task<ClientSource?> GetClientSourceByCodeAsync(string clientCode);
    Task AddClientSourceAsync(ClientSource clientSource);
    Task<int> UpdateClientSourceAsync(ClientSource clientSource);
    Task<int> DeleteClientSourceAsync(string id);
    Task<bool> IsValidClientSource(string clientCode);
}
