using System.Text.Json.Serialization;

namespace Longbeach_Customer.Models
{
    public class CustomerRequest
    {
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
        public string? HashCode { get; set; }
    }
}
