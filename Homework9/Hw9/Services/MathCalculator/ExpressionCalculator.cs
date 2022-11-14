﻿using System.Linq.Expressions;

namespace Hw9.Services.MathCalculator;

public static class ExpressionCalculator
{
    public static async Task<double> VisitAsync(Expression expression)
        => await new CalcVisitor().VisitWith(expression)[expression].Value;
}