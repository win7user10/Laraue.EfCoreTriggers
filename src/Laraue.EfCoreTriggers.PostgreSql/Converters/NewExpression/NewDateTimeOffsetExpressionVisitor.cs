using Laraue.EfCoreTriggers.Common.Converters.NewExpression;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.PostgreSql.Converters.NewExpression;

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
        return SqlBuilder.FromString("'0001-01-01'");
    }
}