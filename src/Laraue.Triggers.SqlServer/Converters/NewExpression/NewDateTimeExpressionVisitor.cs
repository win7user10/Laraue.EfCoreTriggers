using Laraue.Triggers.Core.Converters.NewExpression;
using Laraue.Triggers.Core.SqlGeneration;
using Laraue.Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Triggers.SqlServer.Converters.NewExpression;

/// <inheritdoc />
public class NewDateTimeExpressionVisitor : BaseNewDateTimeExpressionVisitor
{
    /// <inheritdoc />
    public NewDateTimeExpressionVisitor(IExpressionVisitorFactory visitorFactory)
        : base(visitorFactory)
    {
    }

    /// <inheritdoc />
    public override SqlBuilder Visit(System.Linq.Expressions.NewExpression expression, VisitedMembers visitedMembers)
    {
        return SqlBuilder.FromString("'1753-01-01'");
    }
}