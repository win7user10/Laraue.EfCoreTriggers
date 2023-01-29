using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;

/// <inheritdoc />
public class MethodCallExpressionVisitor : BaseExpressionVisitor<MethodCallExpression>
{
    private readonly IMethodCallVisitor[] _visitors;

    /// <inheritdoc />
    public MethodCallExpressionVisitor(IEnumerable<IMethodCallVisitor> methodCallVisitors)
    {
        _visitors = methodCallVisitors.Reverse().ToArray();
    }

    /// <inheritdoc />s
    public override SqlBuilder Visit(MethodCallExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
    {
        var visitor = GetVisitor(expression);
        
        return visitor.Visit(expression, argumentTypes, visitedMembers);
    }

    private IMethodCallVisitor GetVisitor(MethodCallExpression expression)
    {
        foreach (var converter in _visitors)
        {
            if (converter.IsApplicable(expression))
            {
                return converter;
            }
        }
        
        throw new NotSupportedException($"Expression {expression.Method.Name} is not supported");
    }
}