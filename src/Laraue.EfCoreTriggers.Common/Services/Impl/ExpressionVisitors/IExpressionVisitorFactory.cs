using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;

/// <summary>
/// Factory which will use the suitable <see cref="IExpressionVisitor{TExpression}"/>
/// depending on type of passed expression.
/// </summary>
public interface IExpressionVisitorFactory
{
    /// <summary>
    /// Visit expression and return <see cref="SqlBuilder"/>.
    /// </summary>
    /// <param name="expression">Expression to visit.</param>
    /// <param name="argumentTypes">Argument types to know what prefix should have each os passed arg of expression.</param>
    /// <param name="visitedMembers">Dictionary which collect all visited members.</param>
    /// <returns></returns>
    SqlBuilder Visit(
        Expression expression,
        ArgumentTypes argumentTypes,
        VisitedMembers visitedMembers);
}