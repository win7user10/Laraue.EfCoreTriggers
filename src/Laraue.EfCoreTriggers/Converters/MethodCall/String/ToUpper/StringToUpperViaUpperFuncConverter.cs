using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.SqlGeneration;
using Laraue.EfCoreTriggers.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Converters.MethodCall.String.ToUpper
{
    public class StringToUpperViaUpperFuncConverter : BaseStringConverter
    {
        /// <inheritdoc />
        public override string MethodName => nameof(string.ToUpper);

        /// <inheritdoc />
        public override SqlBuilder BuildSql(BaseExpressionProvider provider, MethodCallExpression expression, Dictionary<string, ArgumentType> argumentTypes)
        {
            var sqlBuilder = provider.GetExpressionSql(expression.Object, argumentTypes);
            return new(sqlBuilder.AffectedColumns, $"UPPER({sqlBuilder})");
        }
    }
}