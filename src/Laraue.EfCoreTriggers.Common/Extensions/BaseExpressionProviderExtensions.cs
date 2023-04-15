using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Extensions
{
    public static class BaseExpressionProviderExtensions
    {
        /// <summary>
        /// Visit each argument of <see cref="MethodCallExpression"/> and
        /// generates a SQL for each of them.
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="expression"></param>
        /// <param name="visitedMembers"></param>
        /// <returns></returns>
        public static SqlBuilder[] VisitArguments(
            this IExpressionVisitorFactory visitor,
            MethodCallExpression expression,
            VisitedMembers visitedMembers)
        {
            return expression.Arguments
                .Select(argumentExpression => visitor
                    .Visit(argumentExpression, visitedMembers))
                .ToArray();
        }
    }
}