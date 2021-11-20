using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Abs
{
    public class MathAbsConverter : BaseMathConverter
    {
        public override string MethodName => nameof(System.Math.Abs);

        public override SqlBuilder BuildSql(BaseExpressionProvider provider, MethodCallExpression expression, Dictionary<string, ArgumentType> argumentTypes)
        {
            var argument = expression.Arguments[0];
            var sqlBuilder = provider.GetExpressionSql(argument, argumentTypes);
            return new SqlBuilder(sqlBuilder.AffectedColumns, $"ABS({sqlBuilder})");
        }
    }
}
