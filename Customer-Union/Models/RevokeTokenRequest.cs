namespace Customer_Union.Models
{
    public class RevokeTokenRequest
    {
        public string ClientCode { get; set; } = default!; 
        public string ClientSecret { get; set; } = default!; 
    }
}
