namespace Hw11.Services.MathCalculator;

public interface IMathCalculator
{ 
    Task<double> CalculateMathExpressionAsync(string? expression);
}