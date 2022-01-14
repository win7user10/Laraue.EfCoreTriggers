using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Internal;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall
{
    /// <summary>
    /// Converter for <see cref="MethodCallExpression"/>.
    /// </summary>
    public interface IMethodCallConverter
    {
        /// <summary>
        /// Should this converter be used to translate a <see cref="MethodCallExpression"/> to a SQL.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        bool IsApplicable(MethodCallExpression expression);

        /// <summary>
        /// Build a SQL for passed <see cref="MethodCallExpression"/>.
        /// </summary>
        /// <param name="visitor">Provider to build parts of SQL.</param>
        /// <param name="expression">Expression to parse.</param>
        /// <param name="argumentTypes">Argument types of the expression.</param>
        /// <param name="visitedMembers">Visited members.</param>
        /// <returns></returns>
        SqlBuilder BuildSql(
            IExpressionVisitor visitor,
            MethodCallExpression expression,
            ArgumentTypes argumentTypes,
            VisitedMembers visitedMembers);
    }
}