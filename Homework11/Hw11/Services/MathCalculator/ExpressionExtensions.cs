using System.Linq.Expressions;

namespace Hw11.Services.MathCalculator;

public static class ExpressionExtensions
{
    private static IEnumerable<Expression> GetChildrenNodes(this BinaryExpression node)
        => new[] {node.Left, node.Right};

    private static IEnumerable<Expression> GetChildrenNodes(this UnaryExpression node)
        => new[] {node.Operand};

    private static IEnumerable<Expression> GetChildrenNodes(this ConstantExpression node)
        => Array.Empty<Expression>();

    public static IEnumerable<Expression> GetChildrenNodes(this Expression expression)
    =>
        GetChildrenNodes((dynamic) expression);
}