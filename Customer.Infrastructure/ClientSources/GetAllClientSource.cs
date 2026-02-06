using Customer_Union.Application.Interfaces.ClientSources;
using Customer_Union.Domain.Entities;
using Customer_Union.Domain.IRepository;
using Microsoft.Extensions.Logging;

namespace Customer_Union.Infrastructure.ClientSources;

public class GetAllClientSource(ILogger<GetAllClientSource> logger, IClientSourceRepository clientSourceRepository) : IGetAllClientSource
{
    public async Task<IEnumerable<ClientSource>> GetAllClientSourcesAsync()
    {
        var clientSources = await clientSourceRepository.GetAllClientSourcesAsync();

        return clientSources;
    }
}
