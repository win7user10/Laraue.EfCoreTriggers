using System;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.NewExpression;

/// <inheritdoc />
public abstract class BaseNewDateTimeExpressionVisitor : BaseNewExpressionVisitor
{
    /// <inheritdoc />
    public BaseNewDateTimeExpressionVisitor(IExpressionVisitorFactory visitorFactory)
        : base(visitorFactory)
    {
    }

    /// <inheritdoc />
    protected override Type ReflectedType => typeof(DateTime);
}