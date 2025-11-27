using Laraue.Linq2Triggers.Core.Converters.NewExpression;
using Laraue.Linq2Triggers.Core.SqlGeneration;
using Laraue.Linq2Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Linq2Triggers.Providers.SqlServer.Converters.NewExpression;

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