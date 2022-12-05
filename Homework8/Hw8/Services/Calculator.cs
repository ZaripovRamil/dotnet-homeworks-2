using Hw8.Common;
using Hw8.Interfaces;
using static System.Double;

namespace Hw8.Services;

public class Calculator : ICalculator
{
    public double Calculate(ParseOutput data) =>
        data.Operation switch
        {
            Operation.Plus => Plus(data.Val1, data.Val2),
            Operation.Minus => Minus(data.Val1, data.Val2),
            Operation.Multiply => Multiply(data.Val1, data.Val2),
            Operation.Divide => Divide(data.Val1, data.Val2),
            _ => throw new InvalidOperationException(Messages.InvalidOperationMessage)
        };

    public double Plus(double val1, double val2) => val1 + val2;
    public double Minus(double val1, double val2) => val1 - val2;

    public double Multiply(double val1, double val2) => val1 * val2;

    public double Divide(double val1, double val2) =>
        val2 < Epsilon
            ? val1 < Epsilon
                ? NaN
                :throw new InvalidOperationException(Messages.DivisionByZeroMessage)
            : val1 / val2;
}