using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Extensions;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Cos
{
    public class MathCosConverter : BaseMathConverter
    {
        public override string MethodName => nameof(System.Math.Cos);

        public override SqlBuilder BuildSql(BaseExpressionProvider provider, MethodCallExpression expression, Dictionary<string, ArgumentType> argumentTypes)
        {
            var argument = expression.Arguments[0];
            var sqlBuilder = provider.GetExpressionSql(argument, argumentTypes);
            return new SqlBuilder(sqlBuilder.AffectedColumns, $"COS({sqlBuilder})");
        }
    }
}
