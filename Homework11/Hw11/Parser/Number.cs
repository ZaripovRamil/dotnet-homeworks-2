using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Hw11.Parser;

public class Number:Token
{
    public Number(double value)
    {
        Value = value;
    }

    public double Value { get; }

    [ExcludeFromCodeCoverage]
    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}