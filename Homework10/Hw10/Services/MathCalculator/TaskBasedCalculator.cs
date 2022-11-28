using System.Linq.Expressions;
using Hw10.ErrorMessages;

namespace Hw10.Services.MathCalculator;

public class TaskBasedCalculator
{
    private readonly Dictionary<Expression, Lazy<Task<double>>> _expressionTaskHolder = new();

    public void Add(BinaryExpression expression)
    {
        _expressionTaskHolder.Add(expression,
            new Lazy<Task<double>>(async ()=>await Calculate(expression, expression.Left, expression.Right)));
    }

    public void Add(UnaryExpression expression)
    {
        _expressionTaskHolder.Add(expression,
            new Lazy<Task<double>>(async ()=> await Calculate(expression, expression.Operand)));
    }

    public void Add(ConstantExpression expression)
    {
        _expressionTaskHolder.Add(expression,
            new Lazy<Task<double>>(Task.FromResult((double) expression.Value!)));
    }

    public async Task<double> Calculate(Expression expression, params Expression[] expressions)
    {
        var tasks = expressions
            .Select(exp =>  _expressionTaskHolder[exp].Value)
            .ToArray();
        await Task.WhenAll(tasks);
        var values = tasks.Select(task => task.Result)
            .ToArray();
        if (expression is ConstantExpression) return await _expressionTaskHolder[expression].Value;
        await Task.Delay(1000);
        return Calculate(expression, values);
    }

    private static double Calculate(Expression expression, params double[] values)
    {
        return expression switch
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
    }
}