using System.Linq;
using System.Linq.Expressions;
using Laraue.Triggers.Core.SqlGeneration;
using Laraue.Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Triggers.Core.Extensions
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