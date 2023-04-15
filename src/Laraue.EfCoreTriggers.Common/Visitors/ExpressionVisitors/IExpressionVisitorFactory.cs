using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors
{
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
        /// <param name="visitedMembers">Dictionary which collect all visited members.</param>
        /// <returns></returns>
        SqlBuilder Visit(
            Expression expression,
            VisitedMembers visitedMembers);

        /// <summary>
        /// Get expression visitor for the passed expression type.
        /// </summary>
        /// <typeparam name="TExpression"></typeparam>
        /// <returns></returns>
        IExpressionVisitor<TExpression> GetExpressionVisitor<TExpression>()
            where TExpression : Expression;
    }
}