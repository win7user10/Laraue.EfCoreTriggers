using Laraue.EfCoreTriggers.Common.Converters.NewExpression;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.MySql.Converters.NewExpression;

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
        return SqlBuilder.FromString("'1000-01-01'");
    }
}