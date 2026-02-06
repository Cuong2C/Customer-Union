namespace Customer_Union.Application.Interfaces.ClientSources;

public interface IDeleteClientSource
{
    Task<bool> DeleteClientSourceAsync(string clientSourceCode);
}
