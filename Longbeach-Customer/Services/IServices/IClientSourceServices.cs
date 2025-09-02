using Microsoft.AspNetCore.Http.HttpResults;

namespace Longbeach_Customer.Services.IServices
{
    public interface IClientSourceServices
    {
        Task<Ok<IEnumerable<ClientSource>>> GetAllClientSourcesAsync();
        Task<Results<Ok<ClientSource>, NotFound>> GetClientSourceByCodeAsync(string clientSourceCode);
        Task<Results<Ok, BadRequest>> AddClientSourceAsync(ClientSource clientSource);
        Task<Results<Ok, BadRequest>> UpdateClientSourceAsync(ClientSource clientSource);
        Task<Results<Ok, NotFound>> DeleteClientSourceAsync(string clientSourceCode);
    }
}
