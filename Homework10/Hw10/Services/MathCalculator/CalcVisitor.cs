using System.Linq.Expressions;

namespace Hw10.Services.MathCalculator;

public class CalcVisitor : ExpressionVisitor
{
    private readonly TaskBasedCalculator _calculator = new();

    protected override Expression VisitBinary(BinaryExpression node)
    {
        _calculator.Add(node);
        return base.VisitBinary(node);
    }

    protected override Expression VisitUnary(UnaryExpression node)
    {
        _calculator.Add(node);
        return base.VisitUnary(node);
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        _calculator.Add(node);
        return base.VisitConstant(node);
    }

    public TaskBasedCalculator VisitWith(Expression? node)
    {
        base.Visit(node);
        return _calculator;
    }
}