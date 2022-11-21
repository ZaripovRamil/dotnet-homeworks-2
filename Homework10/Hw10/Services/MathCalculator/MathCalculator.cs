using Hw10.Dto;


namespace Hw10.Services.MathCalculator;

public class MathCalculator : IMathCalculator
{
    public virtual async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? query)
    {
        try
        {
            return new CalculationMathExpressionResultDto(
                await ExpressionCalculator.VisitAsync(
                    Parser.Parser.GetExpression(query)));
        }
        catch (Exception e)
        {
            return new CalculationMathExpressionResultDto(e.Message);
        }
    }
}