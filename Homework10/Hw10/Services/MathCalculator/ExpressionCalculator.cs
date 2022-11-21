using System.Linq.Expressions;

namespace Hw10.Services.MathCalculator;

public static class ExpressionCalculator
{
    public static async Task<double> VisitAsync(Expression expression)
    {
        var children = expression switch
        {
            BinaryExpression be => new[] {be.Left, be.Right},
            UnaryExpression ue => new[] {ue.Operand},
            _ => Array.Empty<Expression>()
        };
        return await new CalcVisitor().VisitWith(expression).Calculate(expression, children);
    }
}