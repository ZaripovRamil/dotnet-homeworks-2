namespace Hw1;

public static class Parser
{
    public static void ParseCalcArguments(string[] args,
        out double val1,
        out CalculatorOperation operation,
        out double val2)
    {
        if (IsArgLengthSupported(args))
        {
            if (!(double.TryParse(args[0], out val1) && double.TryParse(args[2], out val2)))
                throw new ArgumentException();
            operation = ParseOperation(args[1]);
            if (operation == CalculatorOperation.Undefined) throw new InvalidOperationException();
        }
        else throw new ArgumentException();
    }

    private static bool IsArgLengthSupported(IReadOnlyCollection<string> args) => args.Count == 3;

    private static CalculatorOperation ParseOperation(string arg) => arg switch
    {
        "+" => CalculatorOperation.Plus,
        "-" => CalculatorOperation.Minus,
        "*" => CalculatorOperation.Multiply,
        "/" => CalculatorOperation.Divide,
        _ => CalculatorOperation.Undefined
    };
}