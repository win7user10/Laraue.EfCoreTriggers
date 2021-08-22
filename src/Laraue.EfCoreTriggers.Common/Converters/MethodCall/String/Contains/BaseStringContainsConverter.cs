using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Contains
{
    public abstract class BaseStringContainsConverter : BaseStringConverter
    {
        public abstract string SqlFunctionName { get; }

        /// <inheritdoc />
        public override string MethodName => nameof(string.Contains);

        /// <inheritdoc />
        public override SqlBuilder BuildSql(BaseExpressionProvider provider, MethodCallExpression expression, Dictionary<string, ArgumentType> argumentTypes)
        {
            var argumentSql = provider.GetMethodCallArgumentsSql(expression, argumentTypes)[0];

            var sqlBuilder = provider.GetExpressionSql(expression.Object, argumentTypes);
            return new SqlBuilder(sqlBuilder.AffectedColumns, $"{SqlFunctionName}({sqlBuilder}, {argumentSql}) > 0")
                .MergeColumnsInfo(argumentSql);
        }
    }
}