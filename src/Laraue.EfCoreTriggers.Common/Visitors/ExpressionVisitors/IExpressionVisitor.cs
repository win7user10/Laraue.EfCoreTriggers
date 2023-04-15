using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors
{
    /// <summary>
    /// Expression visitor which return SQL builder after expression visit.
    /// </summary>
    /// <typeparam name="TExpression"></typeparam>
    public interface IExpressionVisitor<in TExpression> where TExpression : Expression
    {
        /// <summary>
        /// Visit expression and return <see cref="SqlBuilder"/>.
        /// </summary>
        /// <param name="expression">Expression to visit.</param>
        /// <param name="argumentTypes">Argument types to know what prefix should have each os passed arg of expression.</param>
        /// <param name="visitedMembers">Dictionary which collect all visited members.</param>
        /// <returns></returns>
        SqlBuilder Visit(TExpression expression, VisitedMembers visitedMembers);
    }
}