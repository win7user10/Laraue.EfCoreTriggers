using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Builders.Providers;

namespace Laraue.EfCoreTriggers.Common.Converters.ExpressionCall
{
    public interface IExpressionCallConverter
    {
        bool IsApplicable(MethodCallExpression expression);

        SqlBuilder BuildSql(BaseExpressionProvider provider, MethodCallExpression expression, Dictionary<string, ArgumentType> argumentTypes);
    }
}