using System.Linq.Expressions;
using Hw9.ErrorMessages;

namespace Hw9.Services.MathCalculator;

public class CalcVisitor : ExpressionVisitor
{
    private readonly Dictionary<Expression, Lazy<Task<double>>> _expressionTaskHolder = new();

    protected override Expression VisitBinary(BinaryExpression node)
    {
        _expressionTaskHolder[node] =
            new Lazy<Task<double>>(async () =>
            {
                
                await Task.WhenAll(_expressionTaskHolder[node.Left].Value, _expressionTaskHolder[node.Right].Value);
                await Task.Delay(1000);
                return Calculate(node, await _expressionTaskHolder[node.Left].Value,
                    await _expressionTaskHolder[node.Right].Value);
            });
        return base.VisitBinary(node);
    }

    protected override Expression VisitUnary(UnaryExpression node)
    {
        _expressionTaskHolder[node] =
            new Lazy<Task<double>>(async () =>
            {
                await _expressionTaskHolder[node.Operand].Value;
                await Task.Delay(1000);
                return Calculate(node, await _expressionTaskHolder[node.Operand].Value);
            });
        return base.VisitUnary(node);
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        _expressionTaskHolder[node] =
            new Lazy<Task<double>>(() =>
                Task.FromResult((double) node.Value!));
        return base.VisitConstant(node);
    }


    private static double Calculate(Expression expression, params double[] values) =>
        expression switch
        {
            {NodeType: ExpressionType.Add} => values[0] + values[1],
            {NodeType: ExpressionType.Subtract} => values[0] - values[1],
            {NodeType: ExpressionType.Multiply} => values[0] * values[1],
            {NodeType: ExpressionType.Divide} =>
                values[1] >= double.Epsilon
                    ? values[0] / values[1]
                    : throw new DivideByZeroException(MathErrorMessager.DivisionByZero),
            {NodeType: ExpressionType.Negate} => -values[0],
            _ => throw new ArgumentOutOfRangeException()
        };

    public Dictionary<Expression, Lazy<Task<double>>> VisitWith(Expression? node)
    {
        base.Visit(node);
        return _expressionTaskHolder;
    }
}