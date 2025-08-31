using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Longbeach.Domain.Entities;

public class ClientSource
{
    public string ClientCode { get; set; } = default!;
    public string ClientName { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}
