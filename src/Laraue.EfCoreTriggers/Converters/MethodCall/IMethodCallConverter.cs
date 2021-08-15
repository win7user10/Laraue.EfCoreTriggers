using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.SqlGeneration;
using Laraue.EfCoreTriggers.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Converters.MethodCall
{
    public interface IMethodCallConverter
    {
        bool IsApplicable(MethodCallExpression expression);

        SqlBuilder BuildSql(BaseExpressionProvider provider, MethodCallExpression expression, Dictionary<string, ArgumentType> argumentTypes);
    }
}