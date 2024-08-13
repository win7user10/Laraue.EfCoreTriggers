using System;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.NewExpression;

/// <inheritdoc />
public abstract class BaseNewDateTimeOffsetExpressionVisitor : BaseNewExpressionVisitor
{
    /// <inheritdoc />
    public BaseNewDateTimeOffsetExpressionVisitor(IExpressionVisitorFactory visitorFactory)
        : base(visitorFactory)
    {
    }

    /// <inheritdoc />
    protected override Type ReflectedType => typeof(DateTimeOffset);
}