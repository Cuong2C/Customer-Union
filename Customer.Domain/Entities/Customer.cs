using System.Security.Cryptography;
using System.Text;

namespace Customer_Union.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }
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

        public void SetAdditionalProperties(string clientSourceCode)
        {
            Id = Guid.NewGuid();
            CreatedClientSourceCode = clientSourceCode;
            UpdatedClientSourceCode = clientSourceCode;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            SetHashCode();
        }

        public void SetUpdateProperties(string clientSourceCode)
        {
            UpdatedClientSourceCode = clientSourceCode;
            UpdatedAt = DateTime.Now;
            SetHashCode();
        }

        private void SetHashCode()
        {
           var stringBuilder = new StringBuilder();
            stringBuilder.Append(Name);
            stringBuilder.Append(TaxCode);
            stringBuilder.Append(Address);
            stringBuilder.Append(Phone);
            stringBuilder.Append(Phone2);
            stringBuilder.Append(Phone3);
            stringBuilder.Append(Email);
            stringBuilder.Append(Nationality);
            stringBuilder.Append(Province);
            stringBuilder.Append(District);
            stringBuilder.Append(Gender);
            stringBuilder.Append(DateOfBirth);
            stringBuilder.Append(BankAccount);
            stringBuilder.Append(BankName);
            stringBuilder.Append(CustomerType);
            stringBuilder.Append(PearlCustomerCode);

            HashCode = ComputeMD5Hash(stringBuilder.ToString());
        }

        private string ComputeMD5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

    }
}
