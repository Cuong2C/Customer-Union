using Dapper;
using System.Data;

namespace Longbeach.UnitTests;

// This class uses for test with sqlite only
public class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
{
    public override Guid Parse(object value)
    {
        return value switch
        {
            Guid guid => guid,
            string s => Guid.Parse(s),
            _ => throw new InvalidCastException($"Cannot convert {value.GetType()} to Guid")
        };
    }

    public override void SetValue(IDbDataParameter parameter, Guid value)
    {
        parameter.Value = value.ToString();
    }
}
