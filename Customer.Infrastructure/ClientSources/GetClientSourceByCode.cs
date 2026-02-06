using Customer_Union.Application.Interfaces.ClientSources;
using Customer_Union.Domain.Entities;
using Customer_Union.Domain.IRepository;
using Microsoft.Extensions.Logging;

namespace Customer_Union.Infrastructure.ClientSources;

public class GetClientSourceByCode(ILogger<GetClientSourceByCode> logger, IClientSourceRepository clientSourceRepository) : IGetClientSourceByCode
{
    public async Task<ClientSource?> GetClientSourceByCodeAsync(string clientSourceCode)
    {
        var clientSource = await clientSourceRepository.GetClientSourceByCodeAsync(clientSourceCode);

        return clientSource;
    }
}
