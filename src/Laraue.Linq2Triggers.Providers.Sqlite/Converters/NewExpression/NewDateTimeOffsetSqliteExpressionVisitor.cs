using Laraue.Linq2Triggers.Core.Converters.NewExpression;
using Laraue.Linq2Triggers.Core.SqlGeneration;
using Laraue.Linq2Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Linq2Triggers.Providers.Sqlite.Converters.NewExpression;

/// <inheritdoc />
public class NewDateTimeOffsetSqliteExpressionVisitor : BaseNewDateTimeOffsetExpressionVisitor
{
    /// <inheritdoc />
    public NewDateTimeOffsetSqliteExpressionVisitor(IExpressionVisitorFactory visitorFactory)
        : base(visitorFactory)
    {
    }

    /// <inheritdoc />
    public override SqlBuilder Visit(System.Linq.Expressions.NewExpression expression, VisitedMembers visitedMembers)
    {
        return SqlBuilder.FromString("'0001-01-01T00:00:00+00:00'");
    }
}