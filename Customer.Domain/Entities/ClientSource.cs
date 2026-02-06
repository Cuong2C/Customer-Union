namespace Customer_Union.Domain.Entities;

public class ClientSource
{
    public string ClientCode { get; set; } = default!;
    public string ClientName { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}
