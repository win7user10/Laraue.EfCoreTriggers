using Laraue.Linq2Triggers.Core.Converters.NewExpression;
using Laraue.Linq2Triggers.Core.SqlGeneration;
using Laraue.Linq2Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Linq2Triggers.Providers.Oracle.Converters.NewExpression;

/// <inheritdoc />
public class NewDateTimeOffsetExpressionVisitor : BaseNewDateTimeOffsetExpressionVisitor
{
    /// <inheritdoc />
    public NewDateTimeOffsetExpressionVisitor(IExpressionVisitorFactory visitorFactory)
        : base(visitorFactory)
    {
    }

    /// <inheritdoc />
    public override SqlBuilder Visit(System.Linq.Expressions.NewExpression expression, VisitedMembers visitedMembers)
    {
        return SqlBuilder.FromString("TO_DATE('1000-01-01', 'YYYY-MM-DD')");
    }
}