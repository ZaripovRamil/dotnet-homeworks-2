using Hw10.Dto;

namespace Hw10.Services;

public interface IMathCalculator
{
    public Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression);
}