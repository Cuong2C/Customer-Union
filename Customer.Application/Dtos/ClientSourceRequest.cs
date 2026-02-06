namespace Customer_Union.Application.Dtos;

public class ClientSourceRequest
{
    public string ClientCode { get; set; } = default!;
    public string ClientName { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}
