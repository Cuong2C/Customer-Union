using Customer_Union.Domain.Entities;

namespace Customer_Union.Application.Interfaces.ClientSources;

public interface IGetClientSourceByCode
{
    Task<ClientSource?> GetClientSourceByCodeAsync(string clientSourceCode);
}
