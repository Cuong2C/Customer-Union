namespace Customer_Union.Application.Dtos;

public class GenrateTokenRequest
{
    public string ClientCode { get; set; } = default!;
    public string ClientSecret { get; set; } = default!;
}
