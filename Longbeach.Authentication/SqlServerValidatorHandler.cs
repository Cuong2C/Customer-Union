using Dapper;
using Longbeach.Infrastructure.Data;

namespace Longbeach.Authentication;

public class SqlServerValidatorHandler : ISqlServerValidatorHandler
{
    private readonly IDbConnectionFactory _connection;
    private readonly ITokenAuthenticationServices _tokenAuthenticationServices;

    public SqlServerValidatorHandler(IDbConnectionFactory connection, ITokenAuthenticationServices tokenAuthenticationServices)
    {
        _connection = connection;
        _tokenAuthenticationServices = tokenAuthenticationServices;
    }
    public bool Validate(string clientSource, string jti)
    {
        using var connection = _connection.CreateConnection();
        var query = "SELECT TOP 1 1 FROM ClientSources WHERE clientCode = @clientSource AND isActive = 1";

        if(!connection.ExecuteScalar<bool>(query, new { clientSource, jti })) return false;

        if(_tokenAuthenticationServices.IsRevokedToken(jti)) return false;

        return true;
    }
}
