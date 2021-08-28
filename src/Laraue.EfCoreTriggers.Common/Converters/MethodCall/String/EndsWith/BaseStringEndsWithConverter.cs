using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.EndsWith
{
    public abstract class BaseStringEndsWithConverter : BaseStringConverter
    {
        /// <inheritdoc />
        public override string MethodName => nameof(string.EndsWith);

        /// <inheritdoc />
        public override SqlBuilder BuildSql(BaseExpressionProvider provider, MethodCallExpression expression, Dictionary<string, ArgumentType> argumentTypes)
        {
            var argumentSql = provider.GetMethodCallArgumentsSql(expression, argumentTypes)[0];
            var sqlBuilder = provider.GetExpressionSql(expression.Object, argumentTypes);
            return new SqlBuilder(sqlBuilder.AffectedColumns, $"{sqlBuilder} LIKE {BuildEndSql(argumentSql)}")
                .MergeColumnsInfo(argumentSql);
        }

        public abstract string BuildEndSql(string argumentSql);
    }
}