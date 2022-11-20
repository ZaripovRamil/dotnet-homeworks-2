using Hw9.Dto;

namespace Hw9.Services.MathCalculator;

public interface IMathCalculator
{
    public Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression);
}