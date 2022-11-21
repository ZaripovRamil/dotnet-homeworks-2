using System.Diagnostics.CodeAnalysis;

namespace Hw9.Parser;

public class Bracket:Token
{
    public Bracket(BracketType type)
    {
        Type = type;
    }

    public BracketType Type { get; }

    [ExcludeFromCodeCoverage]
    public override string ToString() =>
        Type switch
        {
            BracketType.Opening => "(",
            BracketType.Closing => ")",
            _ => throw new ArgumentOutOfRangeException()
        };
}