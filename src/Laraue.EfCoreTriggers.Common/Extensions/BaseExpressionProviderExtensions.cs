using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Internal;

namespace Laraue.EfCoreTriggers.Common.Extensions
{
    public static class BaseExpressionProviderExtensions
    {
        public static SqlBuilder[] VisitArguments(
            this IExpressionVisitor visitor,
            MethodCallExpression expression,
            ArgumentTypes argumentTypes,
            VisitedMembers visitedMembers)
        {
            return expression.Arguments
                .Select(argumentExpression => visitor
                    .Visit(argumentExpression, argumentTypes, visitedMembers))
                .ToArray();
        }
    }
}