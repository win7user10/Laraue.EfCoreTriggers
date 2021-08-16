using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.Extensions
{
    public static class BaseExpressionProviderExtensions
    {
        public static SqlBuilder[] GetMethodCallArgumentsSql(this BaseExpressionProvider provider, MethodCallExpression expression, Dictionary<string, ArgumentType> argumentTypes)
        {
            return expression.Arguments.Select(argumentExpression => provider.GetExpressionSql(argumentExpression, argumentTypes)).ToArray();
        }
    }
}