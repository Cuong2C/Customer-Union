using System.Text;

namespace Longbeach_Customer.Models
{
    public class GenrateTokenResponse
    {
        public string ClientCode { get; set; } = default!;
        public string Token { get; set; } = default!;
    }
}
