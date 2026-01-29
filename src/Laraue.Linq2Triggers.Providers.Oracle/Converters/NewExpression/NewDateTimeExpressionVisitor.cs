using Laraue.Linq2Triggers.Core.Converters.NewExpression;
using Laraue.Linq2Triggers.Core.SqlGeneration;
using Laraue.Linq2Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Linq2Triggers.Providers.Oracle.Converters.NewExpression;

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
        return SqlBuilder.FromString("'0001-01-01'");
    }
}