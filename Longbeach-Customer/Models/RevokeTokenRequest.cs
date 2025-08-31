namespace Longbeach_Customer.Models
{
    public class RevokeTokenRequest
    {
        public string ClientCode { get; set; } = default!; 
        public string ClientSecret { get; set; } = default!; 
    }
}
