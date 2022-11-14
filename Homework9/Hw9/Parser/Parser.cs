using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using static Hw9.Parser.Splitter;
using static Hw9.ErrorMessages.MathErrorMessager;
using Exception = System.Exception;

namespace Hw9.Parser;

public static class Parser
{
    public static Expression GetExpression(string? query)
    {
        var tokens = SplitToTokens(query);
        if (!IsExpressible(tokens, out var message))
            throw new Exception(message);
        return GetExpression(tokens, 0, tokens.Count);
    }

    private static Expression GetExpression(List<Token> tokens, int start, int length)
    {
        var expressionsStack = new Stack<Expression>();
        var operationsStack = new Stack<Operation>();
        for (var index = start; index < start + length; index++)
        {
            var token = tokens[index];
            switch (token)
            {
                case Number number:
                    expressionsStack.Push(Expression.Constant(number.Value));
                    continue;
                case Bracket br:
                {
                    switch (br)
                    {
                        case {Type: BracketType.Opening}:
                        {
                            var expressionStart = index + 1;
                            var bracketDepth = 1;
                            while (bracketDepth != 0)
                                if (tokens[++index] is Bracket nextBr)
                                    bracketDepth += nextBr.Type == BracketType.Opening ? 1 : -1;
                            var expressionLength = index - expressionStart;
                            expressionsStack.Push(GetExpression(tokens, expressionStart, expressionLength));
                            break;
                        }
                    }

                    break;
                }
                case Operation op:
                {
                    CalculatePrioritizedExpression(op, expressionsStack, operationsStack);
                    operationsStack.Push(op);
                    break;
                }
            }
        }

        while (operationsStack.Count > 0)
            expressionsStack.Push(ProvideExpressionByOp(operationsStack.Pop(), expressionsStack));
        return expressionsStack.Pop();
    }

    private static void CalculatePrioritizedExpression(Operation op, Stack<Expression> expressionsStack,
        Stack<Operation> operationsStack)
    {
        while (StackHasMorePrioritizedOperation(operationsStack, op))
            expressionsStack.Push(ProvideExpressionByOp(operationsStack.Pop(), expressionsStack));
    }

    private static Expression ProvideExpressionByOp(Operation op, Stack<Expression> expressionsStack)
    {
        switch (op.Type)
        {
            case OperationType.Negate:
                return Expression.Negate(expressionsStack.Pop());
            case OperationType.Plus:
                return Expression.Add(expressionsStack.Pop(), expressionsStack.Pop());
            case OperationType.Minus:
            {
                var substracted = expressionsStack.Pop();
                var substractable = expressionsStack.Pop();
                return Expression.Subtract(substractable, substracted);
            }

            case OperationType.Multiply:
                return Expression.Multiply(expressionsStack.Pop(), expressionsStack.Pop());
            case OperationType.Divide:
            {
                var divisor = expressionsStack.Pop();
                var divisible = expressionsStack.Pop();
                return Expression.Divide(divisible, divisor);
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static bool StackHasMorePrioritizedOperation(Stack<Operation> operationStack, Operation op)
        => operationStack.Count > 0 && operationStack.Peek().Priority >= op.Priority;


    private static bool IsExpressible(List<Token> tokens, out string message)
    {
        if (IsStartingWithBinaryOperation(tokens, out message))
            return false;
        if (IsEndingWithOperation(tokens, out message)) return false;
        var bracketsDepth = 0;
        Token? lastToken = null;
        foreach (var token in tokens)
            if (!IsExpressibleToken(token, ref lastToken, ref bracketsDepth, out message))
                return false;
        if (bracketsDepth != 0)
        {
            message = IncorrectBracketsNumber;
            return false;
        }

        message = "";
        return true;
    }

    private static bool IsExpressibleToken(Token token, ref Token? lastToken, ref int bracketsDepth, out string message)
    {
        switch (token)
        {
            case Bracket br when IsOperationBeforeClosingBracket(br, lastToken, out message):
                return false;
            case Bracket br:
            {
                bracketsDepth += br.Type == BracketType.Opening ? 1 : -1;
                if (bracketsDepth >= 0)
                {
                    lastToken = token;
                    message = "";
                    return true;
                }

                message = IncorrectBracketsNumber;
                return false;
            }
            case Operation thisOp when IsTwoOperationsInRow(thisOp, lastToken, out message):
                return false;
            case Operation thisOp when IsBinaryOperationAfterOpeningBracket(thisOp, lastToken, out message):
                return false;
            default:
                lastToken = token;
                break;
        }

        message = "";
        return true;
    }

    private static bool IsOperationBeforeClosingBracket(Bracket br, Token? lastToken, out string message)
    {
        if (br.Type is BracketType.Closing && lastToken is Operation op)
        {
            message = OperationBeforeParenthesisMessage(op);
            return true;
        }

        message = "";
        return false;
    }

    private static bool IsBinaryOperationAfterOpeningBracket(Operation thisOp, Token? lastToken, out string message)
    {
        if (lastToken is Bracket {Type: BracketType.Opening})
        {
            if (thisOp.Type == OperationType.Minus) thisOp.Type = OperationType.Negate;
            else
            {
                message = InvalidOperatorAfterParenthesisMessage(thisOp);
                return true;
            }
        }

        message = "";
        return false;
    }

    private static bool IsTwoOperationsInRow(Operation thisOp, Token? lastToken, out string message)
    {
        if (lastToken is Operation lastOp)
        {
            message = TwoOperationInRowMessage(lastOp, thisOp);
            return true;
        }

        message = "";
        return false;
    }

    private static bool IsEndingWithOperation(List<Token> tokens, out string message)
    {
        if (tokens[^1] is Operation)
        {
            message = EndingWithOperation;
            return true;
        }

        message = "";
        return false;
    }

    private static bool IsStartingWithBinaryOperation(List<Token> tokens, out string message)
    {
        if (tokens[0] is Operation thisOp)
        {
            if (thisOp.Type == OperationType.Minus) thisOp.Type = OperationType.Negate;
            else
            {
                message = StartingWithOperation;
                return true;
            }
        }

        message = "";
        return false;
    }
}