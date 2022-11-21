using System.Linq.Expressions;

namespace Hw11.Services.MathCalculator;

public class CalcVisitor
{
    private readonly TaskBasedCalculator _calculator = new();

    private void Visit(Expression node)
    {
        _calculator.Add(node);
        foreach(var childNode in node.GetChildrenNodes())
            Visit(childNode);
    }

    public TaskBasedCalculator VisitWith(Expression node)
    {
        Visit(node);
        return _calculator;
    }
}