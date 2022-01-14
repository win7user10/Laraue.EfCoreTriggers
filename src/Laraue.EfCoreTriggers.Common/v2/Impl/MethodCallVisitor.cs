using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Converters;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.v2.Internal;

namespace Laraue.EfCoreTriggers.Common.v2.Impl;

public class MethodCallVisitor : BaseExpressionVisitor<MethodCallExpression>
{
    private readonly AvailableConverters _converters;

    public MethodCallVisitor(IExpressionTreeVisitorFactory factory, AvailableConverters converters) 
        : base(factory)
    {
        _converters = converters;
    }

    public override SqlBuilder Visit(MethodCallExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
    {
        foreach (var converter in _converters.ExpressionCallConverters)
        {
            if (converter.IsApplicable(expression))
            {
                return converter.BuildSql(this, expression, argumentTypes, visitedMembers);
            }
        }

        throw new NotSupportedException($"Expression {expression.Method.Name} is not supported");
    }
}