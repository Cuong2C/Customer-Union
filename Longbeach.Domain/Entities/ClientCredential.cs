using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Longbeach.Domain.Entities;

public class ClientCredential
{
    public Guid Id { get; set; }
    public string ClientCode { get; set; } = default!;
    public string SecretHash { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public bool IsRevoked { get; set; }

}
