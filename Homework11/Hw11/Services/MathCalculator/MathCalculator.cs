using Hw11.Exceptions;

namespace Hw11.Services.MathCalculator;

public class MathCalculator : IMathCalculator
{
    private readonly IExceptionHandler _handler;

    public MathCalculator(IExceptionHandler handler)
    {
        _handler = handler;
    }

    public async Task<double> CalculateMathExpressionAsync(string? expression)
    {
        try
        {
            return await ExpressionCalculator.VisitAsync(
                Parser.Parser.GetExpression(expression));
        }
        catch (Exception e)
        {
            _handler.HandleException(e);
            throw;
        }
    }
}