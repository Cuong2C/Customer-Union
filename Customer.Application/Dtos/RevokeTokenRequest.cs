namespace Customer_Union.Application.Dtos;

public class RevokeTokenRequest
{
    public string ClientCode { get; set; } = default!; 
    public string ClientSecret { get; set; } = default!; 
}
