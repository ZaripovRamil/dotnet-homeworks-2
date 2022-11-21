namespace Hw11.Services.MathCalculator;

public class MathCalculator : IMathCalculator
{
    public async Task<double> CalculateMathExpressionAsync(string? expression)
    {
        return await ExpressionCalculator.VisitAsync(
                Parser.Parser.GetExpression(expression));
    }
}