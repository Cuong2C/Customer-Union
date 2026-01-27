namespace Customer_Union.Domain.Entities;

public class CustomerHistory
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? TaxCode { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Phone2 { get; set; }
    public string? Phone3 { get; set; }
    public string? Email { get; set; }
    public string? Nationality { get; set; }
    public string? Province { get; set; }
    public string? District { get; set; }
    public int Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? BankAccount { get; set; }
    public string? BankName { get; set; }
    public string? CustomerType { get; set; }
    public string? PearlCustomerCode { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string CreatedClientSourceCode { get; set; } = default!;
    public string UpdatedClientSourceCode { get; set; } = default!;
    public string HashCode { get; set; } = default!;
    public DateTime ActionTime { get; set; }
    public string AddedByAction { get; set; } = default!;
    public string ChangedByClient { get; set; } = default!;

    public void SetAdditionalProperties(string clientSourceCode, string action)
    {
        AddedByAction = action;
        ChangedByClient = clientSourceCode;
        ActionTime = DateTime.Now;
    }
}

public static class CustomerHistoryAddedByAction
{
    public static string Updated = "Updated";
    public static string Deleted = "Deleted";
}


