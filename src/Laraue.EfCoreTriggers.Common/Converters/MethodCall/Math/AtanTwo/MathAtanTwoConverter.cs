using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Extensions;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.AtanTwo
{
    public class MathAtanTwoConverter : BaseMathConverter
    {
        public override string MethodName => nameof(System.Math.Atan2);

        public override SqlBuilder BuildSql(BaseExpressionProvider provider, MethodCallExpression expression, Dictionary<string, ArgumentType> argumentTypes)
        {
            var argumentsSql = provider.GetMethodCallArgumentsSql(expression, argumentTypes);
            
            return new SqlBuilder($"ATAN2({argumentsSql[0]}, {argumentsSql[1]})")
                .MergeColumnsInfo(argumentsSql);
        }
    }
}
