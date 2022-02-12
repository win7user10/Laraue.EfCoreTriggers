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
    private readonly IMethodCallVisitor[] _converters;

    /// <inheritdoc />
    public MethodCallExpressionVisitor(IEnumerable<IMethodCallVisitor> methodCallVisitors)
    {
        _converters = methodCallVisitors.Reverse().ToArray();
    }

    /// <inheritdoc />
    public override SqlBuilder Visit(MethodCallExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
    {
        foreach (var converter in _converters)
        {
            if (converter.IsApplicable(expression))
            {
                return converter.Visit(expression, argumentTypes, visitedMembers);
            }
        }

        throw new NotSupportedException($"Expression {expression.Method.Name} is not supported");
    }
}