using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.Guid.NewGuid;

/// <summary>
/// Visitor for <see cref="System.Guid.NewGuid"/> method.
/// </summary>
public abstract class BaseNewGuidVisitor : BaseGuidVisitor
{
    /// <inheritdoc />
    protected override string MethodName => nameof(System.Guid.NewGuid);

    /// <inheritdoc />
    protected BaseNewGuidVisitor(IExpressionVisitorFactory visitorFactory) 
        : base(visitorFactory)
    {
    }

    /// <summary>
    /// Ceil function name in SQL.
    /// </summary>
    protected abstract string NewGuidSql { get; }

    /// <inheritdoc />
    public override SqlBuilder Visit(
        MethodCallExpression expression,
        VisitedMembers visitedMembers)
    {
        return SqlBuilder.FromString(NewGuidSql);
    }
}