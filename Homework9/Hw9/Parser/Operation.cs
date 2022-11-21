using System.Diagnostics.CodeAnalysis;

namespace Hw9.Parser;

public class Operation : Token
{
    public OperationType Type { get; set; }
    
    
    [ExcludeFromCodeCoverage]
    public int Priority
    {
        get
        {
            return Type switch
            {
                OperationType.Plus => 1,
                OperationType.Minus => 1,
                OperationType.Negate => 3,
                OperationType.Multiply => 2,
                OperationType.Divide => 2,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    public Operation(OperationType type)
    {
        Type = type;
    }
    
    [ExcludeFromCodeCoverage]
    public override string ToString() =>
        Type switch
        {
            OperationType.Plus => "+",
            OperationType.Minus => "-",
            OperationType.Multiply => "*",
            OperationType.Divide => "/",
            OperationType.Negate => "-",
            _ => throw new ArgumentOutOfRangeException()
        };

    public static implicit operator string(Operation op) => op.ToString();
}