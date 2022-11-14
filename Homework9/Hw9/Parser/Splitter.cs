namespace Hw9.Parser;

using static ErrorMessages.MathErrorMessager;

public static class Splitter
{
    public static List<Token> SplitToTokens(string? expression)
    {
        if (string.IsNullOrEmpty(expression))
            throw new Exception(EmptyString);
        expression = expression.Replace(" ", "");
        if (!AllCharactersAreValid(expression, out var symbol))
            throw new Exception(UnknownCharacterMessage(symbol!.Value));
        var result = new List<Token>();
        var index = 0;
        while (index < expression.Length)
        {
            if (TryParseOperator(expression[index], out var opToken))
            {
                result.Add(opToken!);
                index += 1;
            }

            if (index < expression.Length)
                if (TryParseBracket(expression[index], out var brToken))
                {
                    result.Add(brToken!);
                    index += 1;
                }

            if (index < expression.Length)
                if (char.IsDigit(expression[index]))
                    result.Add(GetNumber(expression, ref index));
        }

        return result;
    }

    private static bool AllCharactersAreValid(string expression, out char? failing)
    {
        foreach (var symbol in expression.Where(symbol => !"0123456789+-*/.()".Contains(symbol)))
        {
            failing = symbol;
            return false;
        }

        failing = null;
        return true;
    }

    private static Number GetNumber(string expression, ref int index)
    {
        var start = index;
        while (index < expression.Length && char.IsDigit(expression[index]))
            index++;
        if (index < expression.Length && expression[index] == '.')
        {
            index++;
            while (index < expression.Length && char.IsDigit(expression[index]))
                index++;
            if (expression[index - 1] == '.') throw new Exception(NotNumberMessage(expression));
        }

        if (index >= expression.Length || IsBracket(expression[index]) || IsOperator(expression[index]))
            return new Number(double.Parse(expression.Substring(start, index - start)));
        while (index < expression.Length && !IsBracket(expression[index]) && !IsOperator(expression[index]))
            index++;
        throw new Exception(NotNumberMessage(expression.Substring(start, index - start)));
    }

    private static bool IsOperator(char s) => 
        "+-*/".Contains(s);

    private static bool IsBracket(char s) =>
        "()".Contains(s);


    private static bool TryParseBracket(char s, out Bracket? br)
    {
        br = s switch
        {
            '(' => new Bracket(BracketType.Opening),
            ')' => new Bracket(BracketType.Closing),
            _ => null
        };
        return br != null;
    }

    private static bool TryParseOperator(char s, out Operation? op)
    {
        op = s switch
        {
            '+' => new Operation(OperationType.Plus),
            '-' => new Operation(OperationType.Minus),
            '*' => new Operation(OperationType.Multiply),
            '/' => new Operation(OperationType.Divide),
            _ => null
        };
        return op != null;
    }
}