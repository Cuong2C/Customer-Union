using System.Text;

namespace Customer_Union.Models
{
    public class GenrateTokenResponse
    {
        public string ClientCode { get; set; } = default!;
        public string Token { get; set; } = default!;
    }
}
