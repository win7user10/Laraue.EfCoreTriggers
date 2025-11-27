using System;
using Laraue.Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Triggers.Core.Converters.NewExpression;

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