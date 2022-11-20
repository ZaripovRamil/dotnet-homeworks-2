using Hw9.Dto;

namespace Hw9.Services.MathCalculator;

public class MathCalculator : IMathCalculator
{
    public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? query)
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