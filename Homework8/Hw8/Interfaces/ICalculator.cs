using Hw8.Common;

namespace Hw8.Interfaces;

public interface ICalculator
{
    double Calculate(ParseOutput data);
    double Plus(double val1, double val2);
    
    double Minus(double val1, double val2);
    
    double Multiply(double val1, double val2);
    
    double Divide(double val1, double val2);
}