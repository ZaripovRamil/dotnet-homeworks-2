using System.Linq.Expressions;

namespace Hw11.Services.MathCalculator;

public class CalcVisitor
{
    private readonly TaskBasedCalculator _calculator = new();

    private void Visit(Expression node)
    {
        _calculator.Add((dynamic)node);
        foreach(var childNode in GetChildrenNodes((dynamic)node))
            Visit(childNode);
    }

    private static IEnumerable<Expression> GetChildrenNodes(BinaryExpression node)
        => new[] {node.Left, node.Right};

    private static IEnumerable<Expression> GetChildrenNodes(UnaryExpression node)
        => new[] {node.Operand};

    private static IEnumerable<Expression> GetChildrenNodes(ConstantExpression node)
        => Array.Empty<Expression>();

    public TaskBasedCalculator VisitWith(Expression node)
    {
        Visit(node);
        return _calculator;
    }
}