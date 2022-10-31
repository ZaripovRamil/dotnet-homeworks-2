using System.Globalization;
using Hw8.Common;
using Hw8.Interfaces;

namespace Hw8.Services;

public class Parser:IParser
{

    private static bool TryParseOperation(string arg, out Operation operation)
    {
        switch(arg)
        {
            case "Plus": 
                operation = Operation.Plus;
                return true;
            case "Minus": 
                operation = Operation.Minus;
                return true;
            case "Multiply": 
                operation = Operation.Multiply;
                return true;
            case "Divide": 
                operation = Operation.Divide;
                return true;
            default:
                operation = Operation.Invalid;
                return false;
        }
    }

    private static bool TryParseDouble(string arg, out double val) =>
        double.TryParse(arg, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out val);

    public ParseOutput ParseCalcArguments(ParseInput input)
    {
        if (!(TryParseDouble(input.Val1,  out var val1)
              && TryParseDouble(input.Val2, out var val2)))
            throw new ArgumentException(Messages.InvalidNumberMessage);
        if (!TryParseOperation(input.Operation, out var operation))
            throw new InvalidOperationException(Messages.InvalidOperationMessage);
        return new ParseOutput(val1, operation, val2);
    }
}