using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Atan2
{
    public abstract class BaseAtan2Converter : BaseMathConverter
    {
        public override string MethodName => nameof(System.Math.Atan2);

        protected abstract string SqlFunctionName { get; }

        public override SqlBuilder BuildSql(BaseExpressionProvider provider, MethodCallExpression expression, Dictionary<string, ArgumentType> argumentTypes)
        {
            var argumentsSql = provider.GetMethodCallArgumentsSql(expression, argumentTypes);
            
            return new SqlBuilder($"{SqlFunctionName}({argumentsSql[0]}, {argumentsSql[1]})")
                .MergeColumnsInfo(argumentsSql);
        }
    }
}